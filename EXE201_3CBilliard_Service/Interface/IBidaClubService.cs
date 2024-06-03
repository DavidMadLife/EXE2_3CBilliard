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
    public interface IBidaClubService
    {
        Task<BidaClubReponse> GetBidaClubByIdAsync(long id);
        Task<IEnumerable<BidaClubReponse>> GetAllBidaClubsAsync();
        Task<BidaClubReponse> CreateBidaClubAsync(BidaClubRequest request);
        Task<BidaClubReponse> UpdateBidaClubAsync(long id, BidaClubRequest request);
        Task<BidaClubReponse> ActivateBidaClubAsync(long id, NoteRequest noteRequest);
        Task<BidaClubReponse> RejectBidaClubAsync(long id, NoteRequest noteRequest);
        Task DeleteBidaClubAsync(long id);

        Task<(IEnumerable<BidaClubReponse> bidaClubs, int totalCount)> SearchBidaClubsAsync(string? bidaName, string? address, int pageIndex, int pageSize);

        Task<decimal> CalculateAveragePriceAsync(long bidaClubId);
    }
}
