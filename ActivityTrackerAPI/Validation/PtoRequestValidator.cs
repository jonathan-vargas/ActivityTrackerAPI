using ActivityTrackerAPI.Model;
using ActivityTrackerAPI.Repository;
using ActivityTrackerAPI.Utility;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ActivityTrackerAPI.Validation;

public class PtoRequestValidator : IPtoRequestValidator
{
    private readonly IEmployeeValidator _employeeValidator;
    private readonly IPtoRequestRepository _ptoRequestRepository;
    private readonly IEmployeeRepository _employeeRepository;
    public PtoRequestValidator(IEmployeeValidator employeeValidator, IPtoRequestRepository ptoRequestRepository, IEmployeeRepository employeeRepository)
    {
        _employeeValidator = employeeValidator;
        _ptoRequestRepository = ptoRequestRepository;
        _employeeRepository = employeeRepository;
    }

    public Task<bool> IsGetActionAllowed(int employeeId)
    {
        throw new NotImplementedException();
    }
    public async Task<bool> IsPtoRequestInsertValid(PtoRequest ptoRequest, int employeeId)
    {
        if (ptoRequest.PtoStatusId <= 0 || ptoRequest.PtoStatusId != AppStatusCodes.PTO_REQUEST_STATUS_PENDING)
        {
            return false;
        }

        if (!await IsPtoRequestPropertiesValid(ptoRequest, isIncludeIdCheck: false)){
            return false;
        }
        return true;
    }

    public async Task<bool> IsPtoRequestUpdateValid(PtoRequest ptoRequest, int employeeId)
    {    
        if (!await IsPtoRequestPropertiesValid(ptoRequest, isIncludeIdCheck: true))
        {
            return false;
        }

        if (ptoRequest.PtoStatusId <= 0 || ptoRequest.PtoStatusId == AppStatusCodes.PTO_REQUEST_STATUS_APPROVED)
        {
            return false;
        }

        PtoRequest? ptoRequestFromDb = _ptoRequestRepository.GetPtoRequestByPtoRequestIdNoTracking(ptoRequest.PtoRequestId);
        if(ptoRequestFromDb == null)
        {
            return false;
        }

        if(ptoRequestFromDb.PtoStatusId == AppStatusCodes.PTO_REQUEST_STATUS_CANCELED)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> IsPtoRequestDeleteValid(int ptoRequestId)
    {
        PtoRequest? ptoRequest = await _ptoRequestRepository.GetPtoRequestByPtoRequestId(ptoRequestId);
        if (ptoRequest == null)
        {
            return false;
        }

        return true;
    }
    public async Task<bool> IsInsertOrUpdateAllowed(PtoRequest ptoRequest, int employeeId)
    {
        if (!await _employeeValidator.IsEmployeeIdValid(employeeId))
        {
            return false;
        }

        if (await _employeeValidator.IsEmployeeTeamLead(employeeId))
        {
            return false;
        }

        if(ptoRequest.EmployeeId != employeeId)
        {
            return false;
        }

        if(!await _employeeValidator.IsEmployeeAndTeamLeadFromSameTeam(employeeId, ptoRequest.TeamLeadEmployeeId))
        {
            return false;
        }

        return true;
    }
    public async Task<bool> IsDeleteAllowed(int ptoRequestId, int employeeId)
    {
        PtoRequest? ptoRequest = await _ptoRequestRepository.GetPtoRequestByPtoRequestId(ptoRequestId);
        if (ptoRequest == null)
        {
            return false;
        }

        if(ptoRequest.EmployeeId != employeeId)
        {
            return false;
        }

        if (!await _employeeValidator.IsEmployeeIdValid(employeeId))
        {
            return false;
        }

        if (await _employeeValidator.IsEmployeeTeamLead(employeeId))
        {
            return false;
        }

        if(ptoRequest.PtoStatusId == AppStatusCodes.PTO_REQUEST_STATUS_APPROVED)
        {
            return false;
        }

        return true;    
    }

    public async Task<bool> IsPtoRequestIdValid(int ptoRequestId)
    {
        if (ptoRequestId <= 0 || await _ptoRequestRepository.GetPtoRequestByPtoRequestId(ptoRequestId) == null)
        {
            return false;
        }

        return true;
    }

    public bool IsPtoRequestStatusIdValid(int ptoRequestStatusId)
    {
        if (ptoRequestStatusId <= 0 || ptoRequestStatusId == AppStatusCodes.PTO_REQUEST_STATUS_APPROVED)
        {
            return false;
        }

        return true;
    }
    public bool IsDateCollisionExist(DateTime startedDate, DateTime finishedDate, int employeeId, int ptoRequestId)
    {
        startedDate = startedDate.Date; 
        finishedDate = finishedDate.Date;
        PtoRequest? ptoRequest = _ptoRequestRepository.GetPtoRequestByPtoRequestIdNoTracking(ptoRequestId);
        int? collissions;
        if(ptoRequest == null)
        {
            collissions = _ptoRequestRepository.GetPtoRequestDateCollision(startedDate, finishedDate, employeeId);
            return collissions > 0;
        }

        DateTime startedDateFromDb = ptoRequest.StartedDate.Date;
        DateTime finishedDateFromDb = ptoRequest.FinishedDate.Date;
        bool isDateUpdated = (startedDate != startedDateFromDb || finishedDate != finishedDateFromDb) && ptoRequestId > 0;
        collissions = (isDateUpdated)? _ptoRequestRepository.GetPtoRequestDateCollision(startedDate, finishedDate, employeeId): 0;
        if (!collissions.HasValue)
        {
            return false;
        }
        
        return collissions > 0;
    }
    public async Task<bool> isPtoRequestProcessValid(PtoRequest ptoRequest, int ptoRequestId, int employeeId)
    {
        if(ptoRequest.PtoRequestId != ptoRequestId)
        {
            return false;
        }

        if (!await IsPtoRequestIdValid(ptoRequestId))
        {
            return false;
        }

        if (ptoRequest.PtoStatusId == AppStatusCodes.PTO_REQUEST_STATUS_APPROVED)
        {
            return false;
        }

        if (!await _employeeValidator.IsEmployeeIdValid(employeeId))
        {
            return false;
        }

        if (ptoRequest.PtoStatusId <= 0)
        {
            return false;
        }

        return true;
    }
    public async Task<bool> isPtoRequestProcessAllowed(int ptoRequestId, int employeeId)
    {
        if (!await _employeeValidator.IsEmployeeTeamLead(employeeId))
        {
            return false;
        }

        return true;
    }
    private async Task<bool> IsPtoRequestPropertiesValid(PtoRequest ptoRequest, bool isIncludeIdCheck)
    {
        if (isIncludeIdCheck && ptoRequest.PtoRequestId <= 0)
        {
            return false;
        }

        if (!isIncludeIdCheck && ptoRequest.PtoRequestId > 0)
        {
            return false;
        }

        if (ptoRequest.StartedDate == DateTime.MinValue || ptoRequest.FinishedDate == DateTime.MinValue ||
        ptoRequest.StartedDate == DateTime.MaxValue || ptoRequest.FinishedDate == DateTime.MaxValue)
        {
            return false;
        }

        if (ptoRequest.StartedDate > ptoRequest.FinishedDate)
        {
            return false;
        }

        if (!await _employeeValidator.IsEmployeeIdValid(ptoRequest.TeamLeadEmployeeId) ||
           !await _employeeValidator.IsEmployeeTeamLead(ptoRequest.TeamLeadEmployeeId))
        {
            return false;
        }

        DateTime startedDateUTC = ptoRequest.StartedDate.ToUniversalTime();
        DateTime finishedDateUTC = ptoRequest.FinishedDate.ToUniversalTime();        
        DateOnly startedDate = new DateOnly(startedDateUTC.Year, startedDateUTC.Month, startedDateUTC.Day);
        DateOnly finishedDate = new DateOnly(finishedDateUTC.Year, finishedDateUTC.Month, finishedDateUTC.Day);
        DateOnly currentDate = new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);

        if (startedDate <= currentDate || finishedDate <= currentDate)
        {
            return false;
        }

        return true;
    }
}
