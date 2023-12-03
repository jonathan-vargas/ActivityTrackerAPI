using ActivityTrackerAPI.Data;
using ActivityTrackerAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace ActivityTrackerAPI.Repository;

public class TeamRepository : ITeamRepository
{
    private readonly AppDbContext _appDbContext;
    
    public TeamRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
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
        if (_appDbContext?.Team == null)
        {
            return null;
        }
        Team? team = await _appDbContext.Team.FirstOrDefaultAsync(team => team.TeamLeadEmployeeId == employeeId);

        return team;
    }

    public bool IsTeamExists(int teamId)
    {
        return (_appDbContext.Team?.Any(e => e.TeamId == teamId)).GetValueOrDefault();
    }
}
