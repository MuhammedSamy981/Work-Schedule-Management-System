namespace WorkScheduleManagementSystem.Database.Models;

public class AttendanceAndDepartureTime
{
  public int Id { get; set; } 
  public string? FullName { get; set; }
  public DateTime AttendanceTime { get; set; } 
  public DateTime DepartureTime { get; set; } 
  public int EmployeeId { get; set; }        
}