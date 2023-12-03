using ActivityTrackerAPI.Model;
using ActivityTrackerAPI.Repository;

namespace ActivityTrackerAPI.Validation;

public class ActivityValidator : IActivityValidator
{
    private readonly IActivityRepository _activityRepository;
    private readonly IEmployeeValidator _employeeValidator;
    private readonly IEmployeeRepository _employeeRepository;
    public ActivityValidator(IActivityRepository activityRepository, IEmployeeValidator employeeValidator, IEmployeeRepository employeeRepository)
    {
        _activityRepository = activityRepository;
        _employeeValidator = employeeValidator;
        _employeeRepository = employeeRepository;
    }
    public bool IsActivityDeleteValid(int activityId)
    {
        if (activityId <= 0)
            return false;

        bool isActivityExist = _activityRepository.IsActivityExists(activityId);
        return isActivityExist;
    }

    public bool IsActivityInsertValid(Activity activity, int employeeId)
    {
        if (!IsActivityPropertiesValid(activity, isIncludeIdCheck:false) || activity.ActivityId != 0)
        {
            return false;
        }

        return true;
    }

    public bool IsActivityUpdateValid(Activity activity, int employeeId)
    {
        if (!IsActivityPropertiesValid(activity, isIncludeIdCheck: true) || activity.EmployeeId != employeeId)
        {
            return false;
        }
       
        return true;
    }

    public async Task<bool> IsGetActionAllowed(int employeeId)
    {
        Employee? employee = await _employeeRepository.GetEmployeeByEmployeeId(employeeId);
        if(employee == null)
        {
            return false;
        }

        return await _employeeValidator.IsEmployeeTeamLead(employeeId);
    }

    public bool IsInsertOrUpdateAllowed(Activity activity, int employeeId)
    {
        return activity.EmployeeId == employeeId;
    }
    public async Task<bool> IsDeleteAllowed(int activityId, int employeeId)
    {
        Activity? activity = await _activityRepository.GetActivityByActivityId(activityId);
        if (activity == null || activity.EmployeeId != employeeId)
        {
            return false;
        }

        return true;
    }

    private bool IsActivityPropertiesValid(Activity activity, bool isIncludeIdCheck)
    {
        if (isIncludeIdCheck && (activity.ActivityId <= 0 || !_activityRepository.IsActivityExists(activity.ActivityId)))
        {
            return false;
        }

        if(activity == null)
        {
            return false;
        }

        if (activity.StartedDate == DateTime.MinValue || activity.FinishedDate == DateTime.MinValue ||
            activity.StartedDate == DateTime.MaxValue || activity.FinishedDate == DateTime.MaxValue)
        {
            return false;
        }

        if (activity.StartedDate > activity.FinishedDate)
        {
            return false;
        }

        if (activity.EmployeeId <= 0 || !_employeeRepository.IsEmployeeExists(activity.EmployeeId))
        {
            return false;
        }

        if (activity.Description == "" || activity.Description == null)
        {
            return false;
        }
        DateTime startedDateUTC = activity.StartedDate.ToUniversalTime();
        DateTime finishedDateUTC = activity.FinishedDate.ToUniversalTime();
        
        int startedDateYear = startedDateUTC.Year;
        int startedDateMonth = startedDateUTC.Month;
        int startedDateDay = startedDateUTC.Day;
        
        int finishedDateYear = finishedDateUTC.Year;
        int finishedDateMonth = finishedDateUTC.Month;
        int finishedDateDay = finishedDateUTC.Day;

        if (startedDateYear < DateTime.UtcNow.Year ||
            finishedDateYear < DateTime.UtcNow.Year ||
            startedDateMonth < DateTime.UtcNow.Month ||
            finishedDateMonth < DateTime.UtcNow.Month ||
            startedDateDay < DateTime.UtcNow.Day ||
            finishedDateDay < DateTime.UtcNow.Day)
        {
            return false;
        }

        return true;
    }
}
