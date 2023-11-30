using ActivityTrackerAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace ActivityTrackerAPI.Repository;

public interface IActivityRepository
{
    Task<List<Activity>> GetActivitiesByEmployeeId(int employeeId);
    Task<List<Activity>> GetActivitiesByTeamId(int teamId);
    Task<Activity> AddActivity(Activity activity);
    Task<Activity?> UpdateActivity(Activity activity);
    Task<Activity?> DeleteActivity(int activityId);
}
