using Medical_Appointment_System.Models;
using Microsoft.AspNetCore.Mvc;
using OnlineMedicalAppointmentSystem.Models.Dtos;
using OnlineMedicalAppointmentSystem.Services.Interfaces;

namespace OnlineMedicalAppointmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ILogger _logger;
        public AppointmentController(IAppointmentService appointmentService, ILogger<AppointmentController> logger)
        {
            _appointmentService = appointmentService;
            _logger = logger;
        }

        [HttpPost("createAppointment/{serviceName}/{availabilitySlotId:guid}")]
        public async Task<ActionResult<Guid>> CreateAppointment(string serviceName, Guid availabilitySlotId, [FromBody] AppointmentCreateDto appointmentCreate)
        {
            if (appointmentCreate == null)
            {
                _logger.LogError("[AppointmentController] Appointment Create Dto object received is null.");
                return StatusCode(400);
            }
            var createdAppointmentId = await _appointmentService.CreateAppointment(appointmentCreate, serviceName, availabilitySlotId);
            if (createdAppointmentId == Guid.Empty)
            {
                _logger.LogError("[AppointmentController] Failed to create appointment.");
                return StatusCode(400);
            }
            return Ok(createdAppointmentId);
        }

        [HttpGet("getAllAppointmentsByDate/{dateSelected:datetime}")]
        public async Task<ActionResult<List<AppointmentReadDto>>> GetAllAppointmentsByDate(DateTime dateSelected)
        {
            var appointments = await _appointmentService.GetAllAppointmentsByDate(dateSelected);
            if (appointments.Count == 0)
            {
                _logger.LogInformation("[AppointmentController] No appointments found for the selected date: {DateSelected}", dateSelected);
            }
            return Ok(appointments);
        }
        [HttpDelete("deleteAppointment/{appointmentId:guid}")]
        public async Task<ActionResult> DeleteAppointment(Guid appointmentId)
        {
            var isDeleted = await _appointmentService.DeleteAppointment(appointmentId);
            if (!isDeleted)
            {
                _logger.LogError("[AppointmentController] Failed to delete appointment with ID: {AppointmentId}", appointmentId);
                return NotFound();
            }
            return Ok(isDeleted);
        }
    }
}
