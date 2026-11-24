using EMed.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMed.Infrastructure.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("Doctors");
        
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).HasMaxLength(128).IsRequired();
    }
}