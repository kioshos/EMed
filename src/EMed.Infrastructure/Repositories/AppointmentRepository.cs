using EMed.Application.Interfaces;
using EMed.Domain.Models;
using EMed.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EMed.Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ApiContext _dbContext;
    
    public AppointmentRepository(ApiContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<Appointment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Appointments
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Appointment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Appointments.AsNoTracking()
            .FirstAsync(a => a.Id == id, cancellationToken);
        
    }

    public async Task AddAsync(Appointment entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Appointments.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Appointment entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Appointments.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Appointment entity, CancellationToken cancellationToken = default)
    {
         _dbContext.Appointments.Remove(entity);
         await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Appointment>> GetByPatientId(Guid patientId, CancellationToken cancellationToken = default)
    {
       return await _dbContext.Appointments
           .Where(app => app.PatientId == patientId)
           .ToListAsync(cancellationToken);
    }

    public async Task<List<Appointment>> GetByDoctorId(Guid doctorId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Appointments
            .Where(app => app.DoctorId == doctorId)
            .ToListAsync(cancellationToken);
    }
    public async Task<List<Appointment>> GetByDoctorIdWithTime(Guid doctorId, DateTime time, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Appointments
            .Where(app => app.DoctorId == doctorId &&  app.Time.Date == time.Date)
            .ToListAsync(cancellationToken);
    }
}