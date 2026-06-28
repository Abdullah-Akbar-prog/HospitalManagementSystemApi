using Hospital.Domain.Entities;

namespace Hospital.Application.Interfaces.Repositories
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<Appointment?> GetByDoctorAndTime(int doctorId, DateTime appointmentDate);
        Task<Appointment?> GetByPatientIdAsync(int doctorId, DateTime appointmentDate);
        Task<Appointment?> GetByDoctorIdAsync(int doctorId, DateTime appointmentDate);

    }
}
