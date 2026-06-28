using AutoMapper;
using Hospital.Application.DTOs;
using Hospital.Application.Interfaces.Repositories;
using Hospital.Application.Interfaces.Services;
using Hospital.Domain.Entities;
using Hospital.Domain.Enums;
using Hospital.Domain.Exceptions;

namespace Hospital.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<int> BookAppointmentAsync(AppointmentDto dto, int patientId)
        {
            var conflict = await _appointmentRepository.GetByDoctorAndTime(dto.DoctorId, dto.AppointmentDate);
            if (conflict != null)
            {
                throw new BadRequestException("This doctor already has an appointment at that time.");
            }

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

        public async Task<bool> CancelAppointmentAsync(int id, int? callerPatientId, int? callerDoctorId, bool isAdmin)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
            {
                return false;
            }

            var allowed = isAdmin
                  || (callerPatientId.HasValue && appointment.PatientId == callerPatientId.Value)
                  || (callerDoctorId.HasValue && appointment.DoctorId == callerDoctorId.Value);

            if (!allowed)
            {
                throw new UnauthorizedAppException("You can only cancel your own appointments.");
            }
            appointment.Status = AppointmentStatus.Cancelled;
            await _appointmentRepository.UpdateAsync(appointment);
            return true;
        }

        public async Task<List<AppointmentDto>> GetAllAsync()
        {
            var appointments = await _appointmentRepository.GetAllAsync();
            return _mapper.Map<List<AppointmentDto>>(appointments);
        }

        public Task<List<AppointmentDto?>> GetByDoctorIdAsync(int id)
        {
            var appointments = await _appointmentRepository.GetByDoctorIdAsync(id);
            return _mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            return appointment == null ? null : _mapper.Map<AppointmentDto>(appointment);
        }

        public Task<List<AppointmentDto?>> GetByPatientIdAsync(int id)
        {
            var appointments = await _appointmentRepository.GetByPatientIdAsync(id);
            return _mapper.Map<List<AppointmentDto>>(appointments);
        }
    }
}
