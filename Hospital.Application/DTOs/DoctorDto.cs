using System.ComponentModel.DataAnnotations;

namespace Hospital.Application.DTOs
{
    public class DoctorDto
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string? FullName { get; set; }

        [Required, StringLength(100)]
        public string? Specialization { get; set; }

        [Required, StringLength(100)]
        public string? LicenseNumber { get; set; }
    }
}
