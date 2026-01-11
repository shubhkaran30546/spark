
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;

public class EmailSender : IEmailSender
{
    private readonly string _smtpServer = "your_smtp_server";
    private readonly int _smtpPort = 587; // Use your SMTP port
    private readonly string _smtpUser = "your_email@example.com";
    private readonly string _smtpPass = "your_email_password";

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_smtpUser),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true
        };

        mailMessage.To.Add(email);

        using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
        {
            smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
            smtpClient.EnableSsl = true;
            return smtpClient.SendMailAsync(mailMessage);
        }
    }
}
