using EMed.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMed.Infrastructure.Postgres.Configurations;

public class DoctorSpecialityConfiguration : IEntityTypeConfiguration<DoctorSpeciality>
{
    public void Configure(EntityTypeBuilder<DoctorSpeciality> builder)
    {
        builder.ToTable("DoctorSpeciality");
        
        builder.HasKey(x => new { x.DoctorId, x.SpecialityId });
        
        builder.HasOne<Doctor>()
            .WithMany()
            .HasForeignKey(x => x.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne<Speciality>()
            .WithMany()
            .HasForeignKey(x => x.SpecialityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}