using ActivityTrackerAPI.Data;
using ActivityTrackerAPI.Model;
using ActivityTrackerAPI.Utility;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ActivityTrackerAPI.Repository;

public class PtoRequestRepository : IPtoRequestRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly ILogger<PtoRequestRepository> _logger;

    public PtoRequestRepository(AppDbContext appDBContext, ILogger<PtoRequestRepository> logger)
    {
        this._appDbContext = appDBContext;
        this._logger = logger;
    }
    public async Task<PtoRequest?> AddPtoRequest(PtoRequest ptoRequest)
    {
        if(_appDbContext?.PtoRequest == null)
        {
            return null;
        }
        _appDbContext.PtoRequest.Add(ptoRequest);
        await _appDbContext.SaveChangesAsync();
        return (await _appDbContext.PtoRequest.FindAsync(ptoRequest.PtoRequestId) == null) ? null : ptoRequest;
    }
    public async Task<bool> DeletePtoRequest(int ptoRequestId)
    {
       PtoRequest? pTORequestToDelete = await _appDbContext.PtoRequest.FindAsync(ptoRequestId);

        if (pTORequestToDelete == null)
        {
            return false;
        }

        try
        {
            _appDbContext.PtoRequest.Remove(pTORequestToDelete);
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public async Task<List<PtoRequest>?> GetPtoRequest()
    {
        if (_appDbContext?.PtoRequest == null)
            return null;

        return await _appDbContext.PtoRequest.ToListAsync();
    }

    public async Task<List<PtoRequest>?> GetPtoRequestByEmployeeId(int employeeId)
    {
        if (_appDbContext?.PtoRequest == null)
            return null;

        return await _appDbContext.PtoRequest.Where(x => x.EmployeeId == employeeId).ToListAsync();
    }

    public async Task<PtoRequest?> GetPtoRequestByPtoRequestId(int ptoRequestId)
    {
        if (_appDbContext?.PtoRequest == null)
            return null;

        return await _appDbContext.PtoRequest.FindAsync(ptoRequestId);
    }

    public List<PtoRequest>? GetPtoRequestByTeamId(int teamId)
    {
        if (_appDbContext?.Activity == null || _appDbContext.Team == null)
            return null;

        var pTORequestsByTeamId = from pTORequest in _appDbContext.PtoRequest
                                  join employee in _appDbContext.Employee on pTORequest.EmployeeId equals employee.EmployeeId
                                  where employee.TeamId == teamId
                                  select new PtoRequest
                                  {
                                      PtoRequestId = pTORequest.PtoRequestId,
                                      StartedDate = pTORequest.StartedDate,
                                      FinishedDate = pTORequest.FinishedDate,
                                      PtoStatusId = pTORequest.PtoStatusId,
                                      EmployeeId = pTORequest.EmployeeId,
                                      TeamLeadEmployeeId = pTORequest.TeamLeadEmployeeId
                                  };

        if (pTORequestsByTeamId == null)
        {
            var methodName = new StackTrace().GetFrame(0)?.GetMethod()?.Name;
            _logger.LogError(ErrorMessages.ERROR_WHILE_EXECUTING_LINQ_QUERY + (methodName != null ? methodName : ""));

            return null;
        }

        return pTORequestsByTeamId.ToList<PtoRequest>();
    }

    public async Task<bool> ProcessPtoRequest(int pTORequestId, int pTORequestStatusId)
    {
        var pTORequestToProcess = await _appDbContext.PtoRequest.FindAsync(pTORequestId);

        if (pTORequestToProcess == null)
        {
            return false;
        }

        if (pTORequestStatusId != Utility.AppStatusCodes.PTO_REQUEST_STATUS_APPROVED && pTORequestStatusId != Utility.AppStatusCodes.PTO_REQUEST_STATUS_CANCELED)
        {
            return false;
        }
        
        pTORequestToProcess.PtoStatusId = pTORequestStatusId;
        await UpdatePtoRequest(pTORequestToProcess);

        return true;
    }

    public async Task<bool> UpdatePtoRequest(PtoRequest pTORequest)
    {
        if (pTORequest.PtoRequestId == 0)
        {
            return false;
        }

        _appDbContext.Entry(pTORequest).State = EntityState.Modified;

        try
        {
            await _appDbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PTORequestExists(pTORequest.PtoRequestId))
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

    private bool PTORequestExists(int id)
    {
        return (_appDbContext.PtoRequest?.Any(e => e.PtoRequestId == id)).GetValueOrDefault();
    }
}
