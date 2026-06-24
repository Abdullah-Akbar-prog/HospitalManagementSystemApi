using AutoMapper;
using Hospital.Application.DTOs;
using Hospital.Application.Interfaces.Repositories;
using Hospital.Application.Interfaces.Services;
using Hospital.Domain.Entities;

namespace Hospital.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(PatientDto dto, string userId)
        {
            var patient = _mapper.Map<Patient>(dto);
            patient.UserId = userId;

            var result = await _patientRepository.AddAsync(patient);

            return result.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _patientRepository.DeleteAsync(id);
        }

        public async Task<List<PatientDto>> GetAllAsync()
        {
            var patient = await _patientRepository.GetAllAsync();
            return _mapper.Map<List<PatientDto>>(patient);
        }

        public async Task<PatientDto> GetByIdAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Patient {id} not found.");

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<Patient?> GetByUserIdAsync(string userId)
        {
            return await _patientRepository.GetByUserIdAsync(userId);
        }

        public async Task<bool> UpdateAsync(PatientDto dto)
        {
            var patient = await _patientRepository.GetByIdAsync(dto.Id);
            if (patient == null) return false;

            _mapper.Map(dto, patient);

            await _patientRepository.UpdateAsync(patient);
            return true;
        }
    }
}
