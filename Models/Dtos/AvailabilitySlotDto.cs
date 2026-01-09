namespace OnlineMedicalAppointmentSystem.Models.Dtos
{
    public class AvailabilitySlotReadDto
    {
        public Guid AvailabilitySlotId { get; set; }
        public required DateTime SlotStartTime { get; set; }
        public required DateTime SlotEndTime { get; set; }
        public required bool IsBooked { get; set; }
    }

    public class AvailabilitySlotWithServiceIdReadDto
    {
        public Guid AvailabilitySlotId { get; set; }
        public required DateTime SlotStartTime { get; set; }
        public required DateTime SlotEndTime { get; set; }
        public required bool IsBooked { get; set; }
        public required Guid ServiceId { get; set; }
    }
}
