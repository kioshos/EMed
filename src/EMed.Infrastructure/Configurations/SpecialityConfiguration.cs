using EMed.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMed.Infrastructure.Configurations;

public class SpecialityConfiguration : IEntityTypeConfiguration<Speciality>
{
    public void Configure(EntityTypeBuilder<Speciality> builder)
    {
        builder.ToTable("Specialities");
        
        builder.HasKey(sp => sp.Id);
        
        builder.Property(sp => sp.SpecialityName)
            .HasMaxLength(128)
            .IsRequired();
    }
}