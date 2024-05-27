using AutoMapper;
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
    public class BidaTableSlotService : IBidaTableSlotService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public BidaTableSlotService(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BidaTableSlotResponse>> AddSlotsToBidaTableAsync(long bidaTableId, List<long>? slotIds)
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
        }

        public async Task<GetSlotByBidatableResponse> GetSlotIdsByBidaTableIdAsync(long bidaTableId)
        {
            var bidaTableSlots = _unitOfWork.BidaTableSlotRepository.Get(filter: x => x.BidaTableId == bidaTableId, includeProperties: "Slot");
            var slotResponses = bidaTableSlots.Select(bs => _mapper.Map<SlotResponse>(bs.Slot)).ToList();
            return new GetSlotByBidatableResponse
            {
                BidaTableId = bidaTableId,
                SlotIds = slotResponses
            };

        }
    }
}
