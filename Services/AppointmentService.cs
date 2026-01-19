using Medical_Appointment_System.Models;
using OnlineMedicalAppointmentSystem.Models.Dtos;
using OnlineMedicalAppointmentSystem.Repositories.Interfaces;
using OnlineMedicalAppointmentSystem.Services.Interfaces;
using System.Text;

namespace OnlineMedicalAppointmentSystem.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IServiceServices _serviceService;
        private readonly IEmailService _emailService;
        private readonly IAvailabilitySlotService _availabilitySlotService;
        private readonly ILogger _logger;
        public AppointmentService(IAppointmentRepository appointmentRepository, IServiceServices serviceService, IAvailabilitySlotService availabilitySlotService, ILogger<AppointmentService> logger, IEmailService emailService)
        {
            _appointmentRepository = appointmentRepository;
            _serviceService = serviceService;
            _availabilitySlotService = availabilitySlotService;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<Guid> CreateAppointment(AppointmentCreateDto appointment, string serviceName, Guid availabilitySlotId)
        {
            var slot = await _availabilitySlotService.GetAvailabilitySlotWithServiceIdById(availabilitySlotId);
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
            if (slot.ServiceId != service.ServiceId)
            {
                _logger.LogError("[AppointmentService] Attempted to book an appointment where the slot's service does not match the requested service.");
                return Guid.Empty;
            }
            var dto = new AppointmentCreateDto()
            {
                PatientFirstName = appointment.PatientFirstName,
                PatientLastName = appointment.PatientLastName,
                PatientEmail = appointment.PatientEmail
            };
            var newAppointment = new Appointment
            {
                AppointmentId = Guid.NewGuid(),
                PatientFirstName = dto.PatientFirstName,
                PatientLastName = dto.PatientLastName,
                PatientEmail = dto.PatientEmail,
                AppointmentDateTime = slot.SlotStartTime,
                AppointmentBookedStatus = true,
                ServiceId = service.ServiceId,
                AvailabilitySlotId = availabilitySlotId
            };
            var appointmentId = await _appointmentRepository.CreateAppointment(newAppointment);
            return appointmentId;
        }

        public async Task<AppointmentReadDto> GetAppointmentById(Guid appointmentId)
        {
            var appointment = await _appointmentRepository.GetAppointmentById(appointmentId);
            if (appointment == null)
            {
                _logger.LogError($"[AppointmentService] Appointment with ID {appointmentId} not found.");
                return null;
            }
            var dto = new AppointmentReadDto
            {
                AppointmentId = appointment.AppointmentId,
                PatientFirstName = appointment.PatientFirstName,
                PatientLastName = appointment.PatientLastName,
                PatientEmail = appointment.PatientEmail,
                AppointmentDateTime = appointment.AppointmentDateTime,
                AppointmentBookedStatus = appointment.AppointmentBookedStatus
            };
            return dto;
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

        public async Task<bool> SendAppointmentConfirmation(Guid appointmentId)
        {
            var appointment = await _appointmentRepository.GetAppointmentById(appointmentId);
            
            if (!String.IsNullOrEmpty(appointment.PatientEmail))
            {
                var service = await _serviceService.GetServiceById(appointment.ServiceId);
                if (!String.IsNullOrEmpty(appointment.PatientEmail))
                {
                    var subject = "Appointment Confirmation";
                    var to = appointment.PatientEmail;
                    var messageBuilt = new StringBuilder();
                    messageBuilt.AppendLine($"<h1>Dear {appointment.PatientFirstName} {appointment.PatientLastName}, </h1>");
                    messageBuilt.AppendLine($"<p>This is a confirmation that you have a {service.ServiceName} appointment scheduled on {appointment.AppointmentDateTime}.</p>");
                    _logger.LogInformation($"[AppointmentService] Sending appointment confirmation for appointment id, {appointment.AppointmentId}.");
                    var htmlbody = messageBuilt.ToString();
                    var isSent = await SendEmail(to, subject, htmlbody);
                    return isSent;
                }
            }
            return false;
        }

        public async Task SendAppointmentReminder(int daysToAppointment)
        {
            var pendingAppointments = await _appointmentRepository.GetAllPendingAppointments(daysToAppointment);
            foreach (var appointment in pendingAppointments)
            {
                var service = await _serviceService.GetServiceById(appointment.ServiceId);
                if (!String.IsNullOrEmpty(appointment.PatientEmail))
                {
                    var subject = "Appointment Reminder";
                    var to = appointment.PatientEmail;
                    var messageBuilt = new StringBuilder();
                    messageBuilt.AppendLine($"<h1>Dear {appointment.PatientFirstName} {appointment.PatientLastName}, </h1>");
                    messageBuilt.AppendLine($"<p>This is a reminder that you have a {service.ServiceName} appointment scheduled on {appointment.AppointmentDateTime}.</p>");
                    _logger.LogInformation($"[AppointmentService] Sending appointment reminders  for appointments in {daysToAppointment} days.");
                    var htmlbody = messageBuilt.ToString();
                    await SendEmail(to, subject, htmlbody);
                }
           
            }
        }

        public async Task<bool> SendEmail(string to, string subject, string htmlBody)
        {
            var omasEmail = await _emailService.SendEmail(to, subject, htmlBody);
            return omasEmail;
        }
    }
}
