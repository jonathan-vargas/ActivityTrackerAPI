namespace ActivityTrackerAPI.Model;

public class PtoRequest
{
    public int PtoRequestId { get; set; }
    public DateTime StartedDate { get; set; }
    public DateTime FinishedDate { get; set; }
    public int PtoStatusId { get; set; }
    public int EmployeeId { get; set; }
    public int TeamLeadEmployeeId { get; set; }
}