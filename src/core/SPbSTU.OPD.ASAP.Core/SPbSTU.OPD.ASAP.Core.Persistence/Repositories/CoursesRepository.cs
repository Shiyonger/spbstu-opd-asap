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
              from students st
              join student_courses sc on st.user_id = sc.student_id
              join courses c on c.id = sc.course_id
              join subjects s on s.id = c.subject_id
             where st.user_id = @UserId;
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
}