﻿using AutoMapper;
using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Repository.Entities;
using EXE201_3CBilliard_Repository.Repository;
using EXE201_3CBilliard_Repository.Tools;
using EXE201_3CBilliard_Service.Interface;
using Firebase.Auth;
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
        private readonly EXE201_3CBilliard_Repository.Tools.Firebase _firebase;



        public BidaTableService(IUnitOfWork unitOfWork, IMapper mapper, EXE201_3CBilliard_Repository.Tools.Firebase firebase)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _firebase = firebase;
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

        /*public async Task<BidaTableResponse> CreateBidaTableAsync(BidaTableRequest request)
        {
            var bidaTable = _mapper.Map<BidaTable>(request);
            bidaTable.Status = BidaTableStatus.ACTIVE;
            bidaTable.CreateAt = DateTime.Now;

            if (request.Image != null)
            {
                if (request.Image.Length >= 10 * 1024 * 1024)
                {
                    throw new Exception();
                }
                string imageDownloadUrl = await _firebase.UploadImage(request.Image);
                bidaTable.Image = imageDownloadUrl;
            }

            _unitOfWork.BidaTableRepository.Insert(bidaTable);
            _unitOfWork.Save();

            // Tính lại giá trung bình cho BidaClub và cập nhật
            var bidaClub = _unitOfWork.BidaClubRepository.GetById(bidaTable.BidaCludId);
            if (bidaClub != null)
            {
                bidaClub.AveragePrice = await CalculateAveragePriceAsync(bidaTable.BidaCludId);
                _unitOfWork.BidaClubRepository.Update(bidaClub);
                _unitOfWork.Save();
            }

            return _mapper.Map<BidaTableResponse>(bidaTable);
        }*/

        public void AddExistingSlotsToBidaTable(long bidaTableId, TimeSpan openTime, TimeSpan closeTime)
        {
            // Truy vấn danh sách các slot có sẵn
            var existingSlots = _unitOfWork.SlotRepository.Get().ToList();

            // Truy vấn danh sách các slot đã tồn tại cho BidaTableId
            var existingBidaTableSlots = _unitOfWork.BidaTableSlotRepository.Get(filter: bts => bts.BidaTableId == bidaTableId).ToList();

            // Lọc ra các slot trong khoảng thời gian mở cửa của BidaClub
            List<Slot> slotsWithinOpeningHours;

            if (openTime < closeTime)
            {
                // Trường hợp thời gian mở cửa và đóng cửa không qua nửa đêm
                slotsWithinOpeningHours = existingSlots.Where(slot =>
                {
                    var slotStartTime = slot.StartTime;
                    return slotStartTime >= openTime && slotStartTime < closeTime;
                }).ToList();
            }
            else
            {
                // Trường hợp thời gian mở cửa và đóng cửa qua nửa đêm
                slotsWithinOpeningHours = existingSlots.Where(slot =>
                {
                    var slotStartTime = slot.StartTime;
                    return slotStartTime >= openTime || slotStartTime < closeTime;
                }).ToList();
            }

            // Thêm các slot vào bảng BidaTable_Slot với BidaTableId tương ứng nếu chưa tồn tại
            foreach (var slot in slotsWithinOpeningHours)
            {
                if (!existingBidaTableSlots.Any(bts => bts.SlotId == slot.Id))
                {
                    var bidaTableSlot = new BidaTable_Slot
                    {
                        BidaTableId = bidaTableId,
                        SlotId = slot.Id,
                        Status = BidaTable_SlotStatus.ACTIVE
                    };

                    _unitOfWork.BidaTableSlotRepository.Insert(bidaTableSlot);
                }
            }

            _unitOfWork.Save();
        }



        public async Task<BidaTableResponse> CreateBidaTableAsync(BidaTableRequest request)
        {
            var bidaTable = _mapper.Map<BidaTable>(request);
            bidaTable.Status = BidaTableStatus.ACTIVE;
            bidaTable.CreateAt = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")); ;
            

            if (request.Image != null)
            {
                if (request.Image.Length >= 10 * 1024 * 1024)
                {
                    throw new Exception("Image size exceeds the limit.");
                }
                string imageDownloadUrl = await _firebase.UploadImage(request.Image);
                bidaTable.Image = imageDownloadUrl;
            }

            _unitOfWork.BidaTableRepository.Insert(bidaTable);
            _unitOfWork.Save();

            // Lấy thông tin về giờ mở và giờ kết thúc của BidaClub
            var bidaClub = _unitOfWork.BidaClubRepository.GetById(bidaTable.BidaCludId);
            if (bidaClub == null)
            {
                throw new Exception($"BidaClub with id {bidaTable.BidaCludId} not found.");
            }
            var openTime = bidaClub.OpenTime;
            var closeTime = bidaClub.CloseTime;

            // Thêm các slot có sẵn vào bảng BidaTable_Slot
            AddExistingSlotsToBidaTable(bidaTable.Id, openTime, closeTime);

            // Tính lại giá trung bình cho BidaClub và cập nhật
            bidaClub.AveragePrice = await CalculateAveragePriceAsync(bidaTable.BidaCludId);
            _unitOfWork.BidaClubRepository.Update(bidaClub);
            _unitOfWork.Save();

            return _mapper.Map<BidaTableResponse>(bidaTable);
        }






        public async Task<BidaTableResponse> UpdateBidaTableAsync(long id, BidaTableRequest request)
        {
            var bidaTable = _unitOfWork.BidaTableRepository.GetById(id);
            if (bidaTable == null)
                throw new Exception($"BidaTable with id {id} not found.");

            _mapper.Map(request, bidaTable);

            if (request.Image != null)
            {
                if (request.Image.Length >= 10 * 1024 * 1024)
                {
                    throw new Exception("Image size exceeds the limit.");
                }
                string imageDownloadUrl = await _firebase.UploadImage(request.Image);
                bidaTable.Image = imageDownloadUrl;
            }

            _unitOfWork.BidaTableRepository.Update(bidaTable);
            _unitOfWork.Save();

            // Lấy thông tin về giờ mở và giờ kết thúc của BidaClub
            var bidaClub = _unitOfWork.BidaClubRepository.GetById(bidaTable.BidaCludId);
            if (bidaClub == null)
            {
                throw new Exception($"BidaClub with id {bidaTable.BidaCludId} not found.");
            }
            var openTime = bidaClub.OpenTime;
            var closeTime = bidaClub.CloseTime;

            // Thêm các slot có sẵn vào bảng BidaTable_Slot
            AddExistingSlotsToBidaTable(bidaTable.Id, openTime, closeTime);

            // Tính lại giá trung bình cho BidaClub và cập nhật
            bidaClub.AveragePrice = await CalculateAveragePriceAsync(bidaTable.BidaCludId);
            _unitOfWork.BidaClubRepository.Update(bidaClub);
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

            // Tính lại giá trung bình cho BidaClub và cập nhật
            var bidaClub = _unitOfWork.BidaClubRepository.GetById(bidaTable.BidaCludId);
            if (bidaClub != null)
            {
                bidaClub.AveragePrice = await CalculateAveragePriceAsync(bidaTable.BidaCludId);
                _unitOfWork.BidaClubRepository.Update(bidaClub);
                _unitOfWork.Save();
            }
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



        public async Task<decimal> CalculateAveragePriceAsync(long bidaClubId)
        {
            var bidaTables = _unitOfWork.BidaTableRepository.Get(filter: bt => bt.BidaCludId == bidaClubId);
            if (bidaTables.Any())
            {
                var averagePrice = bidaTables.Select(bt => bt.Price).Average();
                return (decimal)averagePrice;
            }
            else
            {
                // Trả về 0 nếu không có bàn bi-da nào trong câu lạc bộ
                return 0;
            }
        }

    }
}
