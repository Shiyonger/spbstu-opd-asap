using Dapper;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;
using SPbSTU.OPD.ASAP.Core.Domain.Models.User;

namespace SPbSTU.OPD.ASAP.Core.Persistence.Repositories;

public class UsersRepository(string connectionString) : PgRepository(connectionString), IUsersRepository
{
    public async Task<long> Create(User user, CancellationToken ct)
    {
        const string sqlQuery =
            """
            insert into users (name, login, password, email, role, github_link)
            values (@Name, @Login, @Password, @Email, @Role, @GithubLink)
            returning id;
            """;

        await using var connection = await GetConnection();
        var id = await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                new
                {
                    Name = user.Name,
                    Login = user.Login,
                    Password = user.Password,
                    Email = user.Email,
                    Role = Enum.GetName(user.Role),
                    GithubLink = user.GithubLink
                },
                cancellationToken: ct));
        
        return id.FirstOrDefault();
    }

    public async Task<User?> GetById(long userId, CancellationToken ct)
    {
        const string sqlQuery =
            """
            select id
                 , name
                 , login
                 , password
                 , email
                 , role
                 , github_link
              from users
             where id = @Id;
            """;
        
        
        await using var connection = await GetConnection();
        IEnumerable<User?> user = await connection.QueryAsync<User>(
            new CommandDefinition(
                sqlQuery,
                new
                {
                    Id = userId
                },
                cancellationToken: ct));
        
        return user.FirstOrDefault();
    }

    public async Task<User?> GetByLogin(string login, CancellationToken ct)
    {
        const string sqlQuery =
            """
            select id
                 , name
                 , login
                 , password
                 , email
                 , role
                 , github_link
              from users
             where login = @Login;
            """;
        
        
        await using var connection = await GetConnection();
        IEnumerable<User?> user = await connection.QueryAsync<User>(
            new CommandDefinition(
                sqlQuery,
                new
                {
                    Login = login
                },
                cancellationToken: ct));
        
        return user.FirstOrDefault();
    }
}