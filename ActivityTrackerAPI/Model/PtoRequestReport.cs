namespace ActivityTrackerAPI.Model;

public class PtoRequestReport : ModelBase
{
    public DateTime StartedDate { get; set; }
    public DateTime FinishedDate { get; set; }
    public string Description { get; set; } = default!;
    public int Duration { get; set; }
    public int EmployeeId { get; set; }
    public int TeamId { get; set; }
}
