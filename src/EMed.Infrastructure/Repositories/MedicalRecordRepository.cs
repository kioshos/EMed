using EMed.Application.Interfaces;
using EMed.Domain.Models;
using EMed.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EMed.Infrastructure.Repositories;

public class MedicalRecordRepository : IRepository<MedicalRecord, Guid>
{
    private readonly ApiContext _dbContext;
    public MedicalRecordRepository(ApiContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<MedicalRecord>> GetAllAsync(CancellationToken cancellationToken = default)
    {
       return await _dbContext.MedicalRecords
           .AsNoTracking()
           .ToListAsync(cancellationToken);
    }

    public async Task<MedicalRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.MedicalRecords
            .AsNoTracking()
            .FirstOrDefaultAsync(mr => mr.Id == id);
        
    }

    public async Task AddAsync(MedicalRecord entity, CancellationToken cancellationToken = default)
    {
        _dbContext.MedicalRecords.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(MedicalRecord entity, CancellationToken cancellationToken = default)
    {
         _dbContext.MedicalRecords.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteAsync(MedicalRecord entity, CancellationToken cancellationToken = default)
    {
        _dbContext.MedicalRecords.Remove(entity);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}