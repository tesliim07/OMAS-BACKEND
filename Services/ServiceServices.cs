using Medical_Appointment_System.Models;
using OnlineMedicalAppointmentSystem.Models.Dtos;
using OnlineMedicalAppointmentSystem.Repositories.Interfaces;
using OnlineMedicalAppointmentSystem.Services.Interfaces;

namespace OnlineMedicalAppointmentSystem.Services
{
    public class ServiceServices : IServiceServices
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly ILogger _logger;
        public ServiceServices(IServiceRepository serviceRepository, ILogger<ServiceServices> logger)
        {
            _serviceRepository = serviceRepository;
            _logger = logger;
        }

        public async Task<Guid> CreateService(ServiceCreateDto serviceCreate)
        {
            if (serviceCreate == null)
            {
                _logger.LogError("[ServiceServices] ServiceCreateDto is null.");
                return Guid.Empty;
            }
            var dto = new ServiceCreateDto()
            {
                ServiceName = serviceCreate.ServiceName,
                DurationInMinutes = serviceCreate.DurationInMinutes
            };
            var newService = new Service()
            {
                ServiceId = Guid.NewGuid(),
                ServiceName = dto.ServiceName,
                DurationInMinutes = dto.DurationInMinutes ?? 30
            };
            return await _serviceRepository.CreateService(newService);
        }

        public async Task<ServiceReadDto> GetServiceById(Guid serviceId)
        {
            var service = await _serviceRepository.GetServiceById(serviceId);
            if (service == null)
            {
                _logger.LogError($"[ServiceServices] Service with ID {serviceId} not found.");
                return null;
            }
            var dto = new ServiceReadDto()
            {
                ServiceId = service.ServiceId,
                ServiceName = service.ServiceName,
                DurationInMinutes = service.DurationInMinutes
            };
            return dto;
        }

        public async Task<ServiceReadDto> GetServiceByName(string serviceName)
        {
            var service = await _serviceRepository.GetServiceByName(serviceName);
            if (service == null)
            {
                _logger.LogError($"[ServiceServices] Service with name {serviceName} not found.");
                return null;
            }
            var dto = new ServiceReadDto()
            {
                ServiceId = service.ServiceId,
                ServiceName = service.ServiceName,
                DurationInMinutes = service.DurationInMinutes
            };
            return dto;
        }

        public async Task<List<ServiceReadDto>> GetAllServices()
        {
            var services = await _serviceRepository.GetAllServices();
            var dtoList = new List<ServiceReadDto>();
            foreach (var service in services)
            {
                dtoList.Add(new ServiceReadDto()
                {
                    ServiceId = service.ServiceId,
                    ServiceName = service.ServiceName,
                    DurationInMinutes = service.DurationInMinutes
                });
            };
            return dtoList;
        }

        public async Task<bool> DeleteService(Guid serviceId)
        {
            var isDeleted = await _serviceRepository.DeleteService(serviceId);
            if (isDeleted)
            {
                return isDeleted;
            }
            _logger.LogError($"[ServiceServices] Failed to delete Service with ID {serviceId}.");
            return isDeleted;

        }
    }
}
