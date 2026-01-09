using Medical_Appointment_System.Contexts;
using Medical_Appointment_System.Models;
using Microsoft.EntityFrameworkCore;
using OnlineMedicalAppointmentSystem.Repositories.Interfaces;

namespace OnlineMedicalAppointmentSystem.Repositories
{
    public class AvailabilitySlotRepository : IAvailabilitySlotRepository
    {
        private readonly MedicalAppointmentSystemDbContext _context;
        public AvailabilitySlotRepository(MedicalAppointmentSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAvailabilitySlot(AvailabilitySlot availabilitySlot)
        {
            await _context.AvailabilitySlots.AddAsync(availabilitySlot);
            await _context.SaveChangesAsync();
            return availabilitySlot.AvailabilitySlotId;
        }

        public async Task<AvailabilitySlot> GetAvailabilitySlotById(Guid availabilitySlotId)
        {
            var availabiltySlot = await _context.AvailabilitySlots.FindAsync(availabilitySlotId);
            if (availabiltySlot != null)
            {
                return availabiltySlot;
            }
            return null;
        }

        public async Task<AvailabilitySlot> GetAvailabilitySlotByDate(DateTime dateTime)
        {
            var availabiltySlot = await _context.AvailabilitySlots.
                FirstOrDefaultAsync(slot => slot.SlotStartTime == dateTime);
            if (availabiltySlot != null)
            {
                return availabiltySlot;
            }
            return null;
        }

        public async Task<List<AvailabilitySlot>> GetAllAvailabilitySlotsByDateAndServiceName(DateTime dateSelected, string serviceName)
        {
            var start = dateSelected.Date;
            var end = start.AddDays(1);
            var availabilitySlots = await _context.AvailabilitySlots
                .Where(slot =>slot.Service.ServiceName == serviceName && slot.SlotStartTime >= start && slot.SlotStartTime < end)
                .OrderBy(slot => slot.SlotStartTime)
                .ToListAsync();
            return availabilitySlots;
        }

        public async Task<bool> DeleteAvailabilitySlot(Guid availabilitySlotId)
        {
            var availabilitySlot = await _context.AvailabilitySlots.FindAsync(availabilitySlotId);
            if (availabilitySlot != null)
            {
                _context.AvailabilitySlots.Remove(availabilitySlot);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
