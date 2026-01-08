using Medical_Appointment_System.Models;
using Microsoft.AspNetCore.Mvc;
using OnlineMedicalAppointmentSystem.Models.Dtos;
using OnlineMedicalAppointmentSystem.Services.Interfaces;

namespace OnlineMedicalAppointmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilitySlotController : ControllerBase
    {
        private readonly IAvailabilitySlotService _availabilitySlotService;
        private readonly ILogger _logger;
        public AvailabilitySlotController(IAvailabilitySlotService availabilitySlotService, ILogger<AvailabilitySlotController> logger)
        {
            _availabilitySlotService = availabilitySlotService;
            _logger = logger;
        }

        [HttpPost("createAvailabilitySlots/{serviceId:guid}/{startRange:datetime}/{endRange:datetime}")]
        public async Task<ActionResult<List<Guid>>> CreateAvailabilitySlots(Guid serviceId, DateTime startRange, DateTime endRange)
        {
            var createdSlotIds = await _availabilitySlotService.CreateAvailabilitySlot(serviceId, startRange, endRange);
            if (createdSlotIds == null || createdSlotIds.Count == 0)
            {
                _logger.LogError("[AvailabiltySlotController] Failed to create availability slots.");
                return StatusCode(400, "Failed to create availability slots.");
            }
            return Ok(createdSlotIds);
        }

        [HttpGet("getAvailabilitySlotById/{availabilitySlotId:guid}")]
        public async Task<ActionResult<AvailabilitySlotReadDto>> GetAvailabilitySlotById(Guid availabilitySlotId)
        {
            var slot = await _availabilitySlotService.GetAvailabilitySlotById(availabilitySlotId);
            if (slot == null)
            {
                _logger.LogError("[AvailabiltySlotController] Availability slot not found for ID: {AvailabilitySlotId}", availabilitySlotId);
                return StatusCode(400);
            }
            return Ok(slot);
        }

        [HttpGet("getAllAvailabilitySlotsByDateAndServiceName/{dateSelected:datetime}/{serviceName}")]
        public async Task<ActionResult<List<AvailabilitySlotReadDto>>> GetAllAvailabilitySlotsByDateAndServiceName(DateTime dateSelected, string serviceName)
        {
            var slots = await _availabilitySlotService.GetAllAvailabilitySlotsByDateAndServiceName(dateSelected, serviceName);
            if (slots == null)
            {
                _logger.LogError("[AvailabiltySlotController] Failed to retrieve availability slots for date: {DateSelected} and service: {ServiceName}", dateSelected, serviceName);
                return StatusCode(400);
            }
            if (slots.Count == 0)
            {
                _logger.LogInformation("[AvailabiltySlotController] No availability slots found for date: {DateSelected}", dateSelected);
            }
            return Ok(slots);
        }

        [HttpDelete("deleteAvailabilitySlot/{availabilitySlotId:guid}")]
        public async Task<ActionResult> DeleteAvailabilitySlot(Guid availabilitySlotId)
        {
            var isDeleted = await _availabilitySlotService.DeleteAvailabilitySlot(availabilitySlotId);
            if (!isDeleted)
            {
                _logger.LogError("[AvailabiltySlotController] Failed to delete availability slot with ID: {AvailabilitySlotId}", availabilitySlotId);
                return StatusCode(400);
            }
            return Ok(isDeleted);
        }
    }
}
