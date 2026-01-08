using Medical_Appointment_System.Models;

namespace OnlineMedicalAppointmentSystem.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        public Task<Guid> CreateAppointment(Appointment appointment);
        public Task<List<Appointment>> GetAllAppointmentsByDate(DateTime dateSelected);
        //public Task<bool> RescheduleAppointment(Guid appointmentId, DateTime newDateTime);
        public Task<bool> DeleteAppointment(Guid appointmentId);
    }
}
