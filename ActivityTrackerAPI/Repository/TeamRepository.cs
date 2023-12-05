using ActivityTrackerAPI.Data;
using ActivityTrackerAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace ActivityTrackerAPI.Repository;

public class TeamRepository : ITeamRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly IEmployeeRepository _employeeRepository;
    public TeamRepository(AppDbContext appDbContext, IEmployeeRepository employeeRepository)
    {
        _appDbContext = appDbContext;
        _employeeRepository = employeeRepository;
    }
    public async Task<Team?> GetTeamByTeamId(int teamId)
    {
        if (_appDbContext?.Team == null)
        {
            return null;
        }
        Team? team = await _appDbContext.Team.FindAsync(teamId);

        return team ?? null;
    }

    public async Task<Team?> GetTeamByEmployeeId(int employeeId)
    {
        Employee? employee = await _employeeRepository.GetEmployeeByEmployeeId(employeeId);

        if (_appDbContext?.Team == null)
        {
            return null;
        }

        if(employee == null)
        {
            return null;
        }

        Team? team = await GetTeamByTeamId(employee.TeamId);

        return team;
    }

    public bool IsTeamExists(int teamId)
    {
        return (_appDbContext.Team?.Any(e => e.TeamId == teamId)).GetValueOrDefault();
    }
}
