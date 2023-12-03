using ActivityTrackerAPI.Model;

namespace ActivityTrackerAPI.Repository;

public interface IEmployeeRepository
{
    Task<List<Employee>?> GetEmployee(); 
    List<Employee>? GetEmployeeByTeamLeadEmployeeId(int teamLeadEmployeeId);
    Task<Employee?> GetEmployeeByEmployeeId(int employeeId);
    bool IsEmployeeExists(int employeeId);
}
