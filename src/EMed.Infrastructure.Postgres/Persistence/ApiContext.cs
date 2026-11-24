using EMed.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EMed.Infrastructure.Postgres.Persistence;

public class ApiContext : DbContext
{
    public DbSet<Appointment> Appointments { get; private set; }
    public DbSet<Doctor> Doctors { get; private set; }
    public DbSet<Patient> Patients { get; private set; }
    public DbSet<Schedule> Schedules { get; private set; }
    public DbSet<MedicalRecord> MedicalRecords { get; private set; }
    public DbSet<Speciality> Specialities { get; private set; }

    public ApiContext(DbContextOptions<ApiContext> options) 
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApiContext).Assembly);
        
    }
}