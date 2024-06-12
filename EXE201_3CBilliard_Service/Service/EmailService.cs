using EXE201_3CBilliard_Service.Interface;
using EXE201_3CBilliard_Service.Service;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using EXE201_3CBilliard_Repository.Repository;
using EXE201_3CBilliard_Model.Models.Response;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly OtpManager _otpManager;
    private readonly IUnitOfWork _unitOfWork;

    public EmailService(IConfiguration configuration, OtpManager otpManager, IUnitOfWork unitOfWork)
    {
        _configuration = configuration;
        _otpManager = otpManager;
        _unitOfWork = unitOfWork;
    }

    public async Task SendOtpEmailAsync(string toEmail)
    {
        var user = _unitOfWork.UserRepository.Get(filter: m => m.Email == toEmail).FirstOrDefault();
        if (user == null)
        {
            throw new Exception($"Email not found.");
        }
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

    public async Task SendBillEmailAsync(string toEmail, BillResponse billResponse)
    {
        var subject = "Your Bill Information";

        // Tạo danh sách các Slot đã đặt
        var bookedSlotInfo = new List<string>();
        long bidaTable = 0;
        foreach (var slotId in billResponse.BookedSlotIds)
        {
            var bidaTableSlot = _unitOfWork.BidaTableSlotRepository.GetById(slotId);
            if (bidaTableSlot != null)
            {
                bidaTable = bidaTableSlot.BidaTableId;

                bookedSlotInfo.Add($"Slot {bidaTableSlot.SlotId}");
            }
        }
        //tìm tableName
        var tableName = _unitOfWork.BidaTableRepository.GetById(bidaTable);
        long bidaClub = tableName.BidaCludId;
        //Tìm clubName
        var clubName = _unitOfWork.BidaClubRepository.GetById(bidaClub); 
        // Tạo message email với thông tin Bill và danh sách các Slot đã đặt
        var message = $@"
    <h1>Bill Information</h1>
    <p>Booker Name: {billResponse.BookerName}</p>
    <p>Booker Phone: {billResponse.BookerPhone}</p>
    <p>Booker Email: {billResponse.BookerEmail}</p>
    <p>Booked Slots: {string.Join(", ", bookedSlotInfo)}</p>
    <p>Table Name: {tableName.TableName}</p>
    <p>Club Name: {clubName.BidaName}</p>
    <p>Price: {billResponse.Price}</p>
    <p>CreateAt: {billResponse.CreateAt}</p>
    <p>Booking Date: {billResponse.BookingDate.Date}</p>
    <p>OrderCode: {billResponse.OrderCode}</p>
    <p>Description: {billResponse.Descrpition}</p>
    <p>Status: {billResponse.Status}</p>
    
    ";

        // Gửi email
        await SendEmailAsync(toEmail, subject, message);
    }


    public async Task SendRejectBookingEmailAsync(string toEmail)
    {
        try
        {
            var subject = "Your Booking Failed";
            var message = $@"
            <h1>Booking Failed</h1>
            <p>Sorry, your booking could not be completed at this time.</p>
            <p>Please contact support for further assistance.</p>
            <p>Best regards,<br>
               3CBilliard Team</p>";
            await SendEmailAsync(toEmail, subject, message);
        }
        catch (Exception ex)
        {
            // Log the exception for debugging purposes
            Console.WriteLine($"Error sending booking failure email: {ex.Message}");
            // Throw a custom exception or handle the error as appropriate for your application
            throw new Exception("Failed to send booking failure email.");
        }
    }


}
