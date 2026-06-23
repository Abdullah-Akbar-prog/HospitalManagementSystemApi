using Hospital.Application.DTOs;
using Hospital.Application.Interfaces.Repositories;
using Hospital.Application.Interfaces.Services;
using Hospital.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Hospital.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DoctorService(IDoctorRepository doctorRepository, IHttpContextAccessor httpContextAccessor)
        {
            _doctorRepository = doctorRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> CreateAsync(DoctorDto dto)
        {
            var userId = _httpContextAccessor.HttpContext?.
            User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                throw new Exception("User is not authenticated");
            }

            var doctor = new Doctor
            {
                UserId = userId,
                FullName = dto.FullName,
                Specialization = dto.Specialization,
                LicenseNumber = dto.LicenseNumber
            };
            var result = await _doctorRepository.AddAsync(doctor);
            return result.Id;
        }

        public async Task<List<DoctorDto>> GetAllAsync()
        {
            var doctors = await _doctorRepository.GetAllAsync();

            return doctors.Select(d => new DoctorDto
            {
                Id = d.Id,
                FullName = d.FullName,
                Specialization = d.Specialization,
                LicenseNumber = d.LicenseNumber
            }).ToList();
        }

        public async Task<DoctorDto> GetByIdAsync(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null)
            {
                return null;
            }

            return new DoctorDto
            {
                Id = doctor.Id,
                FullName = doctor.FullName,
                Specialization = doctor.Specialization,
                LicenseNumber = doctor.LicenseNumber
            };
        }

        public async Task<bool> UpdateAsync(DoctorDto dto)
        {
            var doctor = await _doctorRepository.GetByIdAsync(dto.Id);
            if (doctor == null) return false;

            doctor.FullName = dto.FullName;
            doctor.Specialization = dto.Specialization;
            doctor.LicenseNumber = dto.LicenseNumber;
            await _doctorRepository.UpdateAsync(doctor);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _doctorRepository.DeleteAsync(id);
        }
    }
}
