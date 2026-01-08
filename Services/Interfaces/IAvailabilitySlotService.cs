
using OnlineMedicalAppointmentSystem.Models.Dtos;

namespace OnlineMedicalAppointmentSystem.Services.Interfaces
{
    public interface IAvailabilitySlotService
    {
        public Task<List<Guid>> CreateAvailabilitySlot(Guid serviceId, DateTime startRange, DateTime endRange);
        public Task<AvailabilitySlotReadDto> GetAvailabilitySlotById(Guid availabilitySlotId);
        public Task<List<AvailabilitySlotReadDto>> GetAllAvailabilitySlotsByDateAndServiceName(DateTime dateSelected, string serviceName);
        public Task<bool> DeleteAvailabilitySlot(Guid availabilitySlotId);

    }
}
