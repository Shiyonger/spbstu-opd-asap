using Dapper;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;

namespace SPbSTU.OPD.ASAP.Core.Persistence.Repositories;

public class SubmissionsRepository(string connectionString) : PgRepository(connectionString), ISubmissionsRepository
{
    // public async Task<Dictionary<(string, string), Submission>> GetByUsernameAndAssignment(
    //     Dictionary<string, List<string>> usernamesToTitles,
    //     CancellationToken ct)
    // {
    //     const string sqlQuery =
    //         """
    //         select a.title as title
    //              , sb.id as id
    //              , scg.mentor_id as mentor_id
    //              , a.id as assignment_id
    //              , r.link as repository_link
    //              , sb.created_at as created_at
    //              , sb.updated_at as updated_at
    //           from users u 
    //           join students s on s.user_id = u.id
    //           join submissions sb on sb.student_id = s.id
    //           join assignments a on sb.assignment_id = a.id
    //           join repositories r on r.id = sb.repository_id
    //           join groups g on g.id = s.group_id
    //           join subject_course_groups scg on scg.group_id = g.id
    //          where u.github_username = @Username
    //            and a.title = ANY(@AssignmentTitles)
    //         """;
    //
    //     await using var connection = await GetConnection();
    //     var submissionsMap = new Dictionary<(string, string), Submission>();
    //     foreach (var (username, assignmentTitles) in usernamesToTitles)
    //     {
    //         var submissions = await connection.QueryAsync<(string title, Submission submission)>(
    //             new CommandDefinition(
    //                 sqlQuery,
    //                 new { Username = username, AssignmentTitles = assignmentTitles },
    //                 cancellationToken: ct));
    //         
    //         foreach (var submission in submissions)
    //             submissionsMap.TryAdd((username, submission.title), submission.submission);
    //     }
    //
    //     return submissionsMap;
    // }

    public async Task<Dictionary<(string Username, string Title), Domain.Models.Submission>> GetByUsernameAndAssignment(
        List<string> usernames,
        List<string> assignmentTitles,
        CancellationToken ct)
    {
        const string sqlQuery =
            """
            with input_pairs(username, title) as (
                select unnest(@Usernames) as username
                     , unnest(@AssignmentTitles) as title
            )
            select 
                u.github_username as username,
                a.title as title,
                sb.id as id,
                sb.student_id as student_id,
                scg.mentor_id as mentor_id,
                a.id as assignment_id,
                r.link as repository_link,
                sb.created_at as created_at,
                sb.updated_at as updated_at
            from input_pairs ip
            join users u on u.github_username = ip.username
            join students s on s.user_id = u.id
            join submissions sb on sb.student_id = s.id
            join assignments a on sb.assignment_id = a.id and a.title = ip.title
            join repositories r on r.id = sb.repository_id
            join groups g on g.id = s.group_id
            join subject_course_groups scg on scg.group_id = g.id
                                          and scg.course_id=a.course_id;
            """;

        await using var connection = await GetConnection();
        var submissionsRaw = await connection.QueryAsync(
            new CommandDefinition(
                sqlQuery,
                new
                {
                    Usernames = usernames.ToArray(),
                    AssignmentTitles = assignmentTitles.ToArray()
                },
                cancellationToken: ct));

        var submissions = submissionsRaw.Select(row => (
            Username: (string)row.username,
            Title: (string)row.title,
            Submission: new Domain.Models.Submission
            (
                row.id,
                row.student_id,
                row.mentor_id,
                row.assignment_id,
                row.repository_link,
                row.created_at,
                row.updated_at
            )
        ));


        return submissions.ToDictionary(
            s => (s.Username, s.Title),
            s => s.Submission);
    }

    public async Task<List<long>> CreateSubmissions(
        List<string> usernames,
        List<string> assignmentTitles,
        CancellationToken ct)
    {
        const string sqlQuery =
            """
            with input_pairs(username, title) as (
                select unnest(@Usernames) as username
                     , unnest(@AssignmentTitles) as title
            )
            insert into submissions (student_id, assignment_id, repository_id, created_at, updated_at)
            select 
                s.id as student_id,
                a.id as assignment_id,
                r.id as repository_id,
                now() as created_at,
                now() as updated_at
            from input_pairs ip
            join users u on u.github_username = ip.username
            join students s on s.user_id = u.id
            join assignments a on a.title = ip.title
            join repositories r on r.student_id = s.id
                               and r.assignment_id = a.id
            returning id;
            """;

        await using var connection = await GetConnection();
        var ids = await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                new
                {
                    Usernames = usernames.ToArray(),
                    AssignmentTitles = assignmentTitles.ToArray()
                },
                cancellationToken: ct));
        return ids.ToList();
    }

    public async Task UpdateSubmissions(List<long> submissionIds, CancellationToken ct)
    {
        const string sqlQuery =
            """
            update submissions
               set updated_at = now()
             where submissions.id = ANY(@SubmissionIds);
            """;

        await using var connection = await GetConnection();
        await connection.ExecuteAsync(new CommandDefinition(
            sqlQuery,
            new { SubmissionIds = submissionIds },
            cancellationToken: ct));
    }
}