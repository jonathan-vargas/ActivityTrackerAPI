using ActivityTrackerAPI.Repository;

namespace ActivityTrackerAPI;

public static class ServicesExtensions
{
    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<IErrorRepository, ErrorRepository>();
    }
}
