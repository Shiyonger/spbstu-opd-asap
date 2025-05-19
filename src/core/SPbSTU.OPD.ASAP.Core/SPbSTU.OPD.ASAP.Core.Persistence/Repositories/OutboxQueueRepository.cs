using Dapper;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;
using SPbSTU.OPD.ASAP.Core.Domain.Models;
using SPbSTU.OPD.ASAP.Core.Domain.Models.Outbox;
using SPbSTU.OPD.ASAP.Core.Domain.Models.Outbox.Queue;
using SPbSTU.OPD.ASAP.Core.Persistence.Entities;

namespace SPbSTU.OPD.ASAP.Core.Persistence.Repositories;

public class OutboxQueueRepository(string connectionString) : PgRepository(connectionString), IOutboxQueueRepository
{
    public async Task<List<long>> Create(List<OutboxQueueCreateModel> queueQuery, CancellationToken token)
    {
        const string sqlQuery =
            """
            insert into outbox_queue (link, mentor_id, assignment_id, submission_id, action, is_sent)
            select link, mentor_id, assignment_id, submission_id, action, is_sent
              from unnest(@QueueQueries)
            returning id;
            """;

        var queueEntity = queueQuery.Select(MapToEntity).ToList();

        await using var connection = await GetConnection();
        var ids = await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                new
                {
                    QueueQueries = queueEntity
                },
                cancellationToken: token));
        return ids.ToList();
    }

    public async Task<List<OutboxQueueGetModel>> GetNotSent(CancellationToken token)
    {
        const string sqlQuery =
            """
            select q.id as id
                 , q.link as link
                 , st.id as student_id
                 , u2.name as student_name
                 , st.group_id as group_id
                 , a.spreadsheet_id as spreadsheet_id
                 , q.submission_id as submission_id
                 , s.updated_at as submission_date
                 , q.action
              from outbox_queue q
              join mentors m on q.mentor_id = m.id
              join users u on u.id = m.user_id
              join assignments a on q.assignment_id = a.id
              join submissions s on q.submission_id = s.id
              join students st on st.id = s.student_id
              join users u2 on u2.id = st.user_id
             where is_sent = false;
            """;

        await using var connection = await GetConnection();
        var points = await connection.QueryAsync<OutboxQueueGetModel>(
            new CommandDefinition(
                sqlQuery,
                cancellationToken: token));
        return points.ToList();
    }

    public async Task UpdateSent(List<long> queueQueryIds, CancellationToken token)
    {
        const string sqlQuery = 
            """
            update outbox_queue
               set is_sent = true
             where id = ANY(@SentIds);
            """;

        await using var connection = await GetConnection();
        await connection.ExecuteAsync(new CommandDefinition(
            sqlQuery,
            new { SentIds = queueQueryIds },
            cancellationToken: token));
    }

    private static OutboxQueueEntityV1 MapToEntity(OutboxQueueCreateModel queueCreateModelQuery)
    {
        return new OutboxQueueEntityV1
        {
            Id = 0, Link = queueCreateModelQuery.Link, MentorId = queueCreateModelQuery.MentorId,
            AssignmentId = queueCreateModelQuery.AssignmentId, SubmissionId = queueCreateModelQuery.SubmissionId,
            Action = (int)queueCreateModelQuery.Action, IsSent = false
        };
    }
}