using System.Transactions;
using Npgsql;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts;

namespace SPbSTU.OPD.ASAP.Core.Persistence.Repositories;

public class PgRepository : IPgRepository
{
    private readonly string _connectionString;

    protected const int DefaultTimeoutInSeconds = 5;

    protected PgRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    protected async Task<NpgsqlConnection> GetConnection()
    {
        if (Transaction.Current is not null &&
            Transaction.Current.TransactionInformation.Status is TransactionStatus.Aborted)
        {
            throw new TransactionAbortedException("Transaction was aborted (probably by user cancellation request)");
        }
        
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        
        // Due to in-process migrations
        connection.ReloadTypes();
        
        return connection;
    }
}