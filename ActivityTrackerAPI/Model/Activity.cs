namespace ActivityTrackerAPI.Model;

public class Activity
{
    public int ActivityId { get; set; }

    public DateTime StartedDate { get; set; }

    public DateTime FinishedDate { get; set; }

    public string? Description { get; set; }
    public int EmployeeId { get; set; }
}