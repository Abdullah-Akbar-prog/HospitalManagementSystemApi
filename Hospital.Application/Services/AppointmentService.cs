using Hospital.Application.DTOs;
using Hospital.Application.Interfaces.Repositories;
using Hospital.Application.Interfaces.Services;
using Hospital.Domain.Entities;
using Hospital.Domain.Enums;

namespace Hospital.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<int> BookAppointmentAsync(AppointmentDto dto, int patientId)
        {
            var appointment = new Appointment
            {
                PatientId = patientId,
                DoctorId = dto.DoctorId,
                AppointmentDate = dto.AppointmentDate,
                Reason = dto.Reason,
                Status = AppointmentStatus.Scheduled
            };
            var result = await _appointmentRepository.AddAsync(appointment);
            return result.Id;
        }

        public async Task<bool> CancelAppointmentAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
            {
                return false;
            }
            appointment.Status = AppointmentStatus.Cancelled;
            await _appointmentRepository.UpdateAsync(appointment);
            return true;
        }

        public async Task<List<AppointmentDto>> GetAllAsync()
        {
            var appointments = await _appointmentRepository.GetAllAsync();
            return appointments.Select(a => new AppointmentDto
            {
                Id = a.Id,
                DoctorId = a.DoctorId,
                AppointmentDate = a.AppointmentDate,
                Reason = a.Reason,
                Status = a.Status.ToString()
            }).ToList();
        }

        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            var appointments = await _appointmentRepository.GetByIdAsync(id);
            if (appointments == null) return null;

            return new AppointmentDto
            {
                Id = appointments.Id,
                DoctorId = appointments.DoctorId,
                AppointmentDate = appointments.AppointmentDate,
                Reason = appointments.Reason,
                Status = appointments.Status.ToString()
            };
        }
    }
}
