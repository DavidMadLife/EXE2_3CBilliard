using AutoMapper;
using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Repository.Entities;
using EXE201_3CBilliard_Repository.Repository;
using EXE201_3CBilliard_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Service.Service
{
    public class NotificateService : INotificateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificateService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NotificateResponse>> GetAllNotificateAsync()
        {
            var notificates = _unitOfWork.NotificateRepository.Get();
            return _mapper.Map<IEnumerable<NotificateResponse>>(notificates);
        }

        public async Task<NotificateResponse> GetNotificateByIdAsync(long id)
        {
            var notificate = _unitOfWork.NotificateRepository.GetById(id);
            return _mapper.Map<NotificateResponse>(notificate);
        }


        public async Task DeleteNotificateAsync(long id)
        {
            var notificate = _unitOfWork.NotificateRepository.GetById(id);
            if (notificate == null)
                throw new Exception($"Notificate with id {id} not found.");
            notificate.Status = NotificateStatus.INACTIVE;
            _unitOfWork.NotificateRepository.Update(notificate);
            _unitOfWork.Save();
        }

        public async Task<(IEnumerable<NotificateResponse> notificates, int totalCount)> SearchNotificatesAsync(string? title, string? description, NotificateStatus? status, long? userId, NotificationType? type, string? billOrderCode, string? billStatus, int pageIndex, int pageSize)
        {
            var notificatesWithCount = _unitOfWork.NotificateRepository.GetWithCount(
                filter: x =>
                    (string.IsNullOrEmpty(title) || x.Title.Contains(title)) &&
                    (string.IsNullOrEmpty(description) || x.Descrpition.Contains(description)) &&
                    (!status.HasValue || x.Status == status) &&
                    (!userId.HasValue || x.UserId == userId.Value) &&
                    (!type.HasValue || x.Type == type) &&
                    (string.IsNullOrEmpty(billOrderCode) || x.BillOrderCode.Contains(billOrderCode)) &&
                    (string.IsNullOrEmpty(billStatus) || x.BillStatus == billStatus),
                pageIndex: pageIndex,
                pageSize: pageSize
            );

            var notificates = notificatesWithCount.items;
            var totalCount = notificatesWithCount.totalCount;

            var notificateResponses = _mapper.Map<IEnumerable<NotificateResponse>>(notificates);
            return (notificateResponses, totalCount);
        }



        public async Task SendNotificationAsync(Notificate notification)
        {
            _unitOfWork.NotificateRepository.Insert(notification);
            _unitOfWork.Save();
        }

        public async Task<NotificateResponse> CreateNotificateAsync(NotificateRequest request)
        {
            var noti = _mapper.Map<Notificate>(request);
            noti.Status = NotificateStatus.ACTIVE;
            noti.CreateAt = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
            _unitOfWork.NotificateRepository.Insert(noti);
            _unitOfWork.Save();
            return _mapper.Map<NotificateResponse>(noti);
        }
    }
}
