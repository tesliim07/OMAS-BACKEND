using Medical_Appointment_System.Models;

namespace OnlineMedicalAppointmentSystem.Repositories.Interfaces
{
    public interface IAvailabilitySlotRepository
    {
        public Task<Guid> CreateAvailabilitySlot(AvailabilitySlot availabilitySlot);
        public Task<AvailabilitySlot> GetAvailabilitySlotById(Guid availabilitySlotId);
        public Task<AvailabilitySlot> GetAvailabilitySlotByDate(DateTime dateTime);
        public Task<List<AvailabilitySlot>> GetAllAvailabilitySlotsByDateAndServiceName(DateTime dateSelected, string serviceName);
        public Task<bool> DeleteAvailabilitySlot(Guid availabilitySlotId);
    }
}
