using Medical_Appointment_System.Models;

namespace OnlineMedicalAppointmentSystem.Repositories.Interfaces
{
    public interface IServiceRepository
    {
        public Task<Guid> CreateService(Service service);
        public Task<Service> GetServiceById(Guid serviceId);
        public Task<Service> GetServiceByName(string serviceName);
        public Task<List<Service>> GetAllServices();
        public Task<bool> DeleteService(Guid serviceId);
    }
}
