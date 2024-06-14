using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Service.Interface
{
    public interface IBillService
    {
        Task<BillResponse> GetAndSaveBillByOrderCodeAsync(BillRequest billRequest);
        Task<BillResponse> UpdateBillStatusToActiveAsync(long billId);
        Task<BillResponse> UpdateBillStatusToInactiveAsync(long billId);
        BillTotalResponse GetTotalAmountByDateRange(BillTotalRequest request);
        Task<(IEnumerable<BillResponse> bills, int totalCount)> SearchBillsAsync(long? userId, long? clubId, string? bookerName, DateTime? createAt, string? orderCode, string? status, int pageIndex, int pageSize);
        Task CheckAndUpdateBillStatusAsync();
        Task<string> UpdateBillImageAsync(long billId, IFormFile img);
    }
}
