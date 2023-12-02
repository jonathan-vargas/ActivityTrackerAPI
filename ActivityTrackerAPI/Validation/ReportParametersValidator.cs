namespace ActivityTrackerAPI.Validation;

public class ReportParametersValidator : IReportParametersValidator
{
    public bool ValidateDateRanges(object initialDate, object finalDate)
    {
        if(initialDate == null || finalDate == null) 
        { 
            return false; 
        }
        return (initialDate is DateTime) && (finalDate is DateTime);
    }

    public bool ValidateEmployeeIdType(object employeeId)
    {
        return employeeId is int;
    }

    public bool ValidateTeamIdType(object teamId)
    {
        return teamId is int;
    }
}
