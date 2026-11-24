using EMed.Application.Interfaces;
using EMed.Domain.Models;
using EMed.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EMed.Infrastructure.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly ApiContext _dbContext;

    public DoctorRepository(ApiContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IEnumerable<Doctor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
      return await _dbContext.Doctors
           .AsNoTracking()
           .ToListAsync(cancellationToken);
    }

    public async Task<Doctor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Doctors
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task AddAsync(Doctor entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Doctors.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateAsync(Doctor entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Doctors.Update(entity);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Doctor entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Doctors.Remove(entity);
       await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Doctor>> GetDoctorsByIds(List<Guid> doctorIds, CancellationToken cancellationToken)
    {
        return await _dbContext.Doctors.Where(p => doctorIds.Contains(p.Id)).ToListAsync(cancellationToken);
    }
}