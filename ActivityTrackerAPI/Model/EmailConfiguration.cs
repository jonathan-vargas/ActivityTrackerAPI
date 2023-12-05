namespace ActivityTrackerAPI.Model;

public class EmailConfiguration
{
    public string SmtpServer { get; set; } = default!;
    public int SmtpPort { get; set; }
    public string SmtpUsername { get; set; } = default!;
    public string SmtpPassword { get; set; } = default!;
}
