using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Service.Interface
{
    public interface IEmailService
    {
        Task SendOtpEmailAsync(string toEmail);
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}
