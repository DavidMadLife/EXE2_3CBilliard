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
    public class BidaTableService : IBidaTableService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        

        public BidaTableService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BidaTableResponse>> GetAllBidaTablesAsync()
        {
            var bidaTables = _unitOfWork.BidaTableRepository.Get();
            return _mapper.Map<IEnumerable<BidaTableResponse>>(bidaTables);
        }

        public async Task<BidaTableResponse> GetBidaTableByIdAsync(long id)
        {
            var bidaTable = _unitOfWork.BidaTableRepository.GetById(id);
            return _mapper.Map<BidaTableResponse>(bidaTable);
        }

        public async Task<BidaTableResponse> CreateBidaTableAsync(BidaTableRequest request)
        {
            var bidaTable = _mapper.Map<BidaTable>(request);
            bidaTable.Status = BidaTableStatus.ACTIVE;
            bidaTable.CreateAt = DateTime.Now;
            _unitOfWork.BidaTableRepository.Insert(bidaTable);
            _unitOfWork.Save();
            return _mapper.Map<BidaTableResponse>(bidaTable);
        }

        public async Task<BidaTableResponse> UpdateBidaTableAsync(long id, BidaTableRequest request)
        {
            var bidaTable = _unitOfWork.BidaTableRepository.GetById(id);
            if (bidaTable == null)
                throw new Exception($"BidaTable with id {id} not found.");

            _mapper.Map(request, bidaTable);
            _unitOfWork.BidaTableRepository.Update(bidaTable);
            _unitOfWork.Save();
            return _mapper.Map<BidaTableResponse>(bidaTable);
        }

        public async Task DeleteBidaTableAsync(long id)
        {
            var bidaTable = _unitOfWork.BidaTableRepository.GetById(id);
            if (bidaTable == null)
                throw new Exception($"BidaTable with id {id} not found.");

            bidaTable.Status = BidaTableStatus.DELETED;
            _unitOfWork.BidaTableRepository.Update(bidaTable);
            _unitOfWork.Save();
        }

        public async Task InactiveBidaTableAsync(long id)
        {
            var bidaTable = _unitOfWork.BidaTableRepository.GetById(id);
            if (bidaTable == null)
                throw new Exception($"BidaTable with id {id} not found.");

            bidaTable.Status = BidaTableStatus.INACTIVE;
            _unitOfWork.BidaTableRepository.Update(bidaTable);
            _unitOfWork.Save();
        }


        //Add slot to bida table
        /*public async Task<IEnumerable<BidaTableSlotResponse>> AddSlotsToBidaTableAsync(long bidaTableId, List<long>? slotIds)
        {
            var bidaTable = _unitOfWork.BidaTableRepository.GetById(bidaTableId);
            if (bidaTable == null)
                throw new Exception($"BidaTable with id {bidaTableId} not found.");

            var bidaTableSlots = new List<BidaTable_Slot>();
            foreach (var slotId in slotIds)
            {
                var slot = _unitOfWork.SlotRepository.GetById(slotId);
                if (slot == null)
                    throw new Exception($"Slot with id {slotId} not found.");

                var bidaTableSlot = new BidaTable_Slot
                {
                    BidaTableId = bidaTableId,
                    SlotId = slotId,
                    Status = BidaTable_SlotStatus.ACTIVE
                };
                _unitOfWork.BidaTableSlotRepository.Insert(bidaTableSlot);
                bidaTableSlots.Add(bidaTableSlot);
            }
            _unitOfWork.Save();
            return _mapper.Map<IEnumerable<BidaTableSlotResponse>>(bidaTableSlots);
        }*/

        //Filter
        public async Task<(IEnumerable<BidaTableResponse> bidaTables, int totalCount)> SearchBidaTablesAsync(string? tableName, double? price, long? bidaClubId, int pageIndex = 1, int pageSize = 10)
        {
            var bidaTablesResult = _unitOfWork.BidaTableRepository.GetWithCount(filter: bt =>
                (string.IsNullOrEmpty(tableName) || bt.TableName.Contains(tableName)) &&
                (!price.HasValue || bt.Price == price.Value) &&
                (!bidaClubId.HasValue || bt.BidaCludId == bidaClubId.Value),
                pageIndex: pageIndex,
                pageSize: pageSize);

            var bidaTables = bidaTablesResult.items;
            var totalCount = bidaTablesResult.totalCount;

            return (_mapper.Map<IEnumerable<BidaTableResponse>>(bidaTables), totalCount);
        }



    }
}
