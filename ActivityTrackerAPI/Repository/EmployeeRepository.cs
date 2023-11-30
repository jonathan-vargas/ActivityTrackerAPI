using ActivityTrackerAPI.Data;
using ActivityTrackerAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace ActivityTrackerAPI.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _appDbContext;

    public EmployeeRepository(AppDbContext context)
    {
        this._appDbContext = context;
    }
    public async Task<List<Employee>> GetEmployee()
    {
        if (_appDbContext?.Employee == null)
            return new List<Employee>();

        return await _appDbContext.Employee.ToListAsync();
    }
    public Task<Employee> AddEmployee(Employee employee)
    {
        throw new NotImplementedException();
    }

    public Task<Employee> DeleteEmployee(int activityId)
    {
        throw new NotImplementedException();
    }

    public Task<Employee> UpdateEmployee(Employee activity)
    {
        throw new NotImplementedException();
    }
}
