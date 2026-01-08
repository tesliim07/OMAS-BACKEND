using OnlineMedicalAppointmentSystem.Models.Dtos;

namespace OnlineMedicalAppointmentSystem.Services.Interfaces
{
    public interface IAppointmentService
    {
        public Task<Guid> CreateAppointment(AppointmentCreateDto appointment, string serviceName, Guid availabilitySlotId);
        public Task<List<AppointmentReadDto>> GetAllAppointmentsByDate(DateTime dateSelected);
        public Task<bool> DeleteAppointment(Guid appointmentId);
    }
}
