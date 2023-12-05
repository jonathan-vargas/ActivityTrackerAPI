using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActivityTrackerAPI.Model;
using ActivityTrackerAPI.Repository;
using ActivityTrackerAPI.Validation;
using ActivityTrackerAPI.Utility;
using Microsoft.Extensions.Options;

namespace ActivityTrackerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActivityController : ControllerBase
{
    private readonly IActivityRepository _activityRepository;
    private readonly IActivityValidator _activityValidator;
    private readonly IEmployeeValidator _employeeValidator;
    private readonly ITeamRepository _teamRepository;
    public ActivityController(IActivityRepository activityRepository, IActivityValidator activityValidator, IEmployeeValidator employeeValidator, ITeamRepository teamRepository)
    {
        _activityRepository = activityRepository;
        _activityValidator = activityValidator;
        _employeeValidator = employeeValidator;
        _teamRepository = teamRepository;
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
    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult<IEnumerable<Activity>>> GetActivityByEmployeeId(int employeeId)
    {
        if (!await _employeeValidator.IsEmployeeIdValid(employeeId))
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        List<Activity>? activities;
        Team? team = await _teamRepository.GetTeamByEmployeeId(employeeId);
        if(team == null)
        {
            return NotFound();
        }
        if (await _employeeValidator.IsEmployeeTeamLead(employeeId))
        {
            activities = await _activityRepository.GetActivitiesByTeamId(team.TeamId);
        }
        else
        {
            activities = await _activityRepository.GetActivitiesByEmployeeId(employeeId);
        }

        return activities != null ? activities : NotFound();
    }
    // PUT: api/Activity/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{employeeId}/{activityId}")]
    public async Task<IActionResult> PutActivity(int employeeId, int activityId, Activity activity)
    {
        if(await _employeeValidator.IsEmployeeTeamLead(employeeId))
        {
            return StatusCode(StatusCodes.Status401Unauthorized, HttpStatusCodesMessages.HTTP_401_UNAUTHORIZED_MESSAGE);
        }

        if (!_activityValidator.IsActivityUpdateValid(activity, employeeId) || activityId != activity.ActivityId)
        {
            return BadRequest(HttpStatusCodesMessages.HTTP_400_BAD_REQUEST_MESSAGE);
        }

        if (!_activityValidator.IsInsertOrUpdateAllowed(activity, employeeId))
        {
            return StatusCode(StatusCodes.Status401Unauthorized, HttpStatusCodesMessages.HTTP_401_UNAUTHORIZED_MESSAGE);
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

    // POST: api/Activity/5/
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost("{employeeId}")]
    public async Task<ActionResult<Activity>> PostActivity(int employeeId, Activity activity)
    {
        if (await _employeeValidator.IsEmployeeTeamLead(employeeId))
        {
            return StatusCode(StatusCodes.Status401Unauthorized, HttpStatusCodesMessages.HTTP_401_UNAUTHORIZED_MESSAGE);
        }

        if (!_activityValidator.IsInsertOrUpdateAllowed(activity, employeeId))
        {
            return StatusCode(StatusCodes.Status401Unauthorized, HttpStatusCodesMessages.HTTP_401_UNAUTHORIZED_MESSAGE);
        }

        if (!_activityValidator.IsActivityInsertValid(activity, employeeId))
        {
            return BadRequest(HttpStatusCodesMessages.HTTP_400_BAD_REQUEST_MESSAGE);
        }

        Activity? insertedActivity = await _activityRepository.AddActivity(activity);
        if (insertedActivity == null)
            return StatusCode(StatusCodes.Status503ServiceUnavailable);

        return CreatedAtAction("GetActivity", new { id = activity.ActivityId }, activity);
    }

    // DELETE: api/Activity/5/5
    [HttpDelete("{employeeId}/{activityId}")]
    public async Task<IActionResult> DeleteActivity(int employeeId, int activityId)
    {
        if(!await _activityValidator.IsDeleteAllowed(activityId, employeeId))
        {
            return StatusCode(StatusCodes.Status401Unauthorized, HttpStatusCodesMessages.HTTP_401_UNAUTHORIZED_MESSAGE);
        }

        if (!_activityValidator.IsActivityDeleteValid(activityId))
        {
            return BadRequest(HttpStatusCodesMessages.HTTP_400_BAD_REQUEST_MESSAGE);
        }

        bool isDeleted = await _activityRepository.DeleteActivity(activityId);

        return (!isDeleted) ? NotFound() : NoContent();
    }

}
