
using ActivityTrackerAPI.Model;
using ActivityTrackerAPI.Repository;

namespace ActivityTrackerAPI.Validation;

public class TeamValidator : ITeamValidator
{
    private readonly ITeamRepository _teamRepository;
    private readonly IEmployeeRepository _employeeRepository;
    public TeamValidator(ITeamRepository teamRepository, IEmployeeRepository employeeRepository)
    {
        _teamRepository = teamRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<bool> IsEmployeeInTeam(int teamId, int employeeId)
    {
        Employee? employee = await _employeeRepository.GetEmployeeByEmployeeId(employeeId);
        if (employee == null)
        {
            return false;
        }

        if(employee.TeamId != teamId)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> IsTeamIdValid(int teamId)
    {
        Team? team = await _teamRepository.GetTeamByTeamId(teamId);
        if(team == null)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> IsTeamLeadEmployeeIdValid(int teamId, int employeeId)
    {
        Team? team = await _teamRepository.GetTeamByTeamId(teamId);
        if (team == null)
        {
            return false;
        }

        if (team.TeamLeadEmployeeId != employeeId)
        {
            return false;
        }

        return true;
    }
}
