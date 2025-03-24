using Dapper;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts;
using SPbSTU.OPD.ASAP.Core.Domain.Models;
using SPbSTU.OPD.ASAP.Core.Domain.ValueObjects;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Entities;

namespace SPbSTU.OPD.ASAP.Core.Infrastructure.Repositories;

public class OutboxPointsRepository : PgRepository, IOutboxPointsRepository
{
    protected OutboxPointsRepository(string connectionString) : base(connectionString)
    {
    }

    public async Task<List<long>> Create(List<OutboxPointsCreateModel> points, CancellationToken token)
    {
        const string sqlQuery =
            """
            insert into outbox_points (points, date, course_id, student_position, assignment_position, is_sent)
            select points, date, course_id, student_position, assignment_position, is_sent
              from unnest(@Points)
            returning id;
            """;

        var pointsEntity = points.Select(MapToEntity).ToList();

        await using var connection = await GetConnection();
        var ids = await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                new
                {
                    Points = pointsEntity
                },
                cancellationToken: token));
        return ids.ToList();
    }

    public async Task<List<OutboxPointsGetModel>> GetNotSent(CancellationToken token)
    {
        const string sqlQuery =
            """
            select id
                 , points
                 , date
                 , course_id
                 , student_position
                 , assignment_position
                 , is_sent
              from outbox_points
             where is_sent = false;
            """;

        await using var connection = await GetConnection();
        var points = await connection.QueryAsync<OutboxPointsEntityV1>(
            new CommandDefinition(
                sqlQuery,
                cancellationToken: token));
        return points.Select(MapToModel).ToList();
    }

    public async Task UpdateSent(List<long> sentPointsIds, CancellationToken token)
    {
        var sqlQuery =
            """
            update outbox_points
               set is_sent = true
            """;

        var conditions = new List<string>();
        var @params = new DynamicParameters();

        if (sentPointsIds.Count != 0)
        {
            sentPointsIds.Sort();
            conditions.Add($"id = ANY(@SentIds)");
            @params.Add($"SentIds", sentPointsIds);
        }

        var cmd = new CommandDefinition(
            sqlQuery + $" WHERE {string.Join(" AND ", conditions)} ",
            @params,
            commandTimeout: DefaultTimeoutInSeconds,
            cancellationToken: token);

        await using var connection = await GetConnection();
        await connection.ExecuteAsync(cmd);
    }

    private static OutboxPointsEntityV1 MapToEntity(OutboxPointsCreateModel pointsCreateModel)
    {
        return new OutboxPointsEntityV1(0, pointsCreateModel.Points, pointsCreateModel.Date, pointsCreateModel.CourseId, pointsCreateModel.StudentPosition.Cell,
            pointsCreateModel.AssignmentPosition.Cell, false);
    }

    private static OutboxPointsGetModel MapToModel(OutboxPointsEntityV1 points)
    {
        return new OutboxPointsGetModel(points.Id, points.Points, points.Date, points.CourseId,
            new Position(points.StudentPosition, points.CourseId),
            new Position(points.AssignmentPosition, points.CourseId));
    }
}