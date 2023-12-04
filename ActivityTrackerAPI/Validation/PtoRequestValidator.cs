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
    public PtoRequestValidator(IEmployeeValidator employeeValidator, IPtoRequestRepository ptoRequestRepository)
    {
        _employeeValidator = employeeValidator;
        _ptoRequestRepository = ptoRequestRepository;
    }

    public Task<bool> IsGetActionAllowed(int employeeId)
    {
        throw new NotImplementedException();
    }
    public async Task<bool> IsPtoRequestInsertValid(PtoRequest ptoRequest, int employeeId)
    {
        await IsPtoRequestPropertiesValid(ptoRequest, isIncludeIdCheck: false);
        return false;
    }

    public async Task<bool> IsPtoRequestUpdateValid(PtoRequest ptoRequest, int employeeId)
    {
        if(!await IsPtoRequestPropertiesValid(ptoRequest, isIncludeIdCheck: true))
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

        if (ptoRequest.PtoStatusId != AppStatusCodes.PTO_REQUEST_STATUS_PENDING)
        {
            return false;
        }

        return true;
    }
    public async Task<bool> IsInsertOrUpdateAllowed(PtoRequest ptoRequest, int employeeId)
    {
        if (ptoRequest.PtoStatusId != AppStatusCodes.PTO_REQUEST_STATUS_PENDING)
        {
            return false;
        }

        if (!await _employeeValidator.IsEmployeeIdValid(employeeId))
        {
            return false;
        }

        if (!await _employeeValidator.IsEmployeeTeamLead(employeeId))
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

        return false;    
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
        if (ptoRequestStatusId <= 0 || ptoRequestStatusId == AppStatusCodes.PTO_REQUEST_STATUS_PENDING)
        {
            return false;
        }

        return true;
    }
    public bool IsDateCollisionExist(DateTime startedDate, DateTime finishedDate, int employeeId)
    {
        int? collissions = _ptoRequestRepository.GetPtoRequestDateCollision(startedDate, finishedDate, employeeId);
        if (!collissions.HasValue || collissions > 0)
        {
            return false;
        }

        return true;
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

        if (IsDateCollisionExist(ptoRequest.StartedDate, ptoRequest.FinishedDate, ptoRequest.EmployeeId))
        {
            return false;
        }

        if (!await _employeeValidator.IsEmployeeIdValid(ptoRequest.TeamLeadEmployeeId) ||
           !await _employeeValidator.IsEmployeeTeamLead(ptoRequest.TeamLeadEmployeeId))
        {
            return false;
        }

        if (!await _employeeValidator.IsEmployeeIdValid(ptoRequest.EmployeeId) ||
            !await _employeeValidator.IsEmployeeTeamLead(ptoRequest.EmployeeId))
        {
            return false;
        }

        if (ptoRequest.PtoStatusId <= 0 || !IsPtoRequestStatusIdValid(ptoRequest.PtoStatusId))
        {
            return false;
        }
        DateTime startedDateUTC = ptoRequest.StartedDate.ToUniversalTime();
        DateTime finishedDateUTC = ptoRequest.FinishedDate.ToUniversalTime();

        int startedDateYear = startedDateUTC.Year;
        int startedDateMonth = startedDateUTC.Month;
        int startedDateDay = startedDateUTC.Day;

        int finishedDateYear = finishedDateUTC.Year;
        int finishedDateMonth = finishedDateUTC.Month;
        int finishedDateDay = finishedDateUTC.Day;

        if (startedDateYear <= DateTime.UtcNow.Year ||
            finishedDateYear <= DateTime.UtcNow.Year ||
            startedDateMonth <= DateTime.UtcNow.Month ||
            finishedDateMonth <= DateTime.UtcNow.Month ||
            startedDateDay <= DateTime.UtcNow.Day ||
            finishedDateDay <= DateTime.UtcNow.Day)
        {
            return false;
        }


        return true;
    }
}
