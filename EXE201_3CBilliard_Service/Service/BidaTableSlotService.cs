using AutoMapper;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Repository.Entities;
using EXE201_3CBilliard_Repository.Repository;
using EXE201_3CBilliard_Service.Exceptions;
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
            if (slotIds == null || !slotIds.Any())
                throw new ArgumentException("SlotIds list is null or empty.");

            var bidaTable = _unitOfWork.BidaTableRepository.GetById(bidaTableId);
            if (bidaTable == null)
                throw new Exception($"BidaTable with id {bidaTableId} not found.");

            // Validate if provided slot IDs are unique
            var duplicateSlotIds = slotIds.GroupBy(x => x)
                                          .Where(g => g.Count() > 1)
                                          .Select(y => y.Key)
                                          .ToList();
            if (duplicateSlotIds.Any())
            {
                var duplicateSlotIdString = string.Join(", ", duplicateSlotIds);
                throw new Exception($"Provided slot IDs {duplicateSlotIdString} are duplicated.");
            }

            var existingSlotIds = _unitOfWork.BidaTableSlotRepository
                .Get(filter: x => x.BidaTableId == bidaTableId)
                .Select(x => x.SlotId)
                .ToHashSet();

            var bidaTableSlots = new List<BidaTable_Slot>();
            foreach (var slotId in slotIds)
            {
                if (existingSlotIds.Contains(slotId))
                    throw new SlotAlreadyExistsException($"Slot with id {slotId} already exists in BidaTable with id {bidaTableId}.");

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

        public async Task<GetSlotByBidatableResponse> UpdateSlotsOfBidaTableAsync(long bidaTableId, List<long> slotIds)
        {
            if (slotIds == null || !slotIds.Any())
                throw new ArgumentException("SlotIds list is null or empty.");

            var bidaTable = _unitOfWork.BidaTableRepository.GetById(bidaTableId);
            if (bidaTable == null)
                throw new Exception($"BidaTable with id {bidaTableId} not found.");

            // Validate if provided slot IDs are unique
            var duplicateSlotIds = slotIds.GroupBy(x => x)
                                          .Where(g => g.Count() > 1)
                                          .Select(y => y.Key)
                                          .ToList();
            if (duplicateSlotIds.Any())
            {
                var duplicateSlotIdString = string.Join(", ", duplicateSlotIds);
                throw new Exception($"Provided slot IDs {duplicateSlotIdString} are duplicated.");
            }

            // Get existing slots of BidaTable
            var existingBidaTableSlots = _unitOfWork.BidaTableSlotRepository.Get(filter: x => x.BidaTableId == bidaTableId).ToList();

            // Create new and removed slot lists
            var newSlotIds = slotIds.Except(existingBidaTableSlots.Select(x => x.SlotId)).ToList();
            var removedSlotIds = existingBidaTableSlots.Where(x => !slotIds.Contains(x.SlotId)).Select(x => x.SlotId).ToList();

            // Add new slots
            foreach (var slotId in newSlotIds)
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
            }

            // Remove old slots
            foreach (var slotId in removedSlotIds)
            {
                var bidaTableSlot = existingBidaTableSlots.FirstOrDefault(x => x.SlotId == slotId);
                if (bidaTableSlot != null)
                    _unitOfWork.BidaTableSlotRepository.Delete(bidaTableSlot);
            }

            _unitOfWork.Save();

            // Return updated slots of BidaTable
            var updatedBidaTableSlots = _unitOfWork.BidaTableSlotRepository.Get(filter: x => x.BidaTableId == bidaTableId, includeProperties: "Slot");
            var slotResponses = updatedBidaTableSlots.Select(bs => _mapper.Map<SlotResponse>(bs.Slot)).ToList();
            return new GetSlotByBidatableResponse
            {
                BidaTableId = bidaTableId,
                SlotIds = slotResponses
            };
        }

        public async Task DeleteBidaTableAndSlotsAsync(long bidaTableId)
        {
            var bidaTable = _unitOfWork.BidaTableRepository.GetById(bidaTableId);
            if (bidaTable == null)
                throw new Exception($"BidaTable with id {bidaTableId} not found.");

            // Get all BidaTable_Slot entries for this BidaTable
            var bidaTableSlots = _unitOfWork.BidaTableSlotRepository.Get(filter: x => x.BidaTableId == bidaTableId).ToList();

            // Delete all related BidaTable_Slot entries
            foreach (var bidaTableSlot in bidaTableSlots)
            {
                _unitOfWork.BidaTableSlotRepository.Delete(bidaTableSlot);
            }

            // Delete the BidaTable
            _unitOfWork.BidaTableRepository.Delete(bidaTable);

            // Save changes
            _unitOfWork.Save();
        }

        public async Task<IEnumerable<BidaTableSlotResponse>> GetAllAsync()
        {
            var bidaTableSlots = _unitOfWork.BidaTableSlotRepository.Get(includeProperties: "Slot,BidaTable").ToList();
            return _mapper.Map<IEnumerable<BidaTableSlotResponse>>(bidaTableSlots);
        }

        public async Task<IEnumerable<BidaTableSlotResponse>> GetBookedSlotsByDateAndTableAsync(DateTime bookingDate, long bidaTableId)
        {
            var bookingsOnDateAndTable = _unitOfWork.BookingRepository.Get(filter: b => b.BookingDate.Date == bookingDate.Date && b.BSlot.BidaTableId == bidaTableId).ToList();
            var bookedSlotIds = bookingsOnDateAndTable.Select(b => b.BT_SlotId).ToList();
            var bookedSlots = _unitOfWork.BidaTableSlotRepository.Get(filter: x => bookedSlotIds.Contains(x.Id), includeProperties: "Slot,BidaTable").ToList();

            return _mapper.Map<IEnumerable<BidaTableSlotResponse>>(bookedSlots);
        }

        public async Task<IEnumerable<BidaTableSlotResponse>> GetBidaTableSlotByIdAsync(long bidaTableId)
        {
            var bidaTableSlot = _unitOfWork.BidaTableSlotRepository.Get(filter: bts => bts.BidaTableId == bidaTableId, includeProperties: "Slot,BidaTable").ToList();

            if (bidaTableSlot == null)
            {
                throw new Exception($"BidaTableSlot with id {bidaTableId} not found.");
            }

            return _mapper.Map<IEnumerable<BidaTableSlotResponse>>(bidaTableSlot);
        }
    }
}
