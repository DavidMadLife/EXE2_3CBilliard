using EXE201_3CBilliard_Model.Models.Request;

using EXE201_3CBilliard_Model.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Service.Interface
{
    public interface IPostService
    {
        Task<PostResponse> CreatePostAsync(PostRequest postRequest);
        Task<PostResponse> GetPostByIdAsync(long postId);
        Task<IEnumerable<PostResponse>> GetPostsAsync();
        Task<PostResponse> UpdatePostAsync(long postId, PostRequest postRequest);
        Task<bool> DeletePostAsync(long postId);
    }
}
