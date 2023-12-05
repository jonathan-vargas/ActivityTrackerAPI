using ActivityTrackerAPI.Model;
using ActivityTrackerAPI.Repository;
using ActivityTrackerAPI.Utility;
using ActivityTrackerAPI.Validation;
using System.Configuration;

namespace ActivityTrackerAPI;

public static class ServicesExtensions
{
    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<IErrorRepository, ErrorRepository>();
        services.AddScoped<IPtoRequestRepository, PtoRequestRepository>();
        services.AddScoped<IActivityReportRepository, ActivityReportsRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IActivityReportValidator, ActivityReportValidator>();
        services.AddScoped<IActivityValidator, ActivityValidator>();
        services.AddScoped<IEmployeeValidator, EmployeeValidator>();
        services.AddScoped<ITeamValidator, TeamValidator>();
        services.AddScoped<IPtoRequestValidator, PtoRequestValidator>();
        services.AddScoped<IAppMailSender, AppMailSender>();
    }

    public static void ConfigureLoggingConsole(this IServiceCollection services)
    {
        services.AddLogging(builder =>
        {
            builder.AddConsole();     
        });
    }
}
