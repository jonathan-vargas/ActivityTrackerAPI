﻿using ActivityTrackerAPI.Model;
using System.Net.Mail;
using System.Net;
using Humanizer;

namespace ActivityTrackerAPI.Utility;

public static class AppMailSender
{
    public static async void SendEmail(EmailConfiguration emailConfiguration, EmailContent email, ILogger<PtoRequest> logger)
    {
        if (emailConfiguration == null)
        {
            logger.LogError("EmailConfiguration object is null");
            return;
        }

        if (email == null)
        {
            logger.LogError("EmailContent object is null");
            return;
        }

        try
        {
            using (SmtpClient smtpClient = new SmtpClient(emailConfiguration.SmtpServer, emailConfiguration.SmtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(emailConfiguration.SmtpUsername, emailConfiguration.SmtpPassword);
                smtpClient.EnableSsl = true;

                MailMessage mailMessage = new()
                {
                    From = new MailAddress(email.From),
                    Subject = email.Subject,
                    Body = email.Body,
                    IsBodyHtml = true,
                    
                };

                mailMessage.To.Add(email.To);
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email with the following EmailConfiguration: "+emailConfiguration.ToString());            
        }
    }
}
