using Dapper;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;
using SPbSTU.OPD.ASAP.Core.Domain.ValueObjects;

namespace SPbSTU.OPD.ASAP.Core.Persistence.Repositories;

public class GoogleRepository(string connectionString) : PgRepository(connectionString), IGoogleRepository
{
    public async Task<Dictionary<(string, long), Position>> GetStudentsPositions(
        List<(string, long)> githubUsernamesAndCourses,
        CancellationToken ct)
    {
        const string sqlQuery =
            """
            with input_pairs(username, course_id) as (
                select unnest(@Usernames) as username
                     , unnest(@Courses) as course_id
            )
            select u.github_username as username
                 , ip.course_id as course_id
                 , gp.cell as cell
                 , gp.spreadsheet_id as spreadsheet_id
              from input_pairs ip
              join users u on u.github_username = ip.username
              join students s on s.user_id = u.id
              join google g on g.student_id = s.id
                           and g.course_id = ip.course_id
              join google_positions gp on gp.id = g.student_position_id
             group by u.github_username, ip.course_id, gp.cell, gp.spreadsheet_id;
            """;

        await using var connection = await GetConnection();
        var positions =
            (await connection.QueryAsync<(string username, long courseId, string cell, string spreadsheetId)>(
                new CommandDefinition(
                    sqlQuery,
                    new
                    {
                        Usernames = githubUsernamesAndCourses.Select(g => g.Item1).ToArray(),
                        Courses = githubUsernamesAndCourses.Select(g => g.Item2).ToArray()
                    },
                    cancellationToken: ct)))
            .Select(p => (p.username, p.courseId, new Position(p.cell, p.spreadsheetId)));

        return positions.ToDictionary(p => (p.username, p.courseId), p => p.Item3);
    }

    public async Task<Dictionary<(string, long), Position>> GetAssignmentsPositions(
        List<(string, long)> titlesAndCourses, CancellationToken ct)
    {
        const string sqlQuery =
            """
            with input_pairs(title, course_id) as (
                select unnest(@Titles) as title
                     , unnest(@Courses) as course_id
            )
            select ip.title as title
                 , ip.course_id as course_id
                 , gp.cell as cell
                 , gp.spreadsheet_id as spreadsheet_id
              from input_pairs ip
              join assignments a on a.title = ip.title
              join google g on g.assignment_id = a.id
                           and g.course_id = ip.course_id
              join google_positions gp on gp.id = g.assignment_position_id
             group by ip.title, ip.course_id, gp.cell, gp.spreadsheet_id;
            """;

        await using var connection = await GetConnection();
        var positions = (await connection.QueryAsync<(string title, long courseId, string cell, string spreadsheetId)>(
            new CommandDefinition(
                sqlQuery,
                new
                {
                    Titles = titlesAndCourses.Select(t => t.Item1).ToArray(),
                    Courses = titlesAndCourses.Select(t => t.Item2).ToArray()
                },
                cancellationToken: ct))).Select(p => (p.title, p.courseId, new Position(p.cell, p.spreadsheetId)));

        return positions.ToDictionary(p => (p.title, p.courseId), p => p.Item3);
    }
}