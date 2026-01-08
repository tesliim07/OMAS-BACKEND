using Medical_Appointment_System.Models;
using OnlineMedicalAppointmentSystem.Models.Dtos;
using OnlineMedicalAppointmentSystem.Repositories.Interfaces;
using OnlineMedicalAppointmentSystem.Services.Interfaces;

namespace OnlineMedicalAppointmentSystem.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IServiceServices _serviceService;
        private readonly IAvailabilitySlotService _availabilitySlotService;
        private readonly ILogger _logger;
        public AppointmentService(IAppointmentRepository appointmentRepository, IServiceServices serviceService, IAvailabilitySlotService availabilitySlotService, ILogger<AppointmentService> logger)
        {
            _appointmentRepository = appointmentRepository;
            _serviceService = serviceService;
            _availabilitySlotService = availabilitySlotService;
            _logger = logger;
        }

        public async Task<Guid> CreateAppointment(AppointmentCreateDto appointment, string serviceName, Guid availabilitySlotId)
        {
            var slot = await _availabilitySlotService.GetAvailabilitySlotById(availabilitySlotId);
            if (slot == null || slot.IsBooked == true)
            {
                _logger.LogError("[AppointmentService] Attempted to book an appointment with an invalid slotId or already booked slot.");
                return Guid.Empty;
            }
            var service = await _serviceService.GetServiceByName(serviceName);
            if (service == null)
            {
                _logger.LogError("[AppointmentService] Attempted to book an appointment with an invalid service ID.");
                return Guid.Empty;
            }
            var dto = new AppointmentCreateDto()
            {
                PatientFirstName = appointment.PatientFirstName,
                PatientLastName = appointment.PatientLastName,
                PatientEmail = appointment.PatientEmail,
                AppointmentDateTime = appointment.AppointmentDateTime
            };
            var newAppointment = new Appointment
            {
                AppointmentId = Guid.NewGuid(),
                PatientFirstName = dto.PatientFirstName,
                PatientLastName = dto.PatientLastName,
                PatientEmail = dto.PatientEmail,
                AppointmentDateTime = dto.AppointmentDateTime,
                AppointmentBookedStatus = true,
                ServiceId = service.ServiceId,
                AvailabilitySlotId = availabilitySlotId
            };
            var appointmentId = await _appointmentRepository.CreateAppointment(newAppointment);
            return appointmentId;
        }

        public async Task<List<AppointmentReadDto>> GetAllAppointmentsByDate(DateTime dateSelected)
        {
            var appointments = await _appointmentRepository.GetAllAppointmentsByDate(dateSelected);
            var dtoList = new List<AppointmentReadDto>();
            foreach (var appointment in appointments)
            {
                var dto = new AppointmentReadDto
                {
                    AppointmentId = appointment.AppointmentId,
                    PatientFirstName = appointment.PatientFirstName,
                    PatientLastName = appointment.PatientLastName,
                    PatientEmail = appointment.PatientEmail,
                    AppointmentDateTime = appointment.AppointmentDateTime,
                    AppointmentBookedStatus = appointment.AppointmentBookedStatus
                };
                dtoList.Add(dto);
            }

            return dtoList;
        }

        public async Task<bool> DeleteAppointment(Guid appointmentId)
        {
            var isDeleted = await _appointmentRepository.DeleteAppointment(appointmentId);
            if (isDeleted)
            {
                return isDeleted;
            }
            _logger.LogError($"[AppointmentService] Failed to delete Appointment with ID {appointmentId}.");
            return isDeleted;
        }
    }
}
