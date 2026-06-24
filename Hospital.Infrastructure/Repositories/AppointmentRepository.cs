using Hospital.Application.Interfaces.Repositories;
using Hospital.Domain.Entities;
using Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Repositories
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<List<Appointment>> GetAllAsync()
        {
            return await _dbContext.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .ToListAsync();
        }

        public async Task<Appointment?> GetByDoctorAndTime(int doctorId, DateTime appointmentDate)
        {
            return await _dbContext.Appointments
            .FirstOrDefaultAsync(a => a.DoctorId == doctorId && a.AppointmentDate == appointmentDate);
        }

        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _dbContext.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
