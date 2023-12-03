using ActivityTrackerAPI.Model;

namespace ActivityTrackerAPI.Validation;

public interface IActivityValidator
{
    bool IsActivityInsertValid(Activity activity, int employeeId);
    bool IsActivityUpdateValid(Activity activity, int employeeId);
    bool IsActivityDeleteValid(int activityId);
    bool IsInsertOrUpdateAllowed(Activity activity, int employeeId);
    Task<bool> IsDeleteAllowed(int activityId, int employeeId);
    Task<bool> IsGetActionAllowed(int employeeId);
}
