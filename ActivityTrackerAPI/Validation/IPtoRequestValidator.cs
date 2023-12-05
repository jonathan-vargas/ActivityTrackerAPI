using ActivityTrackerAPI.Model;

namespace ActivityTrackerAPI.Validation;

public interface IPtoRequestValidator
{
    Task<bool> IsPtoRequestInsertValid(PtoRequest ptoRequest, int employeeId);
    Task<bool> IsPtoRequestUpdateValid(PtoRequest ptoRequest, int employeeId);
    Task<bool> IsPtoRequestDeleteValid(int ptoRequestId);
    Task<bool> IsInsertOrUpdateAllowed(PtoRequest ptoRequest, int employeeId);
    Task<bool> IsDeleteAllowed(int ptoRequestId, int employeeId);
    Task<bool> IsGetActionAllowed(int employeeId);
    Task<bool> IsPtoRequestIdValid(int ptoRequestId);
    bool IsPtoRequestStatusIdValid(int ptoRequestStatusId);
    bool IsDateCollisionExist(DateTime startedDate, DateTime finishedDate, int employeeId, int ptoRequestId);
    Task<bool> isPtoRequestProcessValid(PtoRequest ptoRequest, int ptoRequestId, int employeeId);
    Task<bool> isPtoRequestProcessAllowed(int ptoRequestId, int employeeId);
}
