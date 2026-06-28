using Hospital.Application.DTOs;
using Hospital.Application.Interfaces.Services;
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
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(await _patientService.GetAllAsync(pageNumber, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _patientService.GetByIdAsync(id);
            if (patient == null) return NotFound();

            return Ok(patient);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PatientDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var patient = await _patientService.CreateAsync(dto, userId);

            return Ok(new { id = patient, message = "Patient profile created successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PatientDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");

            var result = await _patientService.UpdateAsync(dto);

            if (!result) return NotFound();

            return Ok(new { message = "Updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _patientService.DeleteAsync(id);

            if (!result) return NotFound();

            return Ok(new { message = "Delete successfully" });
        }
    }
}
