using ActivityTrackerAPI.Repository;
using ActivityTrackerAPI.Validation;

namespace ActivityTrackerAPI;

public static class ServicesExtensions
{
    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<IErrorRepository, ErrorRepository>();
        services.AddScoped<IPtoRequestRepository, PtoRequestRepository>();
        services.AddScoped<IPtoRequestReportsRepository, PtoRequestReportsRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IReportParametersValidator, ReportParametersValidator>();
        services.AddScoped<IActivityValidator, ActivityValidator>();
        services.AddScoped<IEmployeeValidator, EmployeeValidator>();
    }

    public static void ConfigureLoggingConsole(this IServiceCollection services)
    {
        services.AddLogging(builder =>
        {
            builder.AddConsole();     
        });
    }
}
