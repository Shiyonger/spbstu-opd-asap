using Dapper;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;
using SPbSTU.OPD.ASAP.Core.Domain.ValueObjects;

namespace SPbSTU.OPD.ASAP.Core.Persistence.Repositories;

public class GoogleRepository(string connectionString) : PgRepository(connectionString), IGoogleRepository
{
    public async Task<Dictionary<string, Position>> GetStudentsPositions(List<string> githubUsernames, CancellationToken ct)
    {
        const string sqlQuery =
            """
            select u.github_username
                 , gp.cell as cell
                 , gp.spreadsheet_id as spreadsheet_id
              from users u
              join students s on s.user_id = u.id
              join google g on g.student_id = s.id
              join google_positions gp on gp.id = g.student_position_id
             where u.github_username = ANY(@Usernames);
            """;
        
        await using var connection = await GetConnection();
        var positions = await connection.QueryAsync<(string username, Position position)>(
            new CommandDefinition(
                sqlQuery,
                new { Usernames = githubUsernames },
                cancellationToken: ct));
        
        return positions.ToDictionary(p => p.username, p => p.position);
    }

    public async Task<Dictionary<string, Position>> GetAssignmentsPositions(List<string> titles, CancellationToken ct)
    {
        const string sqlQuery =
            """
            select gp.cell as cell
                 , gp.spreadsheet_id as spreadsheet_id
              from assignments a 
              join google g on g.assignment_id = a.id
              join google_positions gp on gp.id = g.assignment_position_id
             where a.title = ANY(@Titles);
            """;
        
        await using var connection = await GetConnection();
        var positions = await connection.QueryAsync<(string title, Position position)>(
            new CommandDefinition(
                sqlQuery,
                new { Titles = titles },
                cancellationToken: ct));
        
        return positions.ToDictionary(p => p.title, p => p.position);
    }
}