using ActivityTrackerAPI.Model;

namespace ActivityTrackerAPI.Repository;

public interface IActivityReportRepository
{
    List<ActivityReport>? GetActivityReport(Dictionary<string, DateTime> dateParameters, int employeeIdParameter, int teamIdParameter);
}
