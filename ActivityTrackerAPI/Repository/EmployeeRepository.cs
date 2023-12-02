using ActivityTrackerAPI.Data;
using ActivityTrackerAPI.Model;
using ActivityTrackerAPI.Utility;
using Microsoft.EntityFrameworkCore;

namespace ActivityTrackerAPI.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly ILogger<ActivityRepository> _logger;

    public EmployeeRepository(AppDbContext context, ILogger<ActivityRepository> logger)
    {
        this._appDbContext = context;
        _logger = logger;
    }
    public async Task<List<Employee>?> GetEmployee()
    {
        if (_appDbContext?.Employee == null)
            return null;

        return await _appDbContext.Employee.ToListAsync();
    }
    public async Task<Employee?> GetEmployeeByEmployeeId(int employeeId)
    {
        if (_appDbContext?.Employee == null)
        {
            return null;
        }
        var employee = await _appDbContext.Employee.FindAsync(employeeId);

        return (employee != null)? employee: null;
    }
    public List<Employee>? GetEmployeeByTeamLeadEmployeeId(int teamLeadEmployeeId)
    {
        var employeesByTeamLeadEmployeeId = from employee in _appDbContext.Employee
                                            join team in _appDbContext.Team on employee.TeamId equals team.TeamId
                                            where team.TeamLeadEmployeeId == teamLeadEmployeeId
                                            select new Employee
                                            {
                                                EmployeeId = employee.EmployeeId,
                                                Name = employee.Name,
                                                PaternalLastName = employee.PaternalLastName,
                                                MaternalLastName = employee.MaternalLastName,
                                                Email = employee.Email,
                                                Status = employee.Status,
                                                TeamId = employee.TeamId
                                            };

        if (employeesByTeamLeadEmployeeId == null)
        {
            _logger.LogError(ErrorMessages.ERROR_WHILE_EXECUTING_LINQ_QUERY);
            return null;
        }

        return employeesByTeamLeadEmployeeId.ToList<Employee>();
    }
}
