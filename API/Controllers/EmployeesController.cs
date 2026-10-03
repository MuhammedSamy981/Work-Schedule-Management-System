using Microsoft.AspNetCore.Mvc;
using WorkScheduleManagementSystem.Database.Models;
using WorkScheduleManagementSystem.Repositories;

namespace WorkScheduleManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _repository;

    public EmployeesController(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery]int pageNumber=1,[FromQuery]int pageSize=10)
        => Ok(await _repository.GetAllAsync(pageNumber,pageSize));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await _repository.GetByIdAsync(id);
        return employee is null ? NotFound() : Ok(employee);
    }

    [HttpGet("report")]
    public async Task<IActionResult> GetAllMonthlyEmployeeReports([FromQuery]int pageNumber=1,[FromQuery]int pageSize=10)
        => Ok(await _repository.GetAllMonthlyEmployeeReportsAsync(pageNumber,pageSize));

    [HttpGet("report/{employeeId}/{month}/{year}")]
    public async Task<IActionResult> GetMonthlyEmployeeReport(int employeeId,int month,int year)
    {
        var employeeReport = await _repository.GetMonthlyEmployeeReport(employeeId,month,year);
        return employeeReport is null ? NotFound() : Ok(employeeReport);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddEmployee employee)
    {
        var rows = await _repository.AddAsync(employee);
        return StatusCode(rows == 0 ? StatusCodes.Status400BadRequest:StatusCodes.Status201Created );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateEmployee employee)
    {
        if (id != employee.Id) return BadRequest();
        var rows = await _repository.UpdateAsync(employee);
        return rows == 0 ? NotFound() : NoContent();
    }

}