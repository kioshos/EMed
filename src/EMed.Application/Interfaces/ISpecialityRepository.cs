using EMed.Domain.Models;

namespace EMed.Application.Interfaces;

public interface ISpecialityRepository : IRepository<Speciality, ushort>
{
   Task<Speciality?> GetByIdAsync(ushort id, CancellationToken cancellationToken = default);
   Task<List<Speciality?>> GetSpecialitiesByDoctorIdAsync(Guid id, CancellationToken cancellationToken = default);
}