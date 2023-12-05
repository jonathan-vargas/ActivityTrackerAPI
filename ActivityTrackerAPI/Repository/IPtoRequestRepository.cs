using ActivityTrackerAPI.Model;

namespace ActivityTrackerAPI.Repository;

public interface IPtoRequestRepository
{
    Task<List<PtoRequest>?> GetPtoRequest();
    Task<PtoRequest?> AddPtoRequest(PtoRequest ptoRequest);
    Task<bool> UpdatePtoRequest(PtoRequest ptoRequest);
    Task<bool> DeletePtoRequest(int ptoRequestId);
    Task<bool> ProcessPtoRequest(int ptoRequestId, int ptoRequestStatusId);
    Task<List<PtoRequest>?> GetPtoRequestByEmployeeId(int employeeId);
    List<PtoRequest>? GetPtoRequestByTeamId(int teamId);
    Task<PtoRequest?> GetPtoRequestByPtoRequestId(int ptoRequestId);
    PtoRequest? GetPtoRequestByPtoRequestIdNoTracking(int ptoRequestId);
    int? GetPtoRequestDateCollision(DateTime starteDate, DateTime finishedDate, int employeeId);
}
