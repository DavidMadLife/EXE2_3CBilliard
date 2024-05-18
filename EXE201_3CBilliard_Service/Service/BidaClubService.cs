using AutoMapper;
using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Respone;
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
    public class BidaClubService : IBidaClubService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BidaClubService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BidaClubReponse> CreateBidaClubAsync(BidaClubRequest request)
        {
            var bida = _unitOfWork.BidaClubRepository.Get(filter: b => b.BidaName == request.BidaName).FirstOrDefault();
            if (bida != null)
            {
                throw new KeyNotFoundException($"BidaClub with Name is dulicated");
            }
            var entity = _mapper.Map<BidaClub>(request);
            entity.Descrpition = request.Description;
            entity.CreateAt = DateTime.Now; // Set CreateAt time here
            entity.Status = BidaClubStatus.WAITING; // Set status to WAITING

            _unitOfWork.BidaClubRepository.Insert(entity);
            _unitOfWork.Save();

            return _mapper.Map<BidaClubReponse>(entity);
        }

        public async Task DeleteBidaClubAsync(long id)
        {
            var bidaClub = _unitOfWork.BidaClubRepository.GetById(id);
            if (bidaClub == null)
            {
                throw new KeyNotFoundException($"BidaClub with ID {id} not found");
            }

            bidaClub.Status = BidaClubStatus.DELETED; // Set status to DELETED instead of deleting
            _unitOfWork.BidaClubRepository.Update(bidaClub);
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
            var bidaClub = _unitOfWork.BidaClubRepository.GetById(id);
            if (bidaClub == null)
            {
                throw new KeyNotFoundException($"BidaClub with ID {id} not found");
            }

            _mapper.Map(request, bidaClub);
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
            bidaClub.Note = noteRequest.Note; // Update note based on NoteRequest
            _unitOfWork.BidaClubRepository.Update(bidaClub);
            _unitOfWork.Save();

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

            return _mapper.Map<BidaClubReponse>(bidaClub);
        }
    }
}
