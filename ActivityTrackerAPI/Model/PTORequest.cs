namespace ActivityTrackerAPI.Model;

public class PTORequest
{
    public int PTORequestId { get; set; }
    public DateOnly StartedDate { get; set; }
    public DateOnly FinishedDate { get; set; }
    public int PTOStatusId { get; set; }
    public int EmployeeId { get; set; }
    public int TeamLeadEmployeeId { get; set; }
}