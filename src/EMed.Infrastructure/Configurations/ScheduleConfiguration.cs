using EMed.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMed.Infrastructure.Configurations;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.ToTable("Schedule");
        
        builder.HasKey(p => p.Id);
        builder.Property(p => p.StartTime)
            .IsRequired();
        builder.Property(p => p.EndTime)
            .IsRequired();

        builder.HasOne<Doctor>().WithOne()
            .HasForeignKey<Schedule>(s => s.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}