using Hospital.Application.DTOs;
using Hospital.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _doctorService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);
            if (doctor == null) return NotFound();
            return Ok(doctor);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(DoctorDto dto)
        {
            var doctor = await _doctorService.CreateAsync(dto);
            return Ok(new { message = "Doctor create successfully" });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, DoctorDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("Id mismatch");
            }

            var doctor = await _doctorService.UpdateAsync(dto);
            if (!doctor) return NotFound(new { message = "Doctor not found" });
            return Ok(new { message = "Doctor updated successfully" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _doctorService.DeleteAsync(id);
            if (!doctor) return NotFound(new { message = "Doctor not found" });
            return Ok(new { message = "Doctor delete successfully" });
        }
    }
}
