namespace ActivityTrackerAPI.Validation;

public interface IEmployeeValidator
{
    Task<bool> IsEmployeeIdValid(int employeeId);
    Task<bool> IsEmployeeTeamLead(int employeeId);
}
