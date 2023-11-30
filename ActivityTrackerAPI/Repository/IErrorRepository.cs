using ActivityTrackerAPI.Model;

namespace ActivityTrackerAPI.Repository;

public interface IErrorRepository
{
    Task<Error> GetErrorById(int errorId);
}
