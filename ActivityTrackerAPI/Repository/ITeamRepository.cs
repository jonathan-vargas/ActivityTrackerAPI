using ActivityTrackerAPI.Data;
using ActivityTrackerAPI.Model;

namespace ActivityTrackerAPI.Repository;

public interface ITeamRepository
{
    Task<Team?> GetTeamByTeamId(int teamId);
    bool IsTeamExists(int teamId);
    Task<Team?> GetTeamByEmployeeId(int employeeId);
}
