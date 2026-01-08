using System.ComponentModel.DataAnnotations;

namespace Medical_Appointment_System.Models
{
    public class Appointment
    {
        [Key]
        public Guid AppointmentId { get; set; }
        public required string PatientFirstName { get; set; }
        public required string PatientLastName { get; set; }
        public string? PatientEmail { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public required bool AppointmentBookedStatus { get; set; }

        //Each appointment is linked to one service
        public Guid ServiceId { get; set; }
        public Service? Service { get; set; } //Navigation property

        //Each appointment is linked to one availability slot
        public Guid AvailabilitySlotId { get; set; }
        public AvailabilitySlot? AvailabilitySlot { get; set; } //Navigation property

    }
}
