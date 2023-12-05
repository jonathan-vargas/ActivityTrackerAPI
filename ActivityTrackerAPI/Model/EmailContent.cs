namespace ActivityTrackerAPI.Model;

public class EmailContent
{
    public string From { get; set; } = default!;
    public string To { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string Body { get; set; } = default!;
}
