using EMed.Domain.Models;

namespace EMed.Application.Interfaces;

public interface IDoctorRepository : IRepository<Doctor, Guid>
{
    Task<IEnumerable<Doctor>> GetDoctorsByIds(List<Guid> doctorIds, CancellationToken cancellationToken);
}