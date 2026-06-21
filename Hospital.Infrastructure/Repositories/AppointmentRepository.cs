using Hospital.Application.Interfaces;
using Hospital.Domain.Entities;
using Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AppointmentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Appointment> AddAsync(Appointment appointment)
        {
            await _dbContext.Appointments.AddAsync(appointment);
            await _dbContext.SaveChangesAsync();
            return appointment;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var appointment = await _dbContext.Appointments.FindAsync(id);
            if (appointment == null)
                return false;

            _dbContext.Appointments.Remove(appointment);
            await _dbContext.SaveChangesAsync();
            return true;
        }

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

        public async Task UpdateAsync(Appointment appointment)
        {
            _dbContext.Appointments.Update(appointment);
            await _dbContext.SaveChangesAsync();
        }
    }
}
