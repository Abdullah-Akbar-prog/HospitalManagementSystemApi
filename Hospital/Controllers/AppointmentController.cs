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
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;

        public AppointmentController(IAppointmentService appointmentService, IPatientRepository patientRepository, IDoctorRepository doctorRepository)
        {
            _appointmentService = appointmentService;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (User.IsInRole(Roles.Admin))
            {
                return Ok(await _appointmentService.GetAllAsync());
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAppException("User is not authenticated.");

            if (User.IsInRole(Roles.Doctor))
            {
                var doctor = await _doctorRepository.GetByUserIdAsync(userId);
                if (doctor == null) return Ok(new List<AppointmentDto>());
                return Ok(await _appointmentService.GetByDoctorIdAsync(doctor.Id));
            }
            var patient = await _patientRepository.GetByUserIdAsync(userId);
            if (patient == null) return Ok(new List<AppointmentDto>());
            return Ok(await _appointmentService.GetByPatientIdAsync(patient.Id));

        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null)
                return NotFound(new { message = "Appointment not found" });

            if (!await CallerOwnsAppointmentAsync(appointment))
            {
                return Forbid();
            }
            return Ok(appointment);
        }

        [HttpPost("book")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Book(AppointmentDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var patient = await _patientRepository.GetByUserIdAsync(userId);
            if (patient == null)
            {
                return BadRequest("Patient profile not found.");
            }

            var appointment = await _appointmentService.BookAppointmentAsync(dto, patient.Id);

            return CreatedAtAction(
                nameof(GetById),
                new { id = appointment },
                new { id = appointment, message = "Appointment booked successfully" });
        }

        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _appointmentService.CancelAppointmentAsync(id);

            if (result == null) return NotFound(new { message = "Appointment not found" });
            return Ok(new { message = "Appointment cancelled successfully" });
        }

        private async Task<bool> CallerOwnsAppointmentAsync(AppointmentDto appointment)
        {
            if (User.IsInRole(Roles.Admin))
            {
                return true;
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return false;

            if (User.IsInRole(Roles.Doctor))
            {
                var doctor = await _doctorRepository.GetByUserIdAsync(userId);
                return doctor != null && doctor.Id == appointment.DoctorId;
            }

            var patient = await _patientRepository.GetByUserIdAsync(userId);
            return patient != null && patient.Id == appointment.PatientId;

        }
    }
}
