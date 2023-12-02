using ActivityTrackerAPI.Data;
using ActivityTrackerAPI.Model;
using ActivityTrackerAPI.Utility;
using ActivityTrackerAPI.Validation;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace ActivityTrackerAPI.Repository
{
    public class PtoRequestReportsRepository : IPtoRequestReportsRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IReportParametersValidator _reportParametersValidator;
        private readonly ILogger _logger;
        public PtoRequestReportsRepository(AppDbContext appDbContext, IReportParametersValidator reportParametersValidator, ILogger logger) 
        {
            _appDbContext = appDbContext;
            _reportParametersValidator = reportParametersValidator;
            _logger = logger;
        }
        public List<PtoRequestReport>? GetActivityReport(Dictionary<string, DateTime> dateParameters, int employeeIdParameter, int teamIdParameter)
        {
            var activityReport = from activity in _appDbContext.Activity
                                 join employee in _appDbContext.Employee on activity.EmployeeId equals employee.EmployeeId
                                 join team in _appDbContext.Team on employee.TeamId equals team.TeamId
                                 select new PtoRequestReport
                                 {
                                     StartedDate = activity.StartedDate,
                                     FinishedDate = activity.FinishedDate,
                                     Description = activity.Description,
                                     Duration = EF.Functions.DateDiffDay(activity.StartedDate, activity.FinishedDate),
                                     EmployeeId = employee.EmployeeId,
                                     TeamId = team.TeamId
                                 };


            activityReport = ApplyDateRangeFilter(activityReport, dateParameters);
            activityReport = ApplyEmployeeIdFilter(activityReport, employeeIdParameter);
            activityReport = ApplyTeamIdFilter(activityReport, employeeIdParameter);

            return activityReport.ToList();
        }

        private IQueryable<PtoRequestReport> ApplyEmployeeIdFilter(IQueryable<PtoRequestReport> query, int employeeId)
        {
            if(employeeId <= 0)
            {
                return query;
            }
            return query.Where(reportRow => reportRow.EmployeeId == employeeId);
        }        
        private IQueryable<PtoRequestReport> ApplyTeamIdFilter(IQueryable<PtoRequestReport> query, int teamId)
        {
            if (teamId <= 0)
            {
                return query;
            }
            return query.Where(reportRow => reportRow.TeamId == teamId);
        }

        private IQueryable<PtoRequestReport> ApplyDateRangeFilter(IQueryable<PtoRequestReport> query, Dictionary<string, DateTime> dateParameters)
        {
            bool isDateParameterUsed = dateParameters.ContainsKey(Parameters.TO_REQUEST_REPORT_INITAL_DATE_PARAMETER) &&
                dateParameters.ContainsKey(Parameters.TO_REQUEST_REPORT_FINAL_DATE_PARAMETER);

            if (!isDateParameterUsed)
            {
                return query;
            }
            DateTime startedDateParameter = dateParameters[Parameters.TO_REQUEST_REPORT_INITAL_DATE_PARAMETER];
            DateTime finishedDateParameter = dateParameters[Parameters.TO_REQUEST_REPORT_FINAL_DATE_PARAMETER];

            return query.Where(reportRow => !(startedDateParameter < reportRow.StartedDate && finishedDateParameter < reportRow.StartedDate ||
                            startedDateParameter > reportRow.FinishedDate && finishedDateParameter > reportRow.FinishedDate));
        }
    }
}
