using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Repository.Entities;
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
        /*Task<NotificateResponse> CreateNotificateAsync(NotificateRequest request);
        Task<NotificateResponse> UpdateNotificateAsync(long id, NotificateRequest request);*/
        Task DeleteNotificateAsync(long id);
        Task<(IEnumerable<NotificateResponse> notificates, int totalCount)> SearchNotificatesAsync(string? title, string? description, NotificateStatus? status, long? userId, NotificationType? type, string? billOrderCode, string? billStatus, int pageIndex, int pageSize);

        Task SendNotificationAsync(Notificate notification);


        Task<NotificateResponse> CreateNotificateAsync(NotificateRequest request);
    }
}
