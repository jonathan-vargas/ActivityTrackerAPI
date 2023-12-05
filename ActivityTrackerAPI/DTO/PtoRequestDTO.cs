namespace ActivityTrackerAPI.DTO;

public class PtoRequestDTO : DtoBase
{
    public int PtoRequestId { get; set; }
    public DateTime StartedDate { get; set; }
    public DateTime FinishedDate { get; set; }
    public int PtoStatusId { get; set; }
    public int EmployeeId { get; set; }
    public int TeamLeadEmployeeId { get; set; }
}
