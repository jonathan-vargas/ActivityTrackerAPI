namespace ActivityTrackerAPI.Model;

public class Team
{
    public int TeamId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int TeamLeadEmployeeId { get; set; }
}