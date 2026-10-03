namespace WorkScheduleManagementSystem.Database.Models;

public class AddEmployee
{
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public string? Email { get; set; }
  public string? Phone { get; set; }       
  public decimal Salary { get; set; }
}