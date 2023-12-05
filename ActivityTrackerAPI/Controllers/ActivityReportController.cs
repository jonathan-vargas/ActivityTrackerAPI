using ActivityTrackerAPI.Model;
using ActivityTrackerAPI.Repository;
using ActivityTrackerAPI.Utility;
using ActivityTrackerAPI.Validation;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ActivityTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityReportController : ControllerBase
    {
        private readonly IActivityReportValidator _activityReportValidator;
        private readonly IEmployeeValidator _employeeValidator;
        private readonly IActivityReportRepository _activityReportRepository;
        private readonly IEmployeeRepository _employeeRepository;
        public ActivityReportController(IActivityReportValidator activityReportValidator, IEmployeeValidator employeeValidator, IActivityReportRepository activityReportRepository, IEmployeeRepository employeeRepository)
        {
            _activityReportValidator = activityReportValidator;
            _employeeValidator = employeeValidator;
            _activityReportRepository = activityReportRepository;
            _employeeRepository = employeeRepository;
        }

        // GET: api/<ActivityReportController>
        [HttpGet("ActivityReport/{employeeId}/")]
        public async Task<ActionResult<IEnumerable<ActivityReport>>> GetActivityReportByEmployeeId(int employeeId, [FromQuery] DateTime? startedDateParameter, 
            [FromQuery] DateTime? finishedDateParameter, [FromQuery] int employeeIdParameter)
        {
            if (!await _employeeValidator.IsEmployeeIdValid(employeeId))
            {
                return BadRequest(HttpStatusCodesMessages.HTTP_400_BAD_REQUEST_MESSAGE);
            }

            if (!_activityReportValidator.IsDateRangesValid(startedDateParameter, finishedDateParameter))
            {
                return BadRequest(HttpStatusCodesMessages.HTTP_400_BAD_REQUEST_MESSAGE);
            }

            if (!await _activityReportValidator.IsEmployeeIdParameterValid(employeeIdParameter))
            {
                return BadRequest(HttpStatusCodesMessages.HTTP_400_BAD_REQUEST_MESSAGE);
            }
            
            Dictionary<string, DateTime> dateParameters = new Dictionary<string, DateTime>();            
            if (startedDateParameter != null)
            {
                dateParameters.Add(Parameters.TO_REQUEST_REPORT_INITAL_DATE_PARAMETER, (DateTime)startedDateParameter);
            }

            if (finishedDateParameter != null)
            {
                dateParameters.Add(Parameters.TO_REQUEST_REPORT_FINAL_DATE_PARAMETER, (DateTime)finishedDateParameter);
            }

            if (!await _activityReportValidator.IsReportActionAllowed(employeeId, employeeIdParameter))
            {
                return BadRequest(HttpStatusCodesMessages.HTTP_400_BAD_REQUEST_MESSAGE);
            }
            Employee? employee = await _employeeRepository.GetEmployeeByEmployeeId(employeeId);
            int teamIdParameter = 0;
            if (employee != null && await _employeeValidator.IsEmployeeTeamLead(employeeId))
            {
                teamIdParameter = employee.TeamId;
            }

            List<ActivityReport>? activityReports = _activityReportRepository.GetActivityReport(dateParameters, employeeIdParameter, teamIdParameter);

            return (activityReports != null)? activityReports: NotFound();
        }
    }
}
