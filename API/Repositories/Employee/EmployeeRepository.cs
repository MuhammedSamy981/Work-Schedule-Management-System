using System.Data;
using Dapper;
using WorkScheduleManagementSystem.Database.ConnectionFactory;
using WorkScheduleManagementSystem.Database.Models;

namespace WorkScheduleManagementSystem.Repositories;
public class EmployeeRepository : IEmployeeRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public EmployeeRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync(int pageNumber,int pageSize)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = @"SELECT * FROM Employees      
                             ORDER BY Id
                             OFFSET (@PageNumber-1)*@PageSize ROWS
                             FETCH NEXT @PageSize ROWS ONLY";
        return await connection.QueryAsync<Employee>(sql,new 
            {
             PageNumber= pageNumber,
             PageSize= pageSize
            });
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Employees WHERE Id = @Id";
        return await connection.QuerySingleOrDefaultAsync<Employee>(sql, new { Id = id });
    }
    public async Task<IEnumerable<MonthlyEmployeeReport>> GetAllMonthlyEmployeeReportsAsync(int pageNumber,int pageSize)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = @"SELECT * FROM MonthlyEmployeeReports      
                             ORDER BY Year,Month
                             OFFSET (@PageNumber-1)*@PageSize ROWS
                             FETCH NEXT @PageSize ROWS ONLY";
        return await connection.QueryAsync<MonthlyEmployeeReport>(sql,new 
            {
             PageNumber= pageNumber,
             PageSize= pageSize
            });
    }

        public async Task<MonthlyEmployeeReport?> GetMonthlyEmployeeReport(int employeeId,int month,int year)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = @"SELECT * FROM MonthlyEmployeeReports 
                             WHERE EmployeeId=@EmployeeId 
                             AND Month=@Month 
                             AND Year=@Year";
        return await connection.QuerySingleOrDefaultAsync<MonthlyEmployeeReport>(sql,new 
            {
             EmployeeId = employeeId,
             Month=month,
             Year=year
            });
    }
    public async Task<int> AddAsync(AddEmployee Employee)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteAsync("AddEmployee", Employee,commandType:CommandType.StoredProcedure);
    }

    public async Task<int> UpdateAsync(UpdateEmployee Employee)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteAsync("UpdateEmployee", Employee,commandType:CommandType.StoredProcedure);
    }

}