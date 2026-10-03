using System.Data;

namespace WorkScheduleManagementSystem.Database.ConnectionFactory;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
