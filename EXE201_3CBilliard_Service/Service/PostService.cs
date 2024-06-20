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

        public async Task<PostResponse> CreatePostAsync(PostRequest postRequest)
        {
            var postEntity = _mapper.Map<PostRequest, Post>(postRequest);
            postEntity.CreatedAt = DateTime.Now;

            _unitOfWork.PostRepository.Insert(postEntity);
            _unitOfWork.Save();

            var postResponse = _mapper.Map<Post, PostResponse>(postEntity);
            return postResponse;
        }

        public async Task<PostResponse> GetPostByIdAsync(long postId)
        {
            var postEntity = _unitOfWork.PostRepository.GetById(postId);
            var postResponse = _mapper.Map<Post, PostResponse>(postEntity);
            return postResponse;
        }

        public async Task<IEnumerable<PostResponse>> GetPostsAsync()
        {
            var posts = _unitOfWork.PostRepository.Get();
            var postResponses = _mapper.Map<IEnumerable<Post>, IEnumerable<PostResponse>>(posts);
            return postResponses;
        }

        public async Task<PostResponse> UpdatePostAsync(long postId, PostRequest postRequest)
        {
            var existingPost = _unitOfWork.PostRepository.GetById(postId);
            if (existingPost == null)
            {
                // Handle not found scenario
                return null;
            }

            _mapper.Map(postRequest, existingPost);
            existingPost.CreatedAt = DateTime.Now;

            _unitOfWork.PostRepository.Update(existingPost);
            _unitOfWork.Save();

            var updatedPostResponse = _mapper.Map<Post, PostResponse>(existingPost);
            return updatedPostResponse;
        }

        public async Task<bool> DeletePostAsync(long postId)
        {
            var postToDelete = _unitOfWork.PostRepository.GetById(postId);
            if (postToDelete == null)
            {
                // Handle not found scenario
                return false;
            }

            _unitOfWork.PostRepository.Delete(postToDelete);
            _unitOfWork.Save();
            return true;
        }
    }
}
