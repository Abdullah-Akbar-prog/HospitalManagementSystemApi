using AutoMapper;
using Hospital.Application.DTOs;
using Hospital.Application.Interfaces.Repositories;
using Hospital.Application.Interfaces.Services;
using Hospital.Domain.Exceptions;

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

        public async Task<bool> DeleteAsync(int id, int? callerDoctorId, bool isAdmin)
        {
            var record = await _medicalRecordRepository.GetByIdAsync(id);
            if (record == null)
            {
                return false;
            }

            if (!isAdmin && (!callerDoctorId.HasValue || record.DoctorId != callerDoctorId.Value))
            {
                throw new UnauthorizedAppException("Only the doctor who created this record (or an Admin) can delete it.");
            }
            return await _medicalRecordRepository.DeleteAsync(id);
        }

        public async Task<MedicalRecordDto> GetByIdAsync(int id)
        {
            var record = await _medicalRecordRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Medical reccord {id} not found.");
            return _mapper.Map<MedicalRecordDto>(record);
        }

        public async Task<List<MedicalRecordDto>> GetByPatientIdAsync(int patientId)
        {
            var record = await _medicalRecordRepository.GetByPatientIdAsync(patientId);
            return _mapper.Map<List<MedicalRecordDto>>(record);
        }

        public async Task<bool> UpdateAsync(MedicalRecordDto dto, int? callerDoctorId, bool isAdmin)
        {
            var record = await _medicalRecordRepository.GetByIdAsync(dto.Id);
            if (record == null)
            {
                return false;
            }
            if (!isAdmin && (!callerDoctorId.HasValue || record.DoctorId != callerDoctorId.Value))
            {
                throw new UnauthorizedAppException("Only the doctor who created this record (or an Admin) can update it.");
            }
            _mapper.Map(dto, record);
            await _medicalRecordRepository.UpdateAsync(record);
            return true;
        }
    }
}
