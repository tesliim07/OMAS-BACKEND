using System.ComponentModel.DataAnnotations;

namespace Medical_Appointment_System.Models
{
    public class Service
    {
        [Key]
        public Guid ServiceId { get; set; }
        public required string ServiceName { get; set; }
        public required string ServiceDescription { get; set; }
        public int DurationInMinutes { get; set; } = 30;

        // 1 service can have many availability slots
        public ICollection<AvailabilitySlot> AvailabilitySlots { get; set; } = new List<AvailabilitySlot>();

        // 1 service can have many appointments
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    }
}

