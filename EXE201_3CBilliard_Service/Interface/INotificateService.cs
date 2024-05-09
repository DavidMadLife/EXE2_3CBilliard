using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Service.Interface
{
    public interface INotificateService
    {
        Task<IEnumerable<NotificateResponse>> GetAllNotificateAsync();
        Task<NotificateResponse> GetNotificateByIdAsync(long id);
        Task<NotificateResponse> CreateNotificateAsync(NotificateRequest request);
        Task<NotificateResponse> UpdateNotificateAsync(long id, NotificateRequest request);
        Task<bool> DeleteNotificateAsync(long id);
    }
}
