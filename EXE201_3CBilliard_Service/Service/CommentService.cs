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
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CommentResponse> CreateCommentAsync(CommentRequest commentRequest)
        {
            //check post existed
            var post = _unitOfWork.PostRepository.GetById(commentRequest.PostId);
            if (post == null)
            {
                throw new KeyNotFoundException($"Post with ID {commentRequest.PostId} not found");
            }
            //check user existed
            var user = _unitOfWork.UserRepository.GetById(commentRequest.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {commentRequest.UserId} not found");
            }

            var commentEntity = _mapper.Map<CommentRequest, Comment>(commentRequest);
            commentEntity.CreatedAt = DateTime.Now;

            _unitOfWork.CommentRepository.Insert(commentEntity);
            _unitOfWork.Save();

            var commentResponse = _mapper.Map<Comment, CommentResponse>(commentEntity);
            commentResponse.UserName = user.UserName;
            return commentResponse;
        }

        public async Task<IEnumerable<CommentResponse>> GetCommentsForPostAsync(long postId)
        {
            //check post existed
            var post = _unitOfWork.PostRepository.GetById(postId);
            if (post == null)
            {
                throw new KeyNotFoundException($"Post with ID {postId} not found");
            }

            var comments = _unitOfWork.CommentRepository.Get(filter: c => c.PostId == postId);
            var commentResponses = _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentResponse>>(comments);
            foreach (var commentResponse in commentResponses)
            {
                var cmt = _unitOfWork.CommentRepository.GetById(commentResponse.Id);
                var userCommnet = _unitOfWork.UserRepository.GetById(cmt.UserId);
                commentResponse.UserName = userCommnet.UserName;
            }
            return commentResponses;
        }

        public async Task<bool> DeleteCommentAsync(long commentId)
        {
            var cmtToDelete = _unitOfWork.CommentRepository.GetById(commentId);
            if (cmtToDelete == null)
            {
                // Handle not found scenario
                throw new KeyNotFoundException($"Comment with ID {commentId} not found");
            }

            _unitOfWork.CommentRepository.Delete(cmtToDelete);
            _unitOfWork.Save();
            return true;
        }
    }
}
