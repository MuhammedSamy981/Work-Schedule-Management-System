namespace WorkScheduleManagementSystem.Database.Models;

public class AddDepartureTime
{
  public int Id { get; set; } 
  public DateTime DepartureTime { get; set; }     
  public int EmployeeId { get; set; }  
}