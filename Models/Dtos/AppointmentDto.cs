namespace OnlineMedicalAppointmentSystem.Models.Dtos
{
    public class AppointmentCreateDto
    {
        public required string PatientFirstName { get; set; }
        public required string PatientLastName { get; set; }
        public string? PatientEmail { get; set; }
        public DateTime AppointmentDateTime { get; set; }
    }

    public class AppointmentReadDto
    {
        public Guid AppointmentId { get; set; }
        public required string PatientFirstName { get; set; }
        public required string PatientLastName { get; set; }
        public string? PatientEmail { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public bool AppointmentBookedStatus { get; set; }
    }
}
