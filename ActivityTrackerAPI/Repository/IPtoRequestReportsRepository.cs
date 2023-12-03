using ActivityTrackerAPI.Model;

namespace ActivityTrackerAPI.Repository;

public interface IPtoRequestReportsRepository
{
    List<PtoRequestReport>? GetActivityReport(Dictionary<string, DateTime> dateParameters, int employeeIdParameter, int teamIdParameter);
}
