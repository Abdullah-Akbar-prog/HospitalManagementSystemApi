using Hospital.Domain.Entities;

namespace Hospital.Application.Interfaces.Repositories
{
    public interface IMedicalRecordRepository : IRepository<MedicalRecord>
    {
        Task<List<MedicalRecord>> GetByPatientIdAsync(int patientId);
    }
}
