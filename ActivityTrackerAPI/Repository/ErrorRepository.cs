
using ActivityTrackerAPI.Data;
using ActivityTrackerAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace ActivityTrackerAPI.Repository;

public class ErrorRepository : IErrorRepository
{
    private readonly AppDbContext _appDbContext;

    public ErrorRepository(AppDbContext appDbContext)
    {
        this._appDbContext = appDbContext;
    }
    public async Task<Error> GetErrorById(int errorId)
    {
        if (_appDbContext?.Error == null)
            return new Error();

        return await _appDbContext.Error.Where(x => x.ErrorId == errorId).FirstAsync();
    }
}
