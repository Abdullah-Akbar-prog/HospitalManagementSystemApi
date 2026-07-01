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
        public async Task<IActionResult> Create(string userId, DoctorDto dto)
        {
            var doctor = await _doctorService.CreateAsync(dto, userId);
            return Ok(new { id = doctor, message = "Doctor create successfully" });
        }

        [HttpGet("me")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetMe()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAppException("User is not authenticated");

            var doctor = await _doctorService.GetByUserIdAsync(userId)
                ?? throw new NotFoundException("Doctor profile not found");
            return Ok(doctor);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Update(int id, DoctorDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("Id mismatch");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAppException("User is not authenticated.");
            var isAdmin = User.IsInRole(Roles.Admin);

            var updated = await _doctorService.UpdateAsync(dto, userId, isAdmin);
            if (!updated) return NotFound(new { message = "Doctor not found" });
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
