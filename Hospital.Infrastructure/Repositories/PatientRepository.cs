using Hospital.Application.Interfaces.Repositories;
using Hospital.Domain.Entities;
using Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Repositories
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<Patient?> GetByUserIdAsync(string userId) =>
            await _dbContext.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
    }
}