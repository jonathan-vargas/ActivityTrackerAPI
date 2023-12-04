using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActivityTrackerAPI.Model;
using ActivityTrackerAPI.Repository;
using ActivityTrackerAPI.DTO;
using ActivityTrackerAPI.Validation;
using ActivityTrackerAPI.Utility;
using System.Diagnostics;
using Microsoft.Extensions.Options;

namespace ActivityTrackerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PtoRequestController : ControllerBase
{
    private readonly IPtoRequestRepository _ptoRequestRepository;
    private readonly IEmployeeValidator _employeeValidator;
    private readonly ITeamRepository _teamRepository;
    private readonly IPtoRequestValidator _ptoRequestValidator;
    private readonly EmailConfiguration _emailConfiguration;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<PtoRequest> _logger;
    public PtoRequestController(IPtoRequestRepository ptoRequestRepository, IEmployeeValidator employeeValidator, ITeamRepository teamRepository, IPtoRequestValidator ptoRequestValidator, IOptions<EmailConfiguration> emailConfiguration, IEmployeeRepository employeeRepository, ILogger<PtoRequest> logger)
    {
        _ptoRequestRepository = ptoRequestRepository;
        _employeeValidator = employeeValidator;
        _teamRepository = teamRepository;
        _ptoRequestValidator = ptoRequestValidator;
        _emailConfiguration = emailConfiguration.Value;
        _employeeRepository = employeeRepository;
        _logger = logger;
    }

