using AutoMapper;
using EXE201_3CBilliard_Model.Models.Request;

using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Repository.Entities;
using EXE201_3CBilliard_Repository.Repository;
using EXE201_3CBilliard_Repository.Tools;
using EXE201_3CBilliard_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;

namespace EXE201_3CBilliard_Service.Service
{
    public class BidaClubService : IBidaClubService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly EXE201_3CBilliard_Repository.Tools.Firebase _firebase;


        public BidaClubService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, EXE201_3CBilliard_Repository.Tools.Firebase firebase)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
            _firebase = firebase;
        }

        public async Task<BidaClubReponse> CreateBidaClubAsync(BidaClubRequest request)
        {
            var bida = _unitOfWork.BidaClubRepository.Get(filter: b => b.BidaName == request.BidaName && b.Status != BidaClubStatus.INACTIVE).FirstOrDefault();
            if (bida != null)
            {
                throw new KeyNotFoundException($"BidaClub with Name is dulicated");
            }
            var bidaClub = _mapper.Map<BidaClub>(request);
            bidaClub.Descrpition = request.Description;
            bidaClub.CreateAt = DateTime.Now; // Set CreateAt time here
            bidaClub.Status = BidaClubStatus.WAITING; // Set status to WAITING
            bidaClub.Note = "Waiting for confirm !!";
            
            /*bidaClub.OpeningHours = null;*/


            if (request.Image != null)
            {
                if (request.Image.Length >= 10 * 1024 * 1024)
                {
                    throw new Exception();
                }
                string imageDownloadUrl = await _firebase.UploadImage(request.Image);
                bidaClub.Image = imageDownloadUrl;
            }

            //Change role user
            var user = _unitOfWork.UserRepository.GetById(request.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {request.UserId} not found");
            }

            // Update user's role to Bida Owner (role ID 4)
            user.RoleId = 4;
            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.BidaClubRepository.Insert(bidaClub);
            _unitOfWork.Save();


            var notification = new Notificate
            {
                Title = "Câu lạc bộ Bida được tạo",
                Descrpition = $"Câu lạc bộ bida của bạn '{bidaClub.BidaName}' đã được tạo và đang chờ xác nhận.",
                CreateAt = DateTime.Now,
                Status = NotificateStatus.ACTIVE,
                UserId = request.UserId,
                Type = NotificationType.ClubNotification,
            };
            _unitOfWork.NotificateRepository.Insert(notification);
            _unitOfWork.Save();


            /*// Gửi thông báo Firebase
            var message = new Message()
            {
                Notification = new Notification
                {
                    Title = "Câu lạc bộ Bida được tạo",
                    Body = $"Câu lạc bộ bida của bạn '{bidaClub.BidaName}' đã được tạo và đang chờ xác nhận."
                },
                Topic = "bidaClubUpdates"
            };

            await FirebaseMessaging.DefaultInstance.SendAsync(message);*/

            return _mapper.Map<BidaClubReponse>(bidaClub);
        }

        public async Task DeleteBidaClubAsync(long id)
        {
            var bidaClub = _unitOfWork.BidaClubRepository.GetById(id);
            if (bidaClub == null)
            {
                throw new KeyNotFoundException($"BidaClub with ID {id} not found");
            }

            bidaClub.Status = BidaClubStatus.INACTIVE; // Set status to INACTIVE instead of deleting
            _unitOfWork.BidaClubRepository.Update(bidaClub);
            _unitOfWork.Save();

            var notification = new Notificate
            {
                Title = "Câu lạc bộ Bida bị xóa",
                Descrpition = $"Câu lạc bộ bida của bạn '{bidaClub.BidaName}' đã bị xóa.",
                CreateAt = DateTime.Now,
                Status = NotificateStatus.ACTIVE,
                UserId = bidaClub.UserId,
                Type = NotificationType.ClubNotification
            };
            _unitOfWork.NotificateRepository.Insert(notification);
            _unitOfWork.Save();
        }

        public async Task<IEnumerable<BidaClubReponse>> GetAllBidaClubsAsync()
        {
            var bidaClubs = _unitOfWork.BidaClubRepository.Get();
            return _mapper.Map<IEnumerable<BidaClubReponse>>(bidaClubs);
        }

        public async Task<BidaClubReponse> GetBidaClubByIdAsync(long id)
        {
            var bidaClub = _unitOfWork.BidaClubRepository.GetById(id);
            if (bidaClub == null)
            {
                return null;
            }

            return _mapper.Map<BidaClubReponse>(bidaClub);
        }

        public async Task<BidaClubReponse> UpdateBidaClubAsync(long id, BidaClubRequest request)
        {
            var bida = _unitOfWork.BidaClubRepository.Get(filter: b => b.BidaName == request.BidaName && b.Status == BidaClubStatus.ACTIVE).FirstOrDefault();
            if (bida != null)
            {
                throw new Exception($"BidaClub with Name is dulicated");
            }
            var bidaClub = _unitOfWork.BidaClubRepository.GetById(id);
            if (bidaClub == null)
            {
                throw new KeyNotFoundException($"BidaClub with ID {id} not found");
            }
            if (bidaClub.Status != BidaClubStatus.ACTIVE)
            {
                throw new InvalidOperationException($"BidaClub with ID {id} is not in ACTIVE status");
            }
            _mapper.Map(request, bidaClub);
            if (request.Image != null)
            {
                if (request.Image.Length >= 10 * 1024 * 1024)
                {
                    throw new Exception();
                }
                string imageDownloadUrl = await _firebase.UploadImage(request.Image);
                bidaClub.Image = imageDownloadUrl;
            }

            


            _unitOfWork.BidaClubRepository.Update(bidaClub);
            _unitOfWork.Save();

            return _mapper.Map<BidaClubReponse>(bidaClub);
        }

        public async Task<BidaClubReponse> ActivateBidaClubAsync(long id, NoteRequest noteRequest)
        {
            var bidaClub = _unitOfWork.BidaClubRepository.GetById(id);
            if (bidaClub == null)
            {
                throw new KeyNotFoundException($"BidaClub with ID {id} not found");
            }
            if (bidaClub.Status != BidaClubStatus.WAITING)
            {
                throw new InvalidOperationException($"BidaClub with ID {id} is not in WAITING status");
            }

            bidaClub.Status = BidaClubStatus.ACTIVE;
            bidaClub.Note = noteRequest.Note;
            _unitOfWork.BidaClubRepository.Update(bidaClub);
            _unitOfWork.Save();


            //Notìicate 
            var notification = new Notificate
            {
                Title = "Câu lạc bộ Bida được kích hoạt",
                Descrpition = $"Câu lạc bộ bida của bạn '{bidaClub.BidaName}' đã được kích hoạt.",
                CreateAt = DateTime.Now,
                Status = NotificateStatus.ACTIVE,
                UserId = bidaClub.UserId,
                Type = NotificationType.ClubNotification
            };
            _unitOfWork.NotificateRepository.Insert(notification);
            _unitOfWork.Save();

            // Send email notification with detailed information
            var emailSubject = "Your BidaClub has been activated!";
            var emailMessage = $@"
                Dear {bidaClub.BidaName},<br><br>
                Your BidaClub has been successfully activated.<br><br>
                Here are the details of your BidaClub:<br>
                <strong>Name:</strong> {bidaClub.BidaName}<br>
                <strong>Address:</strong> {bidaClub.Address}<br>
                <strong>Email:</strong> {bidaClub.Email}<br>
                <strong>Description:</strong> {bidaClub.Descrpition}<br>
                <strong>Phone:</strong> {bidaClub.Phone}<br>
                <strong>Average Price:</strong> {bidaClub.AveragePrice:C}<br>
                <br>
                If you have any questions or need further assistance, please do not hesitate to contact us.<br><br>
                Best regards,<br>
                3CBilliard Team
                ";

            await _emailService.SendEmailAsync(bidaClub.Email, emailSubject, emailMessage);

            return _mapper.Map<BidaClubReponse>(bidaClub);
        }


        public async Task<BidaClubReponse> RejectBidaClubAsync(long id, NoteRequest noteRequest)
        {
            var bidaClub = _unitOfWork.BidaClubRepository.GetById(id);
            if (bidaClub == null)
            {
                throw new KeyNotFoundException($"BidaClub with ID {id} not found");
            }

            if (bidaClub.Status != BidaClubStatus.WAITING)
            {
                throw new InvalidOperationException($"BidaClub with ID {id} is not in WAITING status");
            }

            bidaClub.Status = BidaClubStatus.DELETED;
            bidaClub.Note = noteRequest.Note; // Update note based on NoteRequest
            _unitOfWork.BidaClubRepository.Update(bidaClub);
            _unitOfWork.Save();


            // Tạo thông báo
            var notification = new Notificate
            {
                Title = "Câu lạc bộ Bida bị từ chối",
                Descrpition = $"Câu lạc bộ bida của bạn '{bidaClub.BidaName}' đã bị từ chối. Lý do: {noteRequest.Note}",
                CreateAt = DateTime.Now,
                Status = NotificateStatus.ACTIVE,
                UserId = bidaClub.UserId,
                Type = NotificationType.ClubNotification
            };
            _unitOfWork.NotificateRepository.Insert(notification);
            _unitOfWork.Save();


            // Send email notification with rejection details
            var emailSubject = "Your BidaClub registration has been rejected";
            var emailMessage = $@"
                Dear {bidaClub.BidaName},<br><br>
                We regret to inform you that your BidaClub registration has been rejected.<br><br>
                <strong>Reason:</strong> {noteRequest.Note}<br><br>
                If you have any questions or need further assistance, please do not hesitate to contact us.<br><br>
                Best regards,<br>
                3CBilliard Team
                ";

            await _emailService.SendEmailAsync(bidaClub.Email, emailSubject, emailMessage);

            return _mapper.Map<BidaClubReponse>(bidaClub);
        }



        //Search
        public async Task<(IEnumerable<BidaClubReponse> bidaClubs, int totalCount)> SearchBidaClubsAsync(string? bidaName, long? userId, string? address, string? status, int pageIndex, int pageSize)
        {
            // Khai báo biến statusEnum để lưu trữ giá trị enum BookingStatus sau khi chuyển đổi
            BidaClubStatus? statusEnum = null;

            // Thử chuyển đổi chuỗi status thành giá trị enum BookingStatus
            if (!string.IsNullOrEmpty(status) && Enum.TryParse(status, true, out BidaClubStatus parsedStatus))
            {
                statusEnum = parsedStatus;
            }

            var bidaClubsWithCount = _unitOfWork.BidaClubRepository.GetWithCount(
                filter: x =>
                    (string.IsNullOrEmpty(bidaName) || x.BidaName.Contains(bidaName)) &&
                    (string.IsNullOrEmpty(address) || x.Address.Contains(address)) &&
                    (!userId.HasValue || x.UserId == userId.Value) &&
                    (!statusEnum.HasValue || x.Status == statusEnum.Value),
                pageIndex: pageIndex,
                pageSize: pageSize
            );

            var bidaClubs = bidaClubsWithCount.items;
            var totalCount = bidaClubsWithCount.totalCount;

            var bidaClubResponses = _mapper.Map<IEnumerable<BidaClubReponse>>(bidaClubs);
            return (bidaClubResponses, totalCount);
        }


    }
}
