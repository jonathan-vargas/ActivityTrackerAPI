using ActivityTrackerAPI.Model;

namespace ActivityTrackerAPI.Repository;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetEmployee();
    Task<Employee> AddEmployee(Employee employee);
    Task<Employee> UpdateEmployee(Employee employee);
    Task<Employee> DeleteEmployee(int activityId);
}
