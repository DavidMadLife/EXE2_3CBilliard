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

        public async Task<IEnumerable<CommentResponse>> GetAllCommentsAsync()
        {
            var comments = _unitOfWork.CommentRepository.Get();
            return _mapper.Map<IEnumerable<CommentResponse>>(comments);
        }

        public async Task<CommentResponse> GetCommentByIdAsync(long id)
        {
            var comment = _unitOfWork.CommentRepository.GetById(id);
            return _mapper.Map<CommentResponse>(comment);
        }

        public async Task<CommentResponse> CreateCommentAsync(CommentRequest request)
        {
            var comment = _mapper.Map<Comment>(request);
            _unitOfWork.CommentRepository.Insert(comment);
            _unitOfWork.Save();
            return _mapper.Map<CommentResponse>(comment);
        }

        public async Task<CommentResponse> UpdateCommentAsync(long id, CommentRequest request)
        {
            var comment = _unitOfWork.CommentRepository.GetById(id);
            if (comment == null)
                throw new Exception($"Comment with id {id} not found.");

            _mapper.Map(request, comment);
            _unitOfWork.CommentRepository.Update(comment);
            _unitOfWork.Save();
            return _mapper.Map<CommentResponse>(comment);
        }

        public async Task<bool> DeleteCommentAsync(long id)
        {
            var comment = _unitOfWork.CommentRepository.GetById(id);
            if (comment == null)
                throw new Exception($"Comment with id {id} not found.");

            _unitOfWork.CommentRepository.Delete(comment);
            _unitOfWork.Save();
            return true;
        }
    }
}
