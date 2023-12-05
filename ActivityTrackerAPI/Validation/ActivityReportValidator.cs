using ActivityTrackerAPI.Model;
using ActivityTrackerAPI.Repository;

namespace ActivityTrackerAPI.Validation;

public class ActivityReportValidator : IActivityReportValidator
{
    private readonly IEmployeeValidator _employeeValidator;
    private readonly ITeamValidator _teamValidator;
    public ActivityReportValidator(IEmployeeValidator employeeValidator, ITeamValidator teamValidator)
    {
        _employeeValidator = employeeValidator;
        _teamValidator = teamValidator;
    }
    public async Task<bool> IsActivityReportAllowed(int employeeId, int employeeIdParameter, int teamIdParameter)
    {
        bool isEmployeeTeamLead = await _employeeValidator.IsEmployeeTeamLead(employeeId);

        if (!await _employeeValidator.IsEmployeeIdValid(employeeId))
        {
            return false;
        }

        if (employeeIdParameter > 0 && !await _employeeValidator.IsEmployeeIdValid(employeeIdParameter))
        {
            return false;
        }

        if (employeeIdParameter > 0 && employeeId != employeeIdParameter && !isEmployeeTeamLead)
        {
            return false;
        }

        if(employeeIdParameter == 0 && !isEmployeeTeamLead)
        {
            return false;
        }

        if (employeeId == employeeIdParameter && isEmployeeTeamLead)
        {
            return false;
        }


        return true;
    }

    public bool IsDateRangesValid(DateTime? startedDate, DateTime? finishedDate)
    {
        if(startedDate == null && finishedDate != null || 
            startedDate != null && finishedDate == null)
        {
            return false;
        }

        if (startedDate != null && (startedDate == DateTime.MinValue || finishedDate == DateTime.MinValue ||
                                    startedDate == DateTime.MaxValue || finishedDate == DateTime.MaxValue))
        {
            return false;
        }

        if (startedDate > finishedDate)
        { 
            return false; 
        }

        return true;
    }

    public async Task<bool> IsEmployeeIdParameterValid(int employeeIdParameter)
    {
        if (employeeIdParameter > 0 && !await _employeeValidator.IsEmployeeIdValid(employeeIdParameter))
        {
            return false;
        }

        return true;
    }

    public async Task<bool> IsTeamIdParameterValid(int teamIdParameter)
    {

        if (teamIdParameter > 0 && !await _teamValidator.IsTeamIdValid(teamIdParameter))
        {
            return false;
        }

        return true;
    }

    public async Task<bool> IsReportActionAllowed(int employeeId, int employeeIdParameter)
    {
        if (employeeIdParameter > 0 && await _employeeValidator.IsEmployeeTeamLead(employeeId) && employeeId == employeeIdParameter)
        {
            return false;
        }
        
        if (employeeIdParameter > 0 && !await _employeeValidator.IsEmployeeTeamLead(employeeId) && employeeId != employeeIdParameter)
        {
            return false;
        }

        if (employeeIdParameter <= 0 && !await _employeeValidator.IsEmployeeTeamLead(employeeId))
        {
            return false;
        }
            

        //if(&& !await _teamValidator.IsEmployeeInTeam(teamIdParameter, employeeId))
        //{
        //    return false;
        //}

        //if (teamIdParameter > 0 && await _employeeValidator.IsEmployeeTeamLead(employeeId) && !await _teamValidator.IsTeamLeadEmployeeIdValid(teamIdParameter, employeeId))
        //{
        //    return false;
        //}

        return true;
    }
}
