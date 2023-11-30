using ActivityTrackerAPI.Data;
using ActivityTrackerAPI.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace ActivityTrackerAPI.Repository;

public class ActivityRepository : IActivityRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly IErrorRepository _errorRepository;

    public ActivityRepository(AppDbContext appDbContext, IErrorRepository errorRepository)
    {
        this._appDbContext = appDbContext;
        this._errorRepository = errorRepository;
    }
    public async Task<List<Activity>> GetActivitiesByEmployeeId(int employeeId)
    {
        if (_appDbContext?.Activity == null)
            return new List<Activity>();

        return await _appDbContext.Activity.Where(x => x.EmployeeId == employeeId).ToListAsync();
    }

    public async Task<List<Activity>> GetActivitiesByTeamId(int teamId)
    {
        if (_appDbContext?.Activity == null)
            return new List<Activity>();

        var errorCodeId = 0;

        List<Activity> activitiesByTeam = await _appDbContext.Activity
                                                .FromSqlInterpolated($"EXECUTE dbo.spActivityGetByTeamId @TeamId={teamId}, @ErrorCodeId={errorCodeId} OUTPUT")
                                                .ToListAsync();

        if(errorCodeId != 0)
        {
            Error errorDetail = await _errorRepository.GetErrorById(errorCodeId);
            throw new Exception(errorDetail.Description);
        }

        return activitiesByTeam;
    }
    public async Task<Activity> AddActivity(Activity activity)
    {
        _appDbContext.Activity.Add(activity);
        await _appDbContext.SaveChangesAsync();

        return (await _appDbContext.Activity.FindAsync(activity.ActivityId) == null) ? new Activity() : activity;
    }

    public async Task<Activity?> DeleteActivity(int activityId)
    {
        if (_appDbContext?.Activity?.FindAsync(activityId) == null)
        {
            return null;
        }
        Activity? activityToDelete = await _appDbContext.Activity.FindAsync(activityId);

        if (activityToDelete == null)
            return null;

        _appDbContext.Activity.Remove(activityToDelete);
        await _appDbContext.SaveChangesAsync();
        
        return new Activity();
    }

    public async Task<Activity?> UpdateActivity(Activity activity)
    {
        if (activity.ActivityId == 0)
        {
            return null;
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
                return null;
            }
            else
            {
                throw;
            }
        }

        return activity;
    }
    private bool ActivityExists(int id)
    {
        return (_appDbContext.Activity?.Any(e => e.ActivityId == id)).GetValueOrDefault();
    }
}
