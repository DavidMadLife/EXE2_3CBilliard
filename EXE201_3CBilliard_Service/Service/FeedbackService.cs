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
    public class FeedbackService :IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FeedbackResponse>> GetAllFeedbackAsync()
        {
            var feedbacks = _unitOfWork.FeedbackRepository.Get();
            return _mapper.Map<IEnumerable<FeedbackResponse>>(feedbacks);
        }

        public async Task<FeedbackResponse> GetFeedbackByIdAsync(long id)
        {
            var feedback = _unitOfWork.FeedbackRepository.GetById(id);
            return _mapper.Map<FeedbackResponse>(feedback);
        }

        public async Task<FeedbackResponse> CreateFeedbackAsync(FeedbackRequset request)
        {
            var feedback = _mapper.Map<Feedback>(request);
            _unitOfWork.FeedbackRepository.Insert(feedback);
            _unitOfWork.Save();
            return _mapper.Map<FeedbackResponse>(feedback);
        }

        public async Task<FeedbackResponse> UpdateFeedbackAsync(long id, FeedbackRequset request)
        {
            var feedback = _unitOfWork.FeedbackRepository.GetById(id);
            if (feedback == null)
                throw new Exception($"Feedback with id {id} not found.");

            _mapper.Map(request, feedback);
            _unitOfWork.FeedbackRepository.Update(feedback);
            _unitOfWork.Save();
            return _mapper.Map<FeedbackResponse>(feedback);
        }

        public async Task<bool> DeleteFeedbackAsync(long id)
        {
            var feedback = _unitOfWork.FeedbackRepository.GetById(id);
            if (feedback == null)
                throw new Exception($"Feedback with id {id} not found.");

            _unitOfWork.FeedbackRepository.Delete(feedback);
            _unitOfWork.Save();
            return true;
        }
    }
}
