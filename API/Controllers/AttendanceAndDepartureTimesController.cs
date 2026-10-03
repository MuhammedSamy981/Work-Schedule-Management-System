using Microsoft.AspNetCore.Mvc;
using WorkScheduleManagementSystem.Database.Models;
using WorkScheduleManagementSystem.Repositories;

namespace WorkScheduleManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendanceAndDepartureTimesController : ControllerBase
{
    private readonly IAttendanceAndDepartureTimeRepository _repository;

    public AttendanceAndDepartureTimesController(IAttendanceAndDepartureTimeRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{employeeId}/{month}/{year}")]
    public async Task<IActionResult> GetAll(int employeeId,int month,int year,
    [FromQuery]int pageNumber=1,[FromQuery]int pageSize=10)
        => Ok(await _repository.GetAllAsync(pageNumber,pageSize,employeeId,month,year));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var AttendanceAndDepartureTime = await _repository.GetByIdAsync(id);
        return AttendanceAndDepartureTime is null ? NotFound() : Ok(AttendanceAndDepartureTime);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddAttendanceTime attendanceTime)
    {
        var rows = await _repository.AddAttendanceTimeAsync(attendanceTime);
        return rows == 0 ? NotFound() : NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> AddDepartureTime(AddDepartureTime departureTime)
    {
        var rows = await _repository.AddDepartureTimeAsync(departureTime);
        return rows == 0 ? NotFound() : NoContent();
    }

}