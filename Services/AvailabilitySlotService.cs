using Medical_Appointment_System.Models;
using OnlineMedicalAppointmentSystem.Models.Dtos;
using OnlineMedicalAppointmentSystem.Repositories.Interfaces;
using OnlineMedicalAppointmentSystem.Services.Interfaces;
using static System.Reflection.Metadata.BlobBuilder;

namespace OnlineMedicalAppointmentSystem.Services
{
    public class AvailabilitySlotService : IAvailabilitySlotService
    {
        private readonly IAvailabilitySlotRepository _availabilitySlotRepository;
        private readonly IServiceServices _serviceServices;
        private readonly ILogger _logger;
        public AvailabilitySlotService(IAvailabilitySlotRepository availabilitySlotRepository, IServiceServices serviceServices, ILogger<AvailabilitySlotService> logger)
        {
            _availabilitySlotRepository = availabilitySlotRepository;
            _serviceServices = serviceServices;
            _logger = logger;
        }

        public async Task<List<Guid>> CreateAvailabilitySlot(Guid serviceId, DateTime startRange, DateTime endRange)
        {
            var createdSlotIds = new List<Guid>();
            var service = await _serviceServices.GetServiceById(serviceId);
            if (service == null)
            {
                _logger.LogError($"[AvailabilitySlotService] Service with ID {serviceId} not found.");
                return null;
            }
            var timeRange = endRange - startRange;
            while (startRange.AddMinutes(service.DurationInMinutes) <= endRange)
            {
                var slot = await _availabilitySlotRepository.GetAvailabilitySlotByDate(startRange, service.DurationInMinutes);
                if (slot == null)
                {
                    var newSlot = new AvailabilitySlot
                    {
                        AvailabilitySlotId = Guid.NewGuid(),
                        SlotStartTime = startRange,
                        SlotEndTime = startRange.AddMinutes(service.DurationInMinutes),
                        IsBooked = false,
                        ServiceId = service.ServiceId
                    };
                    var slotId = await _availabilitySlotRepository.CreateAvailabilitySlot(newSlot);
                    createdSlotIds.Add(slotId);
                }
                startRange = startRange.AddMinutes(service.DurationInMinutes);
            }
            return createdSlotIds;
        }

        public async Task<AvailabilitySlotReadDto> GetAvailabilitySlotById(Guid availabilitySlotId)
        {
            var slot = await _availabilitySlotRepository.GetAvailabilitySlotById(availabilitySlotId);
            if (slot == null)
            {
                _logger.LogError($"[AvailabilitySlotService] Availability Slot with ID {availabilitySlotId} not found.");
                return null;
            }
            var dto = new AvailabilitySlotReadDto
            {
                AvailabilitySlotId = slot.AvailabilitySlotId,
                SlotStartTime = slot.SlotStartTime,
                SlotEndTime = slot.SlotEndTime,
                IsBooked = slot.IsBooked
            };
            return dto;
        }

        public async Task<AvailabilitySlotWithServiceIdReadDto> GetAvailabilitySlotWithServiceIdById(Guid availabilitySlotId)
        {
            var slot = await _availabilitySlotRepository.GetAvailabilitySlotById(availabilitySlotId);
            if (slot == null)
            {
                _logger.LogError($"[AvailabilitySlotService] Availability Slot with ID {availabilitySlotId} not found.");
                return null;
            }
            var dto = new AvailabilitySlotWithServiceIdReadDto
            {
                AvailabilitySlotId = slot.AvailabilitySlotId,
                SlotStartTime = slot.SlotStartTime,
                SlotEndTime = slot.SlotEndTime,
                IsBooked = slot.IsBooked,
                ServiceId = slot.ServiceId
            };
            return dto;
        }

        public async Task<List<AvailabilitySlotReadDto>> GetAllAvailabilitySlotsByDateAndServiceName(DateTime dateSelected, string serviceName)
        {
            var slots = await _availabilitySlotRepository.GetAllAvailabilitySlotsByDateAndServiceName(dateSelected, serviceName);
            if (slots == null)
            {
                _logger.LogError($"[AvailabilitySlotService] No Availability Slots found for Service {serviceName} on {dateSelected.ToShortDateString()}.");
                return null;
            }
            var dtoList = new List<AvailabilitySlotReadDto>();
            foreach ( var slot in slots) {
                var dto = new AvailabilitySlotReadDto()
                {
                    AvailabilitySlotId = slot.AvailabilitySlotId,
                    SlotStartTime = slot.SlotStartTime,
                    SlotEndTime = slot.SlotEndTime,
                    IsBooked = slot.IsBooked
                };
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public async Task<bool> DeleteAvailabilitySlot(Guid availabilitySlotId)
        {
            var isDeleted = await _availabilitySlotRepository.DeleteAvailabilitySlot(availabilitySlotId);
            if (isDeleted)
            {
                return isDeleted;
            }
            _logger.LogError($"[AvailabilitySlotService] Failed to delete Availability Slot with ID {availabilitySlotId}.");
            return isDeleted;
        }
    }
}
