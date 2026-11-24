using EMed.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMed.Infrastructure.Postgres.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");
        
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Time).IsRequired();
        builder.Property(e => e.Description).IsRequired();

        builder.HasOne<Patient>().WithMany().HasForeignKey(e => e.PatientId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<Doctor>().WithMany().HasForeignKey(e => e.DoctorId).OnDelete(DeleteBehavior.Cascade);
    }
}