using Hospital.Application.Interfaces.Repositories;
using Hospital.Domain.Entities;
using Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Repositories
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<int> CountTodayAsync()
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            return await _dbContext.Appointments
                .CountAsync(a => a.AppointmentDate >= today && a.AppointmentDate < tomorrow);
        }
        public override async Task<List<Appointment>> GetAllAsync()
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

        public async Task<List<Appointment>> GetByDoctorIdAsync(int doctorId)
        {
            return await _dbContext.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == doctorId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public override async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _dbContext.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Appointment>> GetByPatientIdAsync(int patientId)
        {
            return await _dbContext.Appointments
             .Include(a => a.Patient)
             .Include(a => a.Doctor)
             .Where(a => a.PatientId == patientId)
             .OrderByDescending(a => a.AppointmentDate)
             .ToListAsync();
        }


    }
}
