namespace ActivityTrackerAPI.Validation;

public interface IActivityReportValidator
{
    public bool IsDateRangesValid(DateTime? startedDate, DateTime? finishedDate);
    public Task<bool> IsEmployeeIdParameterValid(int employeeIdParameter);
    public Task<bool> IsTeamIdParameterValid(int teamIdParameter);
    public Task<bool> IsReportActionAllowed(int employeeId, int employeeIdParameter);
}
