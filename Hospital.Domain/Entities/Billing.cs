using Hospital.Domain.Common;

namespace Hospital.Domain.Entities
{
    public class Billing : BaseEntity
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public string PaymentMethod { get; set; }
    }
}
