using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Service.Interface
{
    public interface IFeedbackService
    {
        Task<IEnumerable<FeedbackResponse>> GetAllFeedbackAsync();
        Task<FeedbackResponse> GetFeedbackByIdAsync(long id);
        Task<FeedbackResponse> CreateFeedbackAsync(FeedbackRequset request);
        Task<FeedbackResponse> UpdateFeedbackAsync(long id, FeedbackRequset request);
        Task<bool> DeleteFeedbackAsync(long id);
    }
}
