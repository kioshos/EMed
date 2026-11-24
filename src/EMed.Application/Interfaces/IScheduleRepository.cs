using EMed.Domain.Models;

namespace EMed.Application.Interfaces;

public interface IScheduleRepository : IRepository<Schedule,Guid>
{
    Task<Schedule> GetScheduleByDoctorIdAsync(Guid doctorId, CancellationToken cancellationToken = default);
}