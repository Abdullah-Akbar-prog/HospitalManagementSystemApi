using System.ComponentModel.DataAnnotations;

namespace Hospital.Application.DTOs
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        [Required]
        public int DoctorId { get; set; }
        [Required]
        public DateTime AppointmentDate { get; set; }
        [Required, StringLength(500)]
        public string? Reason { get; set; }
        public string? Status { get; set; }
    }
}
