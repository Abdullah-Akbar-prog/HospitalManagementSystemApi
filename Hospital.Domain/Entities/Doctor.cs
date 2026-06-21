using Hospital.Domain.Common;

namespace Hospital.Domain.Entities
{
    public class Doctor : BaseEntity
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Specialization { get; set; }
        public string LicenseNumber { get; set; }

        public List<Appointment> Appointments { get; set; }
    }
}
