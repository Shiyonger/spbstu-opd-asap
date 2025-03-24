using Dapper;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts;
using SPbSTU.OPD.ASAP.Core.Domain.Models;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Entities;

namespace SPbSTU.OPD.ASAP.Core.Infrastructure.Repositories;

public class OutboxQueueRepository : PgRepository, IOutboxQueueRepository
{
    protected OutboxQueueRepository(string connectionString) : base(connectionString)
    {
    }

    public async Task<List<long>> Create(List<OutboxQueue> queueQuery, CancellationToken token)
    {
        const string sqlQuery =
            """
            insert into outbox_queue (link, mentor_id, assignment_id, submission_id, is_sent)
            select link, mentor_id, assignment_id, submission_id, is_sent
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

    public async Task<List<OutboxQueue>> GetNotSent(CancellationToken token)
    {
        const string sqlQuery =
            """
            select link
                 , mentor_id
                 , assignment_id
                 , submission_id
                 , is_sent
              from outbox_queue
             where is_sent = false;
            """;

        await using var connection = await GetConnection();
        var points = await connection.QueryAsync<OutboxQueueEntityV1>(
            new CommandDefinition(
                sqlQuery,
                cancellationToken: token));
        return points.Select(MapToModel).ToList();
    }

    public async Task UpdateSent(List<long> queueQueryIds, CancellationToken token)
    {
        var sqlQuery =
            """
            update outbox_queue
               set is_sent = true
            """;

        var conditions = new List<string>();
        var @params = new DynamicParameters();

        if (queueQueryIds.Count != 0)
        {
            conditions.Add($"id = ANY(@SentIds)");
            @params.Add($"SentIds", queueQueryIds);
        }

        var cmd = new CommandDefinition(
            sqlQuery + $" WHERE {string.Join(" AND ", conditions)} ",
            @params,
            commandTimeout: DefaultTimeoutInSeconds,
            cancellationToken: token);

        await using var connection = await GetConnection();
        await connection.ExecuteAsync(cmd);
    }

    private static OutboxQueueEntityV1 MapToEntity(OutboxQueue queueQuery)
    {
        return new OutboxQueueEntityV1(queueQuery.Link, queueQuery.MentorId, queueQuery.AssignmentId,
            queueQuery.SubmissionId, false);
    }

    private static OutboxQueue MapToModel(OutboxQueueEntityV1 queueQuery)
    {
        return new OutboxQueue(queueQuery.Link, queueQuery.MentorId, queueQuery.AssignmentId, queueQuery.SubmissionId);
    }
}