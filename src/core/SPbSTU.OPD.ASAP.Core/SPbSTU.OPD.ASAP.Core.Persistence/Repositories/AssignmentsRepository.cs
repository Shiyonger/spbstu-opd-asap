using Dapper;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;
using SPbSTU.OPD.ASAP.Core.Domain.Models;

namespace SPbSTU.OPD.ASAP.Core.Persistence.Repositories;

public class AssignmentsRepository(string connectionString) : PgRepository(connectionString), IAssignmentsRepository
{
    public async Task<List<Assignment>> GetByCourseIdForMentor(long userId, long courseId, CancellationToken ct)
    {
        const string sqlQuery =
            """
            select a.id as id
                 , a.title as title
                 , a.description as description
                 , a.max_points as max_points
                 , a.due_to as due_to
                 , a.spreadsheet_link as link
              from users u
              join student_courses sc on sc.user_id = u.id
              join courses c on c.id = sc.course_id
              join assignments a on a.course_id = c.id
             where u.id = @UserId
               and c.id = @CourseId;
            """;

        await using var connection = await GetConnection();
        var assignments = await connection.QueryAsync<Assignment>(
            new CommandDefinition(
                sqlQuery,
                new { UserId = userId, CourseId = courseId },
                cancellationToken: ct));
        return assignments.ToList();
    }

    public async Task<List<Assignment>> GetByCourseIdForStudent(long userId, long courseId, CancellationToken ct)
    {
        const string sqlQuery =
            """
            select a.id as id
                 , a.title as title
                 , a.description as description
                 , a.max_points as max_points
                 , a.due_to as due_to
                 , r.link as link
              from users u
              join students s on s.user_id = u.id
              join student_courses sc on sc.user_id = u.id
              join courses c on c.id = sc.course_id
              join assignments a on a.course_id = c.id
              join repositories r on r.assignment_id = a.id 
                                 and r.student_id = s.id
             where u.id = @UserId
               and c.id = @CourseId;
            """;

        await using var connection = await GetConnection();
        var assignments = await connection.QueryAsync<Assignment>(
            new CommandDefinition(
                sqlQuery,
                new { UserId = userId, CourseId = courseId },
                cancellationToken: ct));
        return assignments.ToList();
    }

    public async Task<List<Assignment>> GetByCourseId(long courseId, CancellationToken ct)
    {
        const string sqlQuery =
            """
            select a.id as id
                 , a.title as title
                 , a.description as description
                 , a.max_points as max_points
                 , a.due_to as due_to
                 , a.spreadsheet_link as link
              from courses c
              join assignments a on a.course_id = c.id
             where c.id = @CourseId;
            """;

        await using var connection = await GetConnection();
        var assignments = await connection.QueryAsync<Assignment>(
            new CommandDefinition(
                sqlQuery,
                new { CourseId = courseId },
                cancellationToken: ct));
        return assignments.ToList();
    }

    public async Task UpdateSpreadSheet(List<Assignment> assignments, CancellationToken ct)
    {
        const string sqlQuery =
            """
            with input_pairs(assignment_id, spreadsheet_id, spreadsheet_link) as (
                select unnest(@AssignmentIds) as assignment_id
                     , unnest(@SpreadsheetIds) as spreadsheet_id
                     , unnest(@SpreadsheetLinks) as spreadsheet_link
            )
            update assignments a
               set spreadsheet_id = ip.spreadsheet_id
                 , spreadsheet_link = ip.spreadsheet_link
              from input_pairs ip
             where a.id = ip.assignment_id;
            """;

        await using var connection = await GetConnection();
        await connection.ExecuteAsync(new CommandDefinition(sqlQuery,
            new
            {
                AssignmentIds = assignments.Select(a => a.Id).ToArray(),
                SpreadsheetIds = assignments.Select(a => a.SpreadSheetId).ToArray(),
                SpreadsheetLinks = assignments.Select(a => a.Link).ToArray()
            }, cancellationToken: ct));
    }
}