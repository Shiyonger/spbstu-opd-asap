using Dapper;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;
using SPbSTU.OPD.ASAP.Core.Domain.Models;

namespace SPbSTU.OPD.ASAP.Core.Persistence.Repositories;

public class GithubRepository(string connectionString) : PgRepository(connectionString), IGithubRepository
{
    public async Task<List<string>> GetUsernamesToInvite(string githubOrganization, CancellationToken ct)
    {
        const string sqlQuery =
            """
            select distinct u.github_username
              from courses c 
              join student_courses sc on sc.course_id = c.id
              join students s on s.id = sc.student_id
              join users u on u.id = s.user_id
             where c.github_organization = @Organization
               and sc.is_invited = false;
            """;

        await using var connection = await GetConnection();
        var usernames = await connection.QueryAsync<string>(
            new CommandDefinition(
                sqlQuery,
                new { Organization = githubOrganization },
                cancellationToken: ct));
        return usernames.ToList();
    }

    public async Task<List<string>> GetUsernamesToCreateRepository(string githubOrganization, string assignmentTitle,
        CancellationToken ct)
    {
        const string sqlQuery =
            """
            select u.github_username
              from assignments a 
              join courses c on c.id = a.course_id
              join student_courses sc on sc.course_id = c.id
              join students s on s.id = sc.student_id
              join users u on u.id = s.user_id
             where a.title = @AssignmentTitle
               and c.github_organization = @Organization
               and (s.id, a.id) not in 
                   (select r.student_id
                         , r.assignment_id 
                      from repositories r);
            """;

        await using var connection = await GetConnection();
        var usernames = await connection.QueryAsync<string>(
            new CommandDefinition(
                sqlQuery,
                new { AssignmentTitle = assignmentTitle, Organization = githubOrganization },
                cancellationToken: ct));
        return usernames.ToList();
        ;
    }

    public async Task<List<long>> CreateRepositories(List<Repository> repositories, CancellationToken ct)
    {
        const string sqlQuery =
            """
            with input_pairs(username, organization_name, title, link) as (
                select * from unnest(@Usernames, @Organizations, @AssignmentTitles, @Links)
            )
            insert into repositories (student_id, assignment_id, link)
            select s.id, a.id, ip.link
              from input_pairs ip
              join users u on u.github_username = ip.username
              join students s on s.user_id = u.id
              join student_courses sc on sc.student_id = s.id
              join courses c on c.id = sc.course_id
                            and c.github_organization = ip.organization_name
              join assignments a on a.course_id = c.id
                                and a.title = ip.title
            returning id;
            """;

        await using var connection = await GetConnection();
        var ids = await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                new
                {
                    Usernames = repositories.Select(r => r.GithubUsername).ToList(),
                    Organizations = repositories.Select(r => r.GithubOrganizationName).ToList(),
                    AssignmentTitles = repositories.Select(r => r.AssignmentTitle).ToList(),
                    Links = repositories.Select(r => r.RepositoryLink).ToList()
                },
                cancellationToken: ct));
        return ids.ToList();
    }

    public async Task MarkInvited(List<string> usernames, CancellationToken ct)
    {
        const string sqlQuery =
            """
            update student_courses sc
               set is_invited = true
             where sc.student_id in 
                   (select s.id
                      from students s 
                      join users u on u.id = s.user_id 
                     where u.github_username = ANY(@Usernames));
            """;

        await using var connection = await GetConnection();
        await connection.ExecuteAsync(
            new CommandDefinition(
                sqlQuery,
                new { Usernames = usernames },
                cancellationToken: ct));
    }
}