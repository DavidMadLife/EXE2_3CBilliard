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
    public class SlotService : ISlotService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SlotService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SlotResponse>> GetAllSlotsAsync()
        {
            var slots = _unitOfWork.SlotRepository.Get();
            return _mapper.Map<IEnumerable<SlotResponse>>(slots);
        }

        public async Task<SlotResponse> GetSlotByIdAsync(long id)
        {
            var slot = _unitOfWork.SlotRepository.GetById(id);
            return _mapper.Map<SlotResponse>(slot);
        }

        public async Task<SlotResponse> CreateSlotAsync(SlotRequest request)
        {
            var slot = _mapper.Map<Slot>(request);
            _unitOfWork.SlotRepository.Insert(slot);
            _unitOfWork.Save();
            return _mapper.Map<SlotResponse>(slot);
        }

        public async Task<SlotResponse> UpdateSlotAsync(long id, SlotRequest request)
        {
            var slot = _unitOfWork.SlotRepository.GetById(id);
            if (slot == null)
                throw new Exception($"Slot with id {id} not found.");

            _mapper.Map(request, slot);
            _unitOfWork.SlotRepository.Update(slot);
            _unitOfWork.Save();
            return _mapper.Map<SlotResponse>(slot);
        }

        public async Task<bool> DeleteSlotAsync(long id)
        {
            var slot = _unitOfWork.SlotRepository.GetById(id);
            if (slot == null)
                throw new Exception($"Slot with id {id} not found.");

            _unitOfWork.SlotRepository.Delete(slot);
            _unitOfWork.Save();
            return true;
        }
    }
}
