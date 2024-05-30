using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Service.Interface
{
    public interface IBidaTableService
    {
        Task<IEnumerable<BidaTableResponse>> GetAllBidaTablesAsync();
        Task<BidaTableResponse> GetBidaTableByIdAsync(long id);
        Task<BidaTableResponse> CreateBidaTableAsync(BidaTableRequest request);
        Task<BidaTableResponse> UpdateBidaTableAsync(long id, BidaTableRequest request);
        Task DeleteBidaTableAsync(long id);
        Task InactiveBidaTableAsync(long id);

        Task<(IEnumerable<BidaTableResponse> bidaTables, int totalCount)> SearchBidaTablesAsync(string? tableName, double? price, long? bidaClubId, int pageIndex, int pageSize);

    }
}
