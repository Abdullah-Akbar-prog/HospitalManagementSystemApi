using Hospital.Application.Interfaces.Repositories;
using Hospital.Domain.Entities;
using Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Repositories
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(ApplicationDbContext dbContext) : base(dbContext) { }
        public async Task<Doctor?> GetByUserIdAsync(string userId)
        {
            return await _dbContext.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
        }
    }
}
