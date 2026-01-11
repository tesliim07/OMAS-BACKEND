using OnlineMedicalAppointmentSystem.Models.Dtos;

namespace OnlineMedicalAppointmentSystem.Services.Interfaces
{
    public interface IAppointmentService
    {
        public Task<Guid> CreateAppointment(AppointmentCreateDto appointment, string serviceName, Guid availabilitySlotId);
        public Task<AppointmentReadDto> GetAppointmentById(Guid appointmentId);
        public Task<List<AppointmentReadDto>> GetAllAppointmentsByDate(DateTime dateSelected);
        public Task<bool> DeleteAppointment(Guid appointmentId);
        public Task<bool> SendAppointmentConfirmation(Guid appointmentId);
        public Task SendAppointmentReminder(int daysToAppointment);
        public Task<bool> SendEmail(string to, string subject, string htmlBody);
    }
}
