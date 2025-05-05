using Dapper;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;
using SPbSTU.OPD.ASAP.Core.Domain.Models;
using SPbSTU.OPD.ASAP.Core.Domain.Models.Outbox;
using SPbSTU.OPD.ASAP.Core.Domain.Models.Outbox.Points;
using SPbSTU.OPD.ASAP.Core.Domain.ValueObjects;
using SPbSTU.OPD.ASAP.Core.Persistence.Entities;

namespace SPbSTU.OPD.ASAP.Core.Persistence.Repositories;

public class OutboxPointsRepository(string connectionString) : PgRepository(connectionString), IOutboxPointsRepository
{
    public async Task<List<long>> Create(List<OutboxPointsCreateModel> points, CancellationToken token)
    {
        const string sqlQuery =
            """
            insert into outbox_points (points, date, course_id, student_position_cell, student_position_spreadsheet_id, assignment_position_cell, assignment_position_spreadsheet_id, is_sent)
            select points, date, course_id, student_position_cell, student_position_spreadsheet_id, assignment_position_cell, assignment_position_spreadsheet_id, is_sent
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
                 , student_position_cell
                 , student_position_spreadsheet_id
                 , assignment_position_cell
                 , assignment_position_spreadsheet_id
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
        const string sqlQuery =
            """
            update outbox_points
               set is_sent = true
             where id = ANY(@SentIds)
            """;

        await using var connection = await GetConnection();
        await connection.ExecuteAsync(new CommandDefinition(
            sqlQuery,
            new { SentIds = sentPointsIds },
            cancellationToken: token));
    }

    private static OutboxPointsEntityV1 MapToEntity(OutboxPointsCreateModel pointsCreateModel)
    {
        return new OutboxPointsEntityV1
        {
            Id = 0, Points = pointsCreateModel.Points, Date = pointsCreateModel.Date,
            CourseId = pointsCreateModel.CourseId,
            StudentPositionCell = pointsCreateModel.StudentPosition.Cell,
            StudentPositionSpreadsheetId = pointsCreateModel.StudentPosition.SpreadSheetId,
            AssignmentPositionCell = pointsCreateModel.AssignmentPosition.Cell,
            AssignmentPositionSpreadsheetId = pointsCreateModel.AssignmentPosition.SpreadSheetId, IsSent = false
        };
    }

    private static OutboxPointsGetModel MapToModel(OutboxPointsEntityV1 points)
    {
        return new OutboxPointsGetModel(points.Id, points.Points, points.Date, points.CourseId,
            new Position(points.StudentPositionCell, points.StudentPositionSpreadsheetId),
            new Position(points.AssignmentPositionCell, points.AssignmentPositionSpreadsheetId));
    }
}