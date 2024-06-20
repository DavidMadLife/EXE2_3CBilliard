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
        Task<CommentResponse> CreateCommentAsync(CommentRequest commentRequest);
        Task<IEnumerable<CommentResponse>> GetCommentsForPostAsync(long postId);
        Task<bool> DeleteCommentAsync(long commentId);
    }
}
