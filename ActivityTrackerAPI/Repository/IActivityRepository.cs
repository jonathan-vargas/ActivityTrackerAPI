using ActivityTrackerAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace ActivityTrackerAPI.Repository;

public interface IActivityRepository
{
    Task<List<Activity>?> GetActivities();
    Task<List<Activity>?> GetActivitiesByEmployeeId(int employeeId);
    Task<List<Activity>?> GetActivitiesByTeamId(int teamId);
    Task<Activity?> AddActivity(Activity activity);
    Task<bool> UpdateActivity(Activity activity);
    Task<bool> DeleteActivity(int activityId);
    Task<Activity?> GetActivityByActivityId(int activityId);
    bool IsActivityExists(int activityId);
}
