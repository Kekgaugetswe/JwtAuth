using System;
using System.Net;
using System.Net.Mail;
using JwtAuth.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace JwtAuth.Infrastructure.Services;

public class EmailSender(IConfiguration configuration) : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var smtpClient = new SmtpClient(configuration["SMTP:Host"])
        {
            Port = int.Parse(configuration["SMTP:Port"]),
            Credentials = new NetworkCredential(configuration["SMTP:Username"], configuration["SMTP:Password"]),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(configuration["SMTP:From"]),
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(email);

        await smtpClient.SendMailAsync(mailMessage);
    }
}
