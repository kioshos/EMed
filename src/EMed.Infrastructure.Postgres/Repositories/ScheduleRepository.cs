

using EMed.Application.Interfaces;
using EMed.Domain.Models;
using EMed.Infrastructure.Postgres.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EMed.Infrastructure.Postgres.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    private readonly ApiContext _dbContext;

    public ScheduleRepository(ApiContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IEnumerable<Schedule>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Schedules.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Schedule?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Schedules.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task AddAsync(Schedule entity, CancellationToken cancellationToken = default)
    {
       _dbContext.Schedules.Add(entity);
       await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateAsync(Schedule entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Schedules.Update(entity);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteAsync(Schedule entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Schedules.Remove(entity);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Schedule> GetScheduleByDoctorIdAsync(Guid doctorId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Schedules.AsNoTracking()
            .FirstOrDefaultAsync(s => s.DoctorId == doctorId, cancellationToken);
    }
}