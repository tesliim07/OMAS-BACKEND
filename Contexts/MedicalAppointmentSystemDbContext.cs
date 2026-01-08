using Medical_Appointment_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Medical_Appointment_System.Contexts
{
    public class MedicalAppointmentSystemDbContext : DbContext
    {
        public MedicalAppointmentSystemDbContext(DbContextOptions<MedicalAppointmentSystemDbContext> options) : base(options)
        {
        }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<AvailabilitySlot> AvailabilitySlots { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Service>()
                .HasIndex(s => s.ServiceName)
                .IsUnique();
        }
    }
}
