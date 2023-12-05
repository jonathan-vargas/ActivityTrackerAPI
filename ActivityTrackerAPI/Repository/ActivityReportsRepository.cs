using ActivityTrackerAPI.Data;
using ActivityTrackerAPI.Model;
using ActivityTrackerAPI.Utility;
using ActivityTrackerAPI.Validation;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace ActivityTrackerAPI.Repository
{
    public class ActivityReportsRepository : IActivityReportRepository
    {
        private readonly AppDbContext _appDbContext;
        public ActivityReportsRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public List<ActivityReport>? GetActivityReport(Dictionary<string, DateTime> dateParameters, int employeeIdParameter, int teamIdParameter)
        {
            var activityReport = from activity in _appDbContext.Activity
                                 join employee in _appDbContext.Employee on activity.EmployeeId equals employee.EmployeeId
                                 join team in _appDbContext.Team on employee.TeamId equals team.TeamId
                                 select new ActivityReport
                                 {
                                     StartedDate = activity.StartedDate,
                                     FinishedDate = activity.FinishedDate,
                                     Description = activity.Description,
                                     Duration = EF.Functions.DateDiffDay(activity.StartedDate, activity.FinishedDate) + 1,
                                     EmployeeId = employee.EmployeeId,
                                     TeamId = team.TeamId
                                 };

            activityReport = ApplyDateRangeFilter(activityReport, dateParameters);
            activityReport = ApplyEmployeeIdFilter(activityReport, employeeIdParameter);
            activityReport = ApplyTeamIdFilter(activityReport, teamIdParameter);

            return activityReport.ToList();
        }

        private IQueryable<ActivityReport> ApplyEmployeeIdFilter(IQueryable<ActivityReport> query, int employeeId)
        {
            if(employeeId <= 0)
            {
                return query;
            }
            return query.Where(reportRow => reportRow.EmployeeId == employeeId);
        }        
        private IQueryable<ActivityReport> ApplyTeamIdFilter(IQueryable<ActivityReport> query, int teamId)
        {
            if (teamId <= 0)
            {
                return query;
            }
            return query.Where(reportRow => reportRow.TeamId == teamId);
        }

        private IQueryable<ActivityReport> ApplyDateRangeFilter(IQueryable<ActivityReport> query, Dictionary<string, DateTime> dateParameters)
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
