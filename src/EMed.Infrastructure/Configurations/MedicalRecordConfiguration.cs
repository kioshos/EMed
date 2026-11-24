using EMed.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMed.Infrastructure.Configurations;

public class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecord>
{
    public void Configure(EntityTypeBuilder<MedicalRecord> builder)
    {
        builder.ToTable("MedicalRecords");
        
        builder.HasKey(d => d.Id);
        builder.Property(d => d.AppointmentId).IsRequired();
        builder.Property(d => d.Conclusion).IsRequired();
        
        builder.HasOne<Appointment>().WithOne()
            .HasForeignKey<MedicalRecord>(d => d.AppointmentId);
        
    }
}