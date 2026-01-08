using Medical_Appointment_System.Models;
using OnlineMedicalAppointmentSystem.Models.Dtos;

namespace OnlineMedicalAppointmentSystem.Services.Interfaces
{
    public interface IServiceServices
    {
        public Task<Guid> CreateService(ServiceCreateDto serviceCreate);
        public Task<ServiceReadDto> GetServiceById(Guid serviceId);
        public Task<ServiceReadDto> GetServiceByName(string serviceName);
        public Task<List<ServiceReadDto>> GetAllServices();
        public Task<bool> DeleteService(Guid serviceId);
    }
}
