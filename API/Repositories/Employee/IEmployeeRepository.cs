using WorkScheduleManagementSystem.Database.Models;

namespace WorkScheduleManagementSystem.Repositories;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync(int pageNumber,int pageSize);
    Task<Employee?> GetByIdAsync(int id);
    Task<IEnumerable<MonthlyEmployeeReport>> GetAllMonthlyEmployeeReportsAsync(int pageNumber,int pageSize);
    Task<MonthlyEmployeeReport?> GetMonthlyEmployeeReport(int employeeId,int month,int year);
    Task<int> AddAsync(AddEmployee Employee);
    Task<int> UpdateAsync(UpdateEmployee Employee);
}