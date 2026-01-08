using Medical_Appointment_System.Models;
using Microsoft.AspNetCore.Mvc;
using OnlineMedicalAppointmentSystem.Models.Dtos;
using OnlineMedicalAppointmentSystem.Services.Interfaces;

namespace OnlineMedicalAppointmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceServices _serviceServices;
        private readonly ILogger _logger;
        public ServiceController(IServiceServices serviceServices, ILogger<ServiceController> logger)
        {
            _serviceServices = serviceServices;
            _logger = logger;
        }

        [HttpPost("createService")]
        public async Task<ActionResult<Guid>> CreateService([FromBody] ServiceCreateDto serviceCreate)
        {
            if (serviceCreate == null) {
                _logger.LogError("[ServiceController] ServiceCreateDto object received is null.");
                return StatusCode(400);
            }
            var createdServiceId = await _serviceServices.CreateService(serviceCreate);
            return Ok(createdServiceId);
        }

        [HttpGet("getServiceById/{serviceId}")]
        public async Task<ActionResult<ServiceReadDto>> GetServiceById(Guid serviceId)
        {
            var service = await _serviceServices.GetServiceById(serviceId);
            if (service == null)
            {
                _logger.LogError("[ServiceController] Service with ID {ServiceId} not found.", serviceId);
                return StatusCode(400);
            }
            return Ok(service);
        }

        [HttpGet("getServiceByName/{serviceName}")]
        public async Task<ActionResult<ServiceReadDto>> GetServiceByName(string serviceName)
        {
            var service = await _serviceServices.GetServiceByName(serviceName);
            if (service == null)
            {
                _logger.LogError("[ServiceController] Service with name {ServiceName} not found.", serviceName);
                return StatusCode(400);
            }
            return Ok(service);
        }

        [HttpGet("getAllServices")]
        public async Task<ActionResult<List<ServiceReadDto>>> GetAllServices()
        {
            var services = await _serviceServices.GetAllServices();
            if (services.Count == 0)
            {
                _logger.LogInformation("[ServiceController] No services found in the system.");
            }
            return Ok(services);
        }

        [HttpDelete("deleteService/{serviceId}")]
        public async Task<ActionResult> DeleteService(Guid serviceId)
        {
            var isDeleted = await _serviceServices.DeleteService(serviceId);
            if (!isDeleted)
            {
                _logger.LogError("[ServiceController] Failed to delete service with ID: {ServiceId}", serviceId);
                return StatusCode(400);
            }
            return Ok(isDeleted);
        }

    }

}
