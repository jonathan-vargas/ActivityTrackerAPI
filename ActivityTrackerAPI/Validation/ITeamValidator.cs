namespace ActivityTrackerAPI.Validation;

public interface ITeamValidator
{
    Task<bool> IsTeamIdValid(int teamId);
    Task<bool> IsTeamLeadEmployeeIdValid(int teamId, int employeeId);
    Task<bool> IsEmployeeInTeam(int teamId, int employeeId);

}
