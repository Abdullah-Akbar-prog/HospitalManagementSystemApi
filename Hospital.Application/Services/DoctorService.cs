using AutoMapper;
using Hospital.Application.DTOs;
using Hospital.Application.Interfaces.Repositories;
using Hospital.Application.Interfaces.Services;
using Hospital.Domain.Entities;
using Hospital.Domain.Exceptions;

namespace Hospital.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public DoctorService(IDoctorRepository doctorRepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }


        public async Task<int> CreateAsync(DoctorDto dto, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BadRequestException("A valid userId is required to create a doctor profile.");
            }

            var existing = await _doctorRepository.GetByUserIdAsync(userId);
            if (existing != null)
            {
                throw new BadRequestException("A doctor profile already exists for this user.");
            }

            var doctor = _mapper.Map<Doctor>(dto);
            doctor.UserId = userId;

            var result = await _doctorRepository.AddAsync(doctor);
            return result.Id;
        }

        public async Task<List<DoctorDto>> GetAllAsync()
        {
            var doctors = await _doctorRepository.GetAllAsync();
            return _mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<DoctorDto> GetByIdAsync(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Doctor {id} not found.");

            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task<bool> UpdateAsync(DoctorDto dto, string callerUserId, bool isAdmin)
        {
            var doctor = await _doctorRepository.GetByIdAsync(dto.Id);
            if (doctor == null) return false;

            if (!isAdmin && doctor.UserId != callerUserId)
            {
                throw new UnauthorizedAppException("You can only update your own doctor profile.");
            }

            _mapper.Map(dto, doctor);
            await _doctorRepository.UpdateAsync(doctor);
            return true;
        }

        public async Task<bool> DeleteAsync(int id) =>
            await _doctorRepository.DeleteAsync(id);

        public async Task<Doctor?> GetByUserIdAsync(string userId) =>
            await _doctorRepository.GetByUserIdAsync(userId);
    }
}
