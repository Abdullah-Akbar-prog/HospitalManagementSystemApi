using Hospital.Domain.Common;

namespace Hospital.Domain.Entities
{
    public class Patient : BaseEntity
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }

        public List<Appointment> Appointments { get; set; }
        public List<MedicalRecord> MedicalRecords { get; set; }
    }
}
