using Medical_Appointment_System.Contexts;
using Medical_Appointment_System.Models;
using Microsoft.EntityFrameworkCore;
using OnlineMedicalAppointmentSystem.Repositories.Interfaces;

namespace OnlineMedicalAppointmentSystem.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly MedicalAppointmentSystemDbContext _context;
        public AppointmentRepository(MedicalAppointmentSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAppointment(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            var availabiltitySlot = await _context.AvailabilitySlots.FindAsync(appointment.AvailabilitySlotId);
            if(availabiltitySlot == null)
            {
                return Guid.Empty;
            }
            availabiltitySlot.IsBooked = true;
            await _context.SaveChangesAsync();
            return appointment.AppointmentId;
        }

        public async Task<Appointment> GetAppointmentById(Guid appointmentId)
        {
            return await _context.Appointments.FindAsync(appointmentId);
        }

        public async Task<List<Appointment>> GetAllAppointmentsByDate(DateTime dateSelected)
        {
            var start = dateSelected.Date;
            var appointments = await _context.Appointments
                .Where(a => a.AppointmentDateTime.Date == start)
                .OrderBy(a => a.AppointmentDateTime)
                .ToListAsync();
            return appointments;
        }

        public async Task<List<Appointment>> GetAllPendingAppointments(int daysToAppointment)
        {
            var todaysDate = DateTime.Today;
            var pendingAppointments = await _context.Appointments
                .Where(a => a.AppointmentDateTime.Date > todaysDate && a.AppointmentDateTime.Date <= todaysDate.AddDays(daysToAppointment))
                .ToListAsync();
            return pendingAppointments;
        }

        public async Task<List<Appointment>> GetAllAppointments()
        {
            return await _context.Appointments.ToListAsync();
        }

        //public async Task<bool> RescheduleAppointment(Guid appointmentId, DateTime newDateTime)
        //{
        //    var appointment = await _context.Appointments.FindAsync(appointmentId);
        //    if (appointment != null)
        //    {
        //        appointment.AppointmentDateTime = newDateTime;
        //        _context.Appointments.Update(appointment);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    return false;
        //}

        public async Task<bool> DeleteAppointment(Guid appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
