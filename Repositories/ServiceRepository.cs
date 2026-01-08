using Medical_Appointment_System.Contexts;
using Medical_Appointment_System.Models;
using Microsoft.EntityFrameworkCore;
using OnlineMedicalAppointmentSystem.Repositories.Interfaces;

namespace OnlineMedicalAppointmentSystem.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly MedicalAppointmentSystemDbContext _context;
        public ServiceRepository(MedicalAppointmentSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateService(Service service)
        {
            //do a try catch block
            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
            return service.ServiceId;
        }
        public async Task<Service> GetServiceById(Guid serviceId)
        {
            var service = await _context.Services.FindAsync(serviceId);
            return service;
        }
        public async Task<Service> GetServiceByName(string serviceName)
        {
            var service = await _context.Services
                .Where(s => s.ServiceName == serviceName)
                .FirstOrDefaultAsync();
            return service;
        }

        public async Task<List<Service>> GetAllServices()
        {
            var services = await _context.Services.ToListAsync();
            return services;
        }

        public async Task<bool> DeleteService(Guid serviceId)
        {
            var service = await _context.Services.FindAsync(serviceId);
            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
