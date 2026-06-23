using Hospital.Application.Interfaces.Repositories;
using Hospital.Domain.Entities;
using Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PatientRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Patient> AddAsync(Patient patient)
        {
            await _dbContext.AddAsync(patient);
            await _dbContext.SaveChangesAsync();
            return patient;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var patient = await _dbContext.Patients.FindAsync(id);
            if (patient == null)
            {
                return false;
            }
            _dbContext.Patients.Remove(patient);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<Patient>> GetAllAsync()
        {
            return await _dbContext.Patients.ToListAsync();
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await _dbContext.Patients.FindAsync(id);
        }

        public async Task<Patient?> GetByUserIdAsync(string id)
        {
            return await _dbContext.Patients.FirstOrDefaultAsync(p => p.UserId == id);
        }

        public async Task UpdateAsync(Patient patient)
        {
            _dbContext.Patients.Update(patient);
            await _dbContext.SaveChangesAsync();

        }
    }
}
