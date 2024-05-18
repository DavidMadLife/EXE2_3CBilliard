using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Service.Interface
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentResponse>> GetAllCommentsAsync();
        Task<CommentResponse> GetCommentByIdAsync(long id);
        Task<CommentResponse> CreateCommentAsync(CommentRequest request);
        Task<CommentResponse> UpdateCommentAsync(long id, CommentRequest request);
        Task DeleteCommentAsync(long id);
    }
}
