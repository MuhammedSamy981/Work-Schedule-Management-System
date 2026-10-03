using WorkScheduleManagementSystem.Database.Models;

namespace WorkScheduleManagementSystem.Repositories;

public interface IAttendanceAndDepartureTimeRepository
{
    Task<IEnumerable<AttendanceAndDepartureTime>> GetAllAsync(int pageNumber,int pageSize,int employeeId,int month,int year);
    Task<AttendanceAndDepartureTime?> GetByIdAsync(int id);
    Task<int> AddAttendanceTimeAsync(AddAttendanceTime attendanceTime);
    Task<int> AddDepartureTimeAsync(AddDepartureTime departureTime);
}