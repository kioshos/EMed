using EMed.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMed.Infrastructure.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(p => p.Address)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(p => p.Age)
            .IsRequired();
        builder.Property(p => p.TelephoneNumber)
            .IsRequired();
    }
}