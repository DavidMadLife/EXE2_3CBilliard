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
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PostResponse>> GetAllPostsAsync()
        {
            var posts = _unitOfWork.PostRepository.Get();
            return _mapper.Map<IEnumerable<PostResponse>>(posts);
        }

        public async Task<PostResponse> GetPostByIdAsync(long id)
        {
            var post = _unitOfWork.PostRepository.GetById(id);
            return _mapper.Map<PostResponse>(post);
        }

        public async Task<PostResponse> CreatePostAsync(PostRequest request)
        {
            var post = _mapper.Map<Post>(request);
            _unitOfWork.PostRepository.Insert(post);
            _unitOfWork.Save();
            return _mapper.Map<PostResponse>(post);
        }

        public async Task<PostResponse> UpdatePostAsync(long id, PostRequest request)
        {
            var post = _unitOfWork.PostRepository.GetById(id);
            if (post == null)
                throw new Exception($"Post with id {id} not found.");

            _mapper.Map(request, post);
            _unitOfWork.PostRepository.Update(post);
            _unitOfWork.Save();
            return _mapper.Map<PostResponse>(post);
        }

        public async Task<bool> DeletePostAsync(long id)
        {
            var post = _unitOfWork.PostRepository.GetById(id);
            if (post == null)
                throw new Exception($"Post with id {id} not found.");

            _unitOfWork.PostRepository.Delete(post);
            _unitOfWork.Save();
            return true;
        }
    }
}
