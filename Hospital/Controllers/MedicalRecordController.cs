using Hospital.Application.DTOs;
using Hospital.Application.Interfaces.Repositories;
using Hospital.Application.Interfaces.Services;
using Hospital.Domain.Common;
using Hospital.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;

        public MedicalRecordController(IMedicalRecordService medicalRecordService, IDoctorRepository doctorRepository, IPatientRepository patientRepository)
        {
            _medicalRecordService = medicalRecordService;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            if (User.IsInRole(Roles.Patient))
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAppException("User is not authenticated.");

                var patient = await _patientRepository.GetByUserIdAsync(userid);
                if (patient == null || patient.Id != patientId)
                {
                    return Forbid();
                }
            }
            return Ok(await _medicalRecordService.GetByPatientIdAsync(patientId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _medicalRecordService.GetByIdAsync(id);
            if (User.IsInRole(Roles.Patient))
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAppException("User is not authenticated.");

                var patient = await _patientRepository.GetByUserIdAsync(userid);
                if (patient == null || patient.Id != record.PatientId)
                {
                    return Forbid();
                }
            }
            return Ok(record);
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Create(MedicalRecordDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                 ?? throw new UnauthorizedAppException("User is not authenticated.");
            var doctor = await _doctorRepository.GetByUserIdAsync(userId);
            if (doctor == null) return BadRequest("Doctor profilt not found");

            var recordId = await _medicalRecordService.CreateAsync(dto, doctor.Id);
            return CreatedAtAction(nameof(GetById), new { id = recordId }, new { id = recordId });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Update(int id, MedicalRecordDto dto)
        {
            if (id != dto.Id) return BadRequest("Id mismatch");

            var (callerDoctorId, isAdmin) = await ResolveCallerAsync();
            var updated = await _medicalRecordService.UpdateAsync(dto, callerDoctorId, isAdmin);
            if (!updated == null) return NotFound();
            return Ok(new { message = "Medical record updated successfully" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Delete(int id)
        {
            var (callerDoctorId, isAdmin) = await ResolveCallerAsync();
            var deleted = await _medicalRecordService.DeleteAsync(id, callerDoctorId, isAdmin);
            if (!deleted) return NotFound();
            return Ok(new { message = "Medical record deleted successfully" });
        }
        private async Task<(int? callerDoctorId, bool isAdmin)> ResolveCallerAsync()
        {
            var isAdmin = User.IsInRole(Roles.Admin);
            if (isAdmin) return (null, true);

            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAppException("USer is not authenticated.");

            var doctor = await _doctorRepository.GetByUserIdAsync(UserId);
            return (doctor?.Id, false);
        }
    }
}
