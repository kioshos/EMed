
using EMed.Application.Interfaces;
using EMed.Domain.Models;
using EMed.Infrastructure.Postgres.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EMed.Infrastructure.Postgres.Repositories;

public class SpecialityRepository : ISpecialityRepository
{
    private readonly ApiContext _dbContext;

    public SpecialityRepository(ApiContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IEnumerable<Speciality>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Specialities.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Speciality?> GetByIdAsync(ushort id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Specialities.FirstOrDefaultAsync(sp => sp.Id == id, cancellationToken);
    }

    public async Task<List<Speciality?>> GetSpecialitiesByDoctorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Specialities
            .Where(d => d.DoctorId == id)
            .ToListAsync(cancellationToken);
    }

    public Task AddAsync(Speciality entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Speciality entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Speciality entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}