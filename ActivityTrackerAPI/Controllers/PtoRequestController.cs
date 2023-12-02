using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActivityTrackerAPI.Model;
using ActivityTrackerAPI.Repository;
using ActivityTrackerAPI.DTO;

namespace ActivityTrackerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PtoRequestController : ControllerBase
{
    private readonly IPtoRequestRepository _ptoRequestRepository;

    public PtoRequestController(IPtoRequestRepository ptoRequestRepository)
    {
        this._ptoRequestRepository = ptoRequestRepository;
    }

    // GET: api/PTORequest
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PtoRequest>>> GetPtoRequest()
    {
        var ptoRequests = await _ptoRequestRepository.GetPtoRequest();

        return ptoRequests == null ? NotFound() : ptoRequests;
    }
    [HttpGet("{ptoRequestId}")]
    public async Task<ActionResult<PtoRequest>> GetPtoRequest(int ptoRequestId)
    {
        var ptoRequest = await _ptoRequestRepository.GetPtoRequestByPtoRequestId(ptoRequestId);

        return ptoRequest == null ? NotFound() : ptoRequest;
    }
    // GET: api/PTORequest/5
    [HttpGet("{employeeId}")]
    public async Task<ActionResult<IEnumerable<PtoRequest>>> GetPtoRequestByEmployeeId(int employeeId)
    {
        List<PtoRequest>? ptoRequest = await _ptoRequestRepository.GetPtoRequestByEmployeeId(employeeId);

        return ptoRequest == null ? NotFound() : ptoRequest;
    }

    [HttpGet("{teamId}")]
    public ActionResult<IEnumerable<PtoRequest>> GetPtoRequestByTeamId(int teamId)
    {
        List<PtoRequest>? ptoRequest = _ptoRequestRepository.GetPtoRequestByTeamId(teamId);

        return ptoRequest == null ? NotFound() : ptoRequest;
    }
    // PUT: api/PTORequest/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{ptoRequestId}")]
    public async Task<IActionResult> PutPtoRequest(int ptoRequestId, PtoRequest ptoRequest)
    {
        if (ptoRequestId != ptoRequest.PtoRequestId)
        {
            return BadRequest();
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
    [HttpPost]
    public async Task<ActionResult<PtoRequest>> PostPtoRequest(PtoRequest ptoRequest)
    {
        PtoRequest? insertedPTORequest = await _ptoRequestRepository.AddPtoRequest(ptoRequest);

        if (insertedPTORequest == null)
        {
            StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
        return CreatedAtAction("GetPTORequest", new { id = ptoRequest.PtoRequestId }, ptoRequest);
    }
    // DELETE: api/PTORequest/5
    [HttpDelete("{ptoRequestId}")]
    public async Task<IActionResult> DeletePtoRequest(int ptoRequestId)
    {
        if (!await _ptoRequestRepository.DeletePtoRequest(ptoRequestId))
        { 
            return NotFound(); 
        }
        return NoContent();
    }
    // PUT: api/PTORequest/5
    [HttpPut("{ptoRequestId}")]
    public async Task<ActionResult<PtoRequest>> ProcessPtoRequest(int ptoRequestId, PtoRequestDTO ptoRequestDTO)
    {
        if (ptoRequestDTO?.PtoStatusId is null or <= 0)
        {
            return BadRequest();
        }

        bool response;
        try
        {
            response = await _ptoRequestRepository.ProcessPtoRequest(ptoRequestId, ptoRequestDTO.PtoStatusId);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(StatusCodes.Status423Locked);
        }

        return (!response) ? NotFound() : Ok();
    }
}