    // GET: api/PTORequest
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PtoRequest>>> GetPtoRequest()
    {
        var ptoRequests = await _ptoRequestRepository.GetPtoRequest();

        return ptoRequests == null ? NotFound() : ptoRequests;
    }
    [HttpGet("{ptoRequestId}")]
    public async Task<ActionResult<PtoRequest>> GetPtoRequest(int employeeId, int ptoRequestId)
    {
        var ptoRequest = await _ptoRequestRepository.GetPtoRequestByPtoRequestId(ptoRequestId);

        return ptoRequest == null ? NotFound() : ptoRequest;
    }
    // GET: api/PTORequest/5
    [HttpGet("{employeeId}")]
    public async Task<ActionResult<IEnumerable<PtoRequest>>> GetPtoRequestByEmployeeId(int employeeId)
    {
        if (!await _employeeValidator.IsEmployeeIdValid(employeeId))
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        List<PtoRequest>? ptoRequests;
        if (await _employeeValidator.IsEmployeeTeamLead(employeeId))
        {
            Team? team = await _teamRepository.GetTeamByEmployeeId(employeeId);
            if(team != null)
            {
                ptoRequests = _ptoRequestRepository.GetPtoRequestByTeamId(team.TeamId);
            }            
        }
        else
        {
            ptoRequests = await _ptoRequestRepository.GetPtoRequestByEmployeeId(employeeId);
        }

        return ptoRequests == null ? NotFound() : ptoRequests;
    }
    // PUT: api/PTORequest/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{employeeId}/{ptoRequestId}")]
    public async Task<IActionResult> PutPtoRequest(int employeeId, int ptoRequestId, PtoRequest ptoRequest)
    {
        if (await _employeeValidator.IsEmployeeTeamLead(employeeId))
        {
            return StatusCode(StatusCodes.Status401Unauthorized, HttpStatusCodesMessages.HTTP_401_UNAUTHORIZED_MESSAGE);
        }

        if (!await _ptoRequestValidator.IsPtoRequestUpdateValid(ptoRequest, employeeId) || ptoRequestId != ptoRequest.PtoRequestId)
        {
            return BadRequest(HttpStatusCodesMessages.HTTP_400_BAD_REQUEST_MESSAGE);
        }

        if (!await _ptoRequestValidator.IsInsertOrUpdateAllowed(ptoRequest, employeeId))
        {
            return StatusCode(StatusCodes.Status401Unauthorized, HttpStatusCodesMessages.HTTP_401_UNAUTHORIZED_MESSAGE);
        }
        
        bool response;
        try
        {
            response = await _ptoRequestRepository.UpdatePtoRequest(ptoRequest);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(StatusCodes.Status423Locked);
        }

        return (!response) ? NotFound() : Ok();
    }
    // POST: api/PTORequest
    [HttpPost("{employeeId}")]
    public async Task<ActionResult<PtoRequest>> PostPtoRequest(int employeeId, PtoRequest ptoRequest)
    {
        if (await _employeeValidator.IsEmployeeTeamLead(employeeId))
        {
            return StatusCode(StatusCodes.Status401Unauthorized, HttpStatusCodesMessages.HTTP_401_UNAUTHORIZED_MESSAGE);
        }

        if (!await _ptoRequestValidator.IsInsertOrUpdateAllowed(ptoRequest, employeeId))
        {
            return StatusCode(StatusCodes.Status401Unauthorized, HttpStatusCodesMessages.HTTP_401_UNAUTHORIZED_MESSAGE);
        }

        if (!await _ptoRequestValidator.IsPtoRequestInsertValid(ptoRequest, employeeId))
        {
            return BadRequest(HttpStatusCodesMessages.HTTP_400_BAD_REQUEST_MESSAGE);
        }

        PtoRequest? insertedPTORequest = await _ptoRequestRepository.AddPtoRequest(ptoRequest);

        if (insertedPTORequest == null)
        {
            StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
        else
        {
            setupAndSendEmail(insertedPTORequest, _emailConfiguration, _logger);
        }
        
        return CreatedAtAction("GetPTORequest", new { id = ptoRequest.PtoRequestId }, ptoRequest);
    }
    // DELETE: api/PTORequest/5
    [HttpDelete("{employeeId}/{ptoRequestId}")]
    public async Task<IActionResult> DeletePtoRequest(int employeeId, int ptoRequestId)
    {
        if (!await _ptoRequestValidator.IsDeleteAllowed(ptoRequestId, employeeId))
        {
            return StatusCode(StatusCodes.Status401Unauthorized, HttpStatusCodesMessages.HTTP_401_UNAUTHORIZED_MESSAGE);
        }

        if (!await _ptoRequestValidator.IsPtoRequestDeleteValid(ptoRequestId))
        {
            return BadRequest(HttpStatusCodesMessages.HTTP_400_BAD_REQUEST_MESSAGE);
        }

        if (!await _ptoRequestRepository.DeletePtoRequest(ptoRequestId))
        { 
            return NotFound(); 
        }
        return NoContent();
    }
    // PUT: api/PTORequest/5
    [HttpPatch("{employeeId}/{ptoRequestId}")]
    public async Task<ActionResult<PtoRequest>> PatchProcessPtoRequest(int employeeId, int ptoRequestId, PtoRequest ptoRequest)
    {
        if (!await _ptoRequestValidator.isPtoRequestProcessValid(ptoRequest, ptoRequestId, employeeId))
        {
            return BadRequest(HttpStatusCodesMessages.HTTP_400_BAD_REQUEST_MESSAGE);
        }

        if (!await _ptoRequestValidator.isPtoRequestProcessAllowed(ptoRequestId, employeeId))
        {
            return StatusCode(StatusCodes.Status401Unauthorized, HttpStatusCodesMessages.HTTP_401_UNAUTHORIZED_MESSAGE);
        }

        bool response;
        try
        {
            response = await _ptoRequestRepository.ProcessPtoRequest(ptoRequestId, ptoRequest.PtoStatusId);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(StatusCodes.Status423Locked);
        }

        return (!response) ? NotFound() : Ok();
    }

    private async void setupAndSendEmail(PtoRequest ptoRequest, EmailConfiguration emailConfiguration, ILogger<PtoRequest> _logger)
    {
        Employee? employee = await _employeeRepository.GetEmployeeByEmployeeId(ptoRequest.EmployeeId);
        Team? team = await _teamRepository.GetTeamByEmployeeId(ptoRequest.EmployeeId);
        if(employee == null && team == null)
        {
            return;
        }
        Employee? teamLead = await _employeeRepository.GetEmployeeByEmployeeId(team.TeamLeadEmployeeId);
        if(teamLead == null)
        {
            return;
        }

        EmailContent emailContent = new EmailContent()
        {
            From = employee?.Email,
            To = teamLead.Email,
            Subject = $"{Parameters.PTO_REQUEST_EMAIL_SUBJECT}:[{employee.Name} {employee.PaternalLastName} {employee.MaternalLastName}",
            Body = $"{Parameters.PTO_REQUEST_EMAIL_BODY} - {employee.Name} {employee.PaternalLastName} {employee.MaternalLastName} ({employee.EmployeeId})"
        };

        AppMailSender.SendEmail(emailConfiguration, emailContent, _logger);
    }
}
