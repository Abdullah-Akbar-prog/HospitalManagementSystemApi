using AutoMapper;
using Hospital.Application.DTOs;
using Hospital.Application.Interfaces.Repositories;
using Hospital.Application.Interfaces.Services;

namespace Hospital.Application.Services
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IMapper _mapper;

        public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository, IMapper mapper)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(MedicalRecordDto dto, int doctorId)
        {
            var record = _mapper.Map<MedicalRecordDto>(dto);
            record.DoctorId = doctorId;

            var result = await _medicalRecordRepository.AddAsync(record);
            return result;
        }

        public Task<bool> DeleteAsync(int id, int? callerDoctorId, bool isAdmin)
        {
            throw new NotImplementedException();
        }

        public Task<MedicalRecordDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<MedicalRecordDto>> GetByPatientIdAsync(int patientId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(MedicalRecordDto dto, int? callerDoctorId, bool isAdmin)
        {
            throw new NotImplementedException();
        }
    }
}
