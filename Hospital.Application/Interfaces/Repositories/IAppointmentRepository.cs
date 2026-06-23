using Hospital.Domain.Entities;

namespace Hospital.Application.Interfaces.Repositories
{
    public interface IAppointmentRepository
    {
        Task<Appointment> AddAsync(Appointment appointment);
        Task<List<Appointment>> GetAllAsync();
        Task<Appointment?> GetByIdAsync(int id);
        Task<Appointment?> GetByDoctorAndTime(int doctorId, DateTime appointmentDate);
        Task UpdateAsync(Appointment appointment);
        Task<bool> DeleteAsync(int id);
    }
}
