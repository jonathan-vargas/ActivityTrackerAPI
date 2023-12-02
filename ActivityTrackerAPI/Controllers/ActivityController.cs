using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActivityTrackerAPI.Model;
using ActivityTrackerAPI.Repository;

namespace ActivityTrackerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActivityController : ControllerBase
{
    private readonly IActivityRepository _activityRepository;
    public ActivityController(IActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    // GET: api/Activity
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Activity>>> GetActivity()
    {
        var activities = await _activityRepository.GetActivities();

        if (activities == null)
        {
            return NotFound();
        }
        return activities;
    }
    // GET: api/Activity/5
    [HttpGet("{activityId}")]
    public async Task<ActionResult<Activity>> GetActivity(int activityId)
    {
        var activity = await _activityRepository.GetActivityByActivityId(activityId);

        if (activity == null)
        {
            return NotFound();
        }

        return activity;
    }
    // GET: api/Activity/5
    [HttpGet("Employee/{employeeId}")]
    public async Task<ActionResult<IEnumerable<Activity>>> GetActivityByEmployeeId(int employeeId)
    {
        var activity = await _activityRepository.GetActivitiesByEmployeeId(employeeId);

        if (activity == null)
        {
            return NotFound();
        }

        return activity;
    }

    // GET: api/Activity/5
    [HttpGet("Team/{employeeId}")]
    public async Task<ActionResult<IEnumerable<Activity>>> GetActivityByTeamId(int teamId)
    {
        var activity = await _activityRepository.GetActivitiesByTeamId(teamId);

        if (activity == null)
        {
            return NotFound();
        }

        return activity;
    }
    // PUT: api/Activity/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutActivity(int activityId, Activity activity)
    {        
        if (activityId != activity.ActivityId)
        {
            return BadRequest();
        }

        bool response;
        try
        {
            response = await _activityRepository.UpdateActivity(activity);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(StatusCodes.Status423Locked);
        }

        return (!response) ? NotFound() : Ok() ;

    }

    // POST: api/Activity
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Activity>> PostActivity(Activity activity)
    {
        var insertedActivity = await _activityRepository.AddActivity(activity);

        if (insertedActivity == null)
            return StatusCode(StatusCodes.Status503ServiceUnavailable);

        return CreatedAtAction("GetActivity", new { id = activity.ActivityId }, activity);
    }

    // DELETE: api/Activity/5
    [HttpDelete("{activityId}")]
    public async Task<IActionResult> DeleteActivity(int activityId)
    {
        bool isDeleted = await _activityRepository.DeleteActivity(activityId);

        if (!isDeleted)
        {
            return NotFound();
        }

        return NoContent();
    }

}
