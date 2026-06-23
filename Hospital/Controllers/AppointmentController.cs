using Hospital.Application.DTOs;
using Hospital.Application.Interfaces.Repositories;
using Hospital.Application.Interfaces.Services;
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

        public AppointmentController(IAppointmentService appointmentService, IPatientRepository patientRepository)
        {
            _appointmentService = appointmentService;
            _patientRepository = patientRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _appointmentService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null)
                return NotFound(new { message = "Appointment not found" });

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
    }
}
