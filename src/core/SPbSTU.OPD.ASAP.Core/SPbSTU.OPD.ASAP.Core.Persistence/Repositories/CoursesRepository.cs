using Dapper;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;
using SPbSTU.OPD.ASAP.Core.Domain.Models;

namespace SPbSTU.OPD.ASAP.Core.Persistence.Repositories;

public class CoursesRepository(string connectionString) : PgRepository(connectionString), ICoursesRepository
{
    public async Task<List<Course>> GetByUserId(long userId, CancellationToken ct)
    {
        const string sqlQuery =
            """
            select c.id as id
                 , c.title as title
                 , s.title as subject_title
                 , c.github_organization as github_organization_link
                 , c.google_spreadsheet as google_spreadsheet_link
              from users u
              join student_courses sc on sc.user_id = u.id
              join courses c on c.id = sc.course_id
              join subjects s on s.id = c.subject_id
             where u.id = @UserId;
            """;

        await using var connection = await GetConnection();
        var courses = await connection.QueryAsync<Course>(
            new CommandDefinition(
                sqlQuery,
                new { UserId = userId },
                cancellationToken: ct));
        return courses.ToList();
    }

    public async Task<List<Course>> GetCoursesByTitles(List<string> titles, CancellationToken ct)
    {
        const string sqlQuery =
            """
            select c.id as id
                 , c.title as title
                 , s.title as subject_title
                 , c.github_organization as github_organization_link
                 , c.google_spreadsheet as google_spreadsheet_link
              from courses c
              join subjects s on s.id = c.subject_id
             where c.title = ANY(@Titles);
            """;

        await using var connection = await GetConnection();
        var courses = await connection.QueryAsync<Course>(
            new CommandDefinition(
                sqlQuery,
                new { Titles = titles },
                cancellationToken: ct));
        return courses.ToList();
    }

    public async Task<List<Course>> GetForCreateSpreadSheet(CancellationToken ct)
    {
        const string sqlQuery =
            """
            select c.id as id
                 , c.title as title
                 , s.title as subject_title
                 , c.github_organization as github_organization_link
              from courses c
              join subjects s on s.id = c.subject_id
             where c.id not in (select distinct course_id 
                                  from google);
            """;

        await using var connection = await GetConnection();
        var courses = await connection.QueryAsync<Course>(
            new CommandDefinition(
                sqlQuery,
                cancellationToken: ct));
        return courses.ToList();
    }

    public async Task UpdateSpreadSheet(List<Course> courses, CancellationToken ct)
    {
        const string sqlQuery =
            """
            with input_pairs(course_id, spreadsheet) as (
                select unnest(@CourseIds) as course_id
                     , unnest(@Spreadsheets) as spreadsheet
            )
            update courses c
               set google_spreadsheet = ip.spreadsheet
              from input_pairs ip
             where c.id = ip.course_id;
            """;

        await using var connection = await GetConnection();
        await connection.ExecuteAsync(new CommandDefinition(sqlQuery,
            new
            {
                CourseIds = courses.Select(c => c.Id).ToArray(),
                Spreadsheets = courses.Select(c => c.GoogleSpreadSheetLink).ToArray()
            }, cancellationToken: ct));
    }
}