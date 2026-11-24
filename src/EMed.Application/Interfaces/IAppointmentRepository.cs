using EMed.Domain.Models;

namespace EMed.Application.Interfaces;

public interface IAppointmentRepository : IRepository<Appointment, Guid>
{
    Task<List<Appointment>> GetByPatientId(Guid patientId, CancellationToken cancellationToken = default);
    Task<List<Appointment>> GetByDoctorIdWithTime(Guid doctorId,DateTime time, CancellationToken cancellationToken = default);
}