using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Respone;
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
        Task<IEnumerable<PostResponse>> GetAllPostsAsync();
        Task<PostResponse> GetPostByIdAsync(long id);
        Task<PostResponse> CreatePostAsync(PostRequest request);
        Task<PostResponse> UpdatePostAsync(long id, PostRequest request);
        Task<PostResponse> ActivatePostAsync(long id, NoteRequest noteRequest);
        Task<PostResponse> RejectPostAsync(long id, NoteRequest noteRequest);
        Task DeletePostAsync(long id);
    }
}
