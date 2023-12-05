using ActivityTrackerAPI.Model;

namespace ActivityTrackerAPI.Utility;

public interface IAppMailSender
{
    void SendEmail(EmailConfiguration emailConfiguration, EmailContent email, ILogger<PtoRequest> logger);
}
