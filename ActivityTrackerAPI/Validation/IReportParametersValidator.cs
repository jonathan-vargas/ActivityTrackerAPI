namespace ActivityTrackerAPI.Validation;

public interface IReportParametersValidator
{
    bool ValidateDateRanges(object initialDate, object finalDate);
    bool ValidateEmployeeIdType(object employeeId);
    bool ValidateTeamIdType(object teamId);
}
