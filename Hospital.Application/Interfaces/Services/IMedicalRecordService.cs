using Hospital.Application.DTOs;

namespace Hospital.Application.Interfaces.Services
{
    public interface IMedicalRecordService
    {
        Task<List<MedicalRecordDto>> GetByPatientIdAsync(int patientId);
        Task<MedicalRecordDto> GetByIdAsync(int id);
        Task<int> CreateAsync(MedicalRecordDto dto, int doctorId);
        Task<bool> UpdateAsync(MedicalRecordDto dto, int? callerDoctorId, bool isAdmin);
        Task<bool> DeleteAsync(int id, int? callerDoctorId, bool isAdmin);
    }
}
