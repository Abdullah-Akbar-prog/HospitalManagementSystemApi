using System.ComponentModel.DataAnnotations;

namespace Hospital.Application.DTOs
{
    public class MedicalRecordDto
    {
        public int Id { get; set; }
        [Required]
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        [Required, StringLength(500)]
        public string? Diagnosis { get; set; }
        [Required, StringLength(500)]
        public string? Prescription { get; set; }
        [Required, StringLength(1000)]
        public string? Notes { get; set; }
    }
}
