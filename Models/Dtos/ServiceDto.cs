namespace OnlineMedicalAppointmentSystem.Models.Dtos
{
    public class ServiceCreateDto
    {
        public required string ServiceName { get; set; }
        public int? DurationInMinutes { get; set; }
    }

    public class ServiceReadDto
    {
        public Guid ServiceId { get; set; }
        public required string ServiceName { get; set; }
        public int DurationInMinutes { get; set; }
    }
}
