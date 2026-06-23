using Hospital.Application.DTOs;
using Hospital.Application.Interfaces.Repositories;
using Hospital.Application.Interfaces.Services;
using Hospital.Domain.Entities;

namespace Hospital.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<int> CreateAsync(PatientDto dto, string userId)
        {
            var patient = new Patient
            {
                UserId = userId,
                FullName = dto.FullName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Phone = dto.Phone
            };

            var result = await _patientRepository.AddAsync(patient);

            return result.Id;
        }

        public async Task<List<PatientDto>> GetAllAsync()
        {
            var patient = await _patientRepository.GetAllAsync();
            return patient.Select(p => new PatientDto
            {
                Id = p.Id,
                UserId = p.UserId,
                FullName = p.FullName,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender,
                Phone = p.Phone
            }).ToList();
        }

        public async Task<PatientDto> GetByIdAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null) return null;

            return new PatientDto
            {
                Id = patient.Id,
                UserId = patient.UserId,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                Phone = patient.Phone
            };
        }

        public async Task<bool> UpdateAsync(PatientDto dto)
        {
            var patient = await _patientRepository.GetByIdAsync(dto.Id);
            if (patient == null) return false;

            patient.FullName = dto.FullName;
            patient.DateOfBirth = dto.DateOfBirth;
            patient.Gender = dto.Gender;
            patient.Phone = dto.Phone;

            await _patientRepository.UpdateAsync(patient);
            return true;
        }
    }
}
