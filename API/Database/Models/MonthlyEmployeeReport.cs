namespace WorkScheduleManagementSystem.Database.Models;

public class MonthlyEmployeeReport
{
  public int EmployeeId { get; set; } 
  public string? FullName { get; set; }
  public int TotalLateAttendanceMinutes { get; set; }
  public int TotalEarlyDepartureMinutes { get; set; }
  public decimal TotalSalaryAfterDeductions { get; set; }
  public int Month { get; set; }        
  public int Year { get; set; }
}
