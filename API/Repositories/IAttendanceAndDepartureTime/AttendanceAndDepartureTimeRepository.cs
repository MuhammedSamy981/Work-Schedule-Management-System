using System.Data;
using Dapper;
using WorkScheduleManagementSystem.Database.ConnectionFactory;
using WorkScheduleManagementSystem.Database.Models;

namespace WorkScheduleManagementSystem.Repositories;
public class AttendanceAndDepartureTimeRepository : IAttendanceAndDepartureTimeRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public AttendanceAndDepartureTimeRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<AttendanceAndDepartureTime>> GetAllAsync(int pageNumber,int pageSize,int employeeId, int month, int year)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = @"SELECT AD.Id,AD.EmployeeId, E.FirstName + ' ' + E.LastName AS FullName,
                             AD.AttendanceTime ,AD.DepartureTime
                             FROM AttendanceAndDepartureTimes AD
                             INNER JOIN Employees E ON E.Id = AD.EmployeeId
                             WHERE AD.EmployeeId=@EmployeeId 
                             AND YEAR(AD.AttendanceTime)=@Year 
                             AND MONTH(AD.AttendanceTime)=@Month
                             ORDER BY AD.Id
                             OFFSET (@PageNumber-1)*@PageSize ROWS
                             FETCH NEXT @PageSize ROWS ONLY";
        return await connection.QueryAsync<AttendanceAndDepartureTime>(sql, new 
            {
             PageNumber= pageNumber,
             PageSize= pageSize,
             EmployeeId = employeeId,
             Month=month,
             Year=year
            });
    }

    public async Task<AttendanceAndDepartureTime?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = @"SELECT AD.Id,AD.EmployeeId, E.FirstName + ' ' + E.LastName AS FullName,
                             AD.AttendanceTime ,AD.DepartureTime
                             FROM AttendanceAndDepartureTimes AD
                             INNER JOIN Employees E ON E.Id = AD.EmployeeId
                             WHERE AD.Id = @Id";
        return await connection.QuerySingleOrDefaultAsync<AttendanceAndDepartureTime>(sql, new { Id = id });
    }


    public  async Task<int> AddAttendanceTimeAsync(AddAttendanceTime attendanceTime)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteAsync("AddAttendanceTime", attendanceTime,commandType:CommandType.StoredProcedure);
    }

    public async Task<int> AddDepartureTimeAsync(AddDepartureTime departureTime)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteAsync("AddDepartureTime", departureTime,commandType:CommandType.StoredProcedure);
    }
}