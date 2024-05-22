using EXE201_3CBilliard_Service.Interface;
using EXE201_3CBilliard_Service.Service;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly OtpManager _otpManager;

    public EmailService(IConfiguration configuration, OtpManager otpManager)
    {
        _configuration = configuration;
        _otpManager = otpManager;
    }

    public async Task SendOtpEmailAsync(string toEmail)
    {
        var otp = _otpManager.GenerateOtp(toEmail);
        var subject = "Your OTP Code";
        var message = $"Your OTP code is: {otp}. It is valid for 3 minutes.";

        await SendEmailAsync(toEmail, subject, message);
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpServer"])
        {
            Port = int.Parse(_configuration["EmailSettings:SmtpPort"]),
            Credentials = new NetworkCredential(_configuration["EmailSettings:SmtpUsername"], _configuration["EmailSettings:SmtpPassword"]),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_configuration["EmailSettings:FromEmail"]),
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
}
