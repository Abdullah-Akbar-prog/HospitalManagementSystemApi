using Hospital.Application.Interfaces.Repositories;
using Hospital.Domain.Entities;
using Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Repositories
{
    public class MedicalRecordRepository : Repository<MedicalRecord>, IMedicalRecordRepository
    {
        public MedicalRecordRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<List<MedicalRecord>> GetByPatientIdAsync(int patientId)
        {
            return await _dbContext.MedicalRecords
                .Where(m => m.PatientId == patientId)
                .Include(m => m.Doctor).ToListAsync();
        }
    }
}
