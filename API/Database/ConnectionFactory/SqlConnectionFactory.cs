using System.Data;
using Microsoft.Data.SqlClient;

namespace WorkScheduleManagementSystem.Database.ConnectionFactory;

public class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default")!;
    }

    public IDbConnection CreateConnection()
        => new SqlConnection(_connectionString);
}