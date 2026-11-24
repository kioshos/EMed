

using EMed.Application.Interfaces;
using EMed.Domain.Models;
using EMed.Infrastructure.Postgres.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EMed.Infrastructure.Postgres.Repositories;

public class PatientRepository : IRepository<Patient, Guid>
{
    private readonly ApiContext _dbContext;

    public PatientRepository(ApiContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Patients
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Patient?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task AddAsync(Patient entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Patients.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Patient entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Patients.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Patient entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Patients.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}