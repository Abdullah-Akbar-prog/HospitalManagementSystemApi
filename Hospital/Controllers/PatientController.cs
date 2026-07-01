using Hospital.Application.DTOs;
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
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(await _patientService.GetAllAsync(pageNumber, pageSize));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _patientService.GetByIdAsync(id);
            if (patient == null) return NotFound();

            return Ok(patient);
        }

        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Create(PatientDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAppException("User is not authenticated");

            var patient = await _patientService.CreateAsync(dto, userId);

            return Ok(new { id = patient, message = "Patient profile created successfully" });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Patient")]
        public async Task<IActionResult> Update(int id, PatientDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAppException("User is not authenticated.");
            var isAdmin = User.IsInRole(Roles.Admin);

            var result = await _patientService.UpdateAsync(dto, userId, isAdmin);
            if (!result) return NotFound();

            return Ok(new { message = "Updated successfully" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _patientService.DeleteAsync(id);

            if (!result) return NotFound();

            return Ok(new { message = "Delete successfully" });
        }

        [HttpGet("me")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMe()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAppException("User is not authenticated.");

            var patient = await _patientService.GetByUserIdAsync(userId)
                ?? throw new NotFoundException("Patient profile not found.");

            return Ok(patient);
        }
    }
}
