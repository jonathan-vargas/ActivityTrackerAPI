using ActivityTrackerAPI.Model;
using ActivityTrackerAPI.Repository;

namespace ActivityTrackerAPI.Validation;

public class EmployeeValidator : IEmployeeValidator
{
    private readonly ITeamRepository _teamRepository;
    private readonly IEmployeeRepository _employeeRepository;
    public EmployeeValidator(ITeamRepository teamRepository, IEmployeeRepository employeeRepository)
    {
        _teamRepository = teamRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<bool> IsEmployeeTeamLead(int employeeId)
    {
        Team? team = await _teamRepository.GetTeamByEmployeeId(employeeId);
        if (team != null)
        {
            return true;
        }
        return false;
    }

    public async Task<bool> IsEmployeeIdValid(int employeeId)
    {
        if(employeeId <= 0 || await _employeeRepository.GetEmployeeByEmployeeId(employeeId) == null)
        {
            return false;
        }

        return true;
    }
}
