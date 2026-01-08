using System.ComponentModel.DataAnnotations;

namespace Medical_Appointment_System.Models
{
    public class AvailabilitySlot
    {
        [Key]
        public Guid AvailabilitySlotId { get; set; }
        public DateTime SlotStartTime { get; set; }
        public DateTime SlotEndTime { get; set; }
        public bool IsBooked { get; set; } = false;

        //Each availability slot has 0 or 1 Appointment
        //public Appointment? Appointment { get; set; }

        //Each availability slot is linked to one service
        public Guid ServiceId { get; set; }
        public Service? Service { get; set; } //Navigation property

    }
}
