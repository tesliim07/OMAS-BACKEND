using Medical_Appointment_System.Models;

namespace OnlineMedicalAppointmentSystem.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        public Task<Guid> CreateAppointment(Appointment appointment);
        public Task<Appointment> GetAppointmentById(Guid appointmentId);
        public Task<List<Appointment>> GetAllAppointmentsByDate(DateTime dateSelected);
        public Task<List<Appointment>> GetAllAppointments();
        public Task<List<Appointment>> GetAllPendingAppointments(int daysToAppointment);
        //public Task<bool> RescheduleAppointment(Guid appointmentId, DateTime newDateTime);
        public Task<bool> DeleteAppointment(Guid appointmentId);
    }
}
