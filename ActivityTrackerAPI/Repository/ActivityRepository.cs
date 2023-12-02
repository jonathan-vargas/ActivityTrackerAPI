using ActivityTrackerAPI.Data;
using ActivityTrackerAPI.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using Activity = ActivityTrackerAPI.Model.Activity;

namespace ActivityTrackerAPI.Repository;

public class ActivityRepository : IActivityRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly IErrorRepository _errorRepository;
    private readonly ILogger<ActivityRepository> _logger;

    public ActivityRepository(AppDbContext appDbContext, IErrorRepository errorRepository, ILogger<ActivityRepository> logger)
    {
        this._appDbContext = appDbContext;
        this._errorRepository = errorRepository;
        this._logger = logger;
    }
    public async Task<List<Activity>?> GetActivities()
    {
        if (_appDbContext?.Activity == null)
            return null;

        return await _appDbContext.Activity.ToListAsync();
    }
    public async Task<Activity?> GetActivityByActivityId(int activityId)
    {
        if (_appDbContext?.Activity == null)
            return null;

        return await _appDbContext.Activity.FindAsync(activityId);
    }


    public async Task<List<Activity>?> GetActivitiesByEmployeeId(int employeeId)
    {
        if (_appDbContext?.Activity == null)
            return null;

        return await _appDbContext.Activity.Where(x => x.EmployeeId == employeeId).ToListAsync();
    }

    public async Task<List<Activity>?> GetActivitiesByTeamId(int teamId)
    {
        if (_appDbContext?.Activity == null)
            return null;

        var errorCodeId = 0;

        List<Activity> activitiesByTeam = await _appDbContext.Activity
                                                .FromSqlInterpolated($"EXECUTE dbo.spActivityGetByTeamId @TeamId={teamId}, @ErrorCodeId={errorCodeId} OUTPUT")
                                                .ToListAsync();

        if (errorCodeId != 0)
        {
            Error errorDetail = await _errorRepository.GetErrorById(errorCodeId);
            _logger.LogError(errorDetail.Description);
            return null;
        }

        return activitiesByTeam;
    }
    public async Task<Activity?> AddActivity(Activity activity)
    {
        _appDbContext.Activity.Add(activity);
        await _appDbContext.SaveChangesAsync();

        return (await _appDbContext.Activity.FindAsync(activity.ActivityId) == null) ? null : activity;
    }

    public async Task<bool> DeleteActivity(int activityId)
    {
        Activity? activityToDelete = await _appDbContext.Activity.FindAsync(activityId);

        if (activityToDelete == null)
        {
            return false;
        }

        try
        {
            _appDbContext.Activity.Remove(activityToDelete);
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            return false;
        }
        
        return true;
    }

    public async Task<bool> UpdateActivity(Activity activity)
    {
        if (activity.ActivityId == 0)
        {
            return false;
        }

        _appDbContext.Entry(activity).State = EntityState.Modified;

        try
        {
            await _appDbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ActivityExists(activity.ActivityId))
            {
                return false;
            }
            else
            {
                throw;
            }
        }

        return true;
    }
    private bool ActivityExists(int id)
    {
        return (_appDbContext.Activity?.Any(e => e.ActivityId == id)).GetValueOrDefault();
    }
}
