using System.ComponentModel.DataAnnotations;

namespace Hospital.Application.DTOs
{
    public class PatientDto
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        [Required, StringLength(100)]
        public string? FullName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string? Gender { get; set; }
        [Required, Phone]
        public string? Phone { get; set; }
    }
}
