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
    public class LikeService : ILikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LikeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LikeResponse> LikePostAsync(LikeRequest likeRequest)
        {
            //check post existed
            var post = _unitOfWork.PostRepository.GetById(likeRequest.PostId);
            if (post == null)
            {
                throw new KeyNotFoundException($"Post with ID {likeRequest.PostId} not found");
            }
            //check user existed
            var user = _unitOfWork.UserRepository.GetById(likeRequest.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {likeRequest.UserId} not found");
            }
            var likeEntity = _mapper.Map<LikeRequest, Like>(likeRequest);

            _unitOfWork.LikeRepository.Insert(likeEntity);
            _unitOfWork.Save();
            var likeResponse = _mapper.Map<Like, LikeResponse>(likeEntity);
            likeResponse.UserName = user.UserName;
            return likeResponse;
        }

        public async Task<IEnumerable<LikeResponse>> GetLikesForPostAsync(long postId)
        {
            //check post existed
            var post = _unitOfWork.PostRepository.GetById(postId);
            if (post == null)
            {
                throw new KeyNotFoundException($"Post with ID {postId} not found");
            }

            var likes = _unitOfWork.LikeRepository.Get(filter: l => l.PostId == postId);
            var likeResponses = _mapper.Map<IEnumerable<Like>, IEnumerable<LikeResponse>>(likes);
            foreach (var likeResponse in likeResponses)
            {
                var like = _unitOfWork.LikeRepository.GetById(likeResponse.Id);
                var userLike = _unitOfWork.UserRepository.GetById(like.UserId);
                likeResponse.UserName = userLike.UserName;
            }
            return likeResponses;
        }

        public async Task<bool> UnlikePostAsync(long likeId)
        {
            //check like existed
            var likeToDelete = _unitOfWork.LikeRepository.GetById(likeId);
            if (likeToDelete == null)
            {
                // Handle not found scenario
                throw new KeyNotFoundException($"Like with ID {likeId} not found");
            }

            _unitOfWork.LikeRepository.Delete(likeToDelete);
            _unitOfWork.Save();
            return true;
        }
    }
}
