using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActivityTrackerAPI.Data;
using ActivityTrackerAPI.Model;
using ActivityTrackerAPI.Repository;

namespace ActivityTrackerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    // GET: api/Employee
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
    {
        List<Employee>? employeeList = await _employeeRepository.GetEmployee();

        return employeeList == null ? NoContent() : employeeList;
    }
    // GET: api/Employee/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployee(int employeeId)
    {
        var employee = await _employeeRepository.GetEmployeeByEmployeeId(employeeId);

        return employee == null ? NotFound() : employee;
    }

    [HttpGet("{teamLeadEmployeeId}")]
    public ActionResult<IEnumerable<Employee>> GetEmployeeByTeamLeadEmployeeId(int teamLeadEmployeeId)
    {
        List<Employee>? employeeList = _employeeRepository.GetEmployeeByTeamLeadEmployeeId(teamLeadEmployeeId);

        if (employeeList == null)
        {
            return NoContent();
        }

        return employeeList;
    }
}
