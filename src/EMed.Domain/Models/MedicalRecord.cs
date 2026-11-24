namespace EMed.Domain.Models;

public class MedicalRecord
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public string Conclusion { get; set; }
    public string Treatment { get; set; }
    
    public MedicalRecord(Guid appointmentId, string conclusion, string treatment)
    {
        
        if (string.IsNullOrWhiteSpace(conclusion))
            throw new ArgumentException("Doctor conclusion cannot be null or whitespace.", nameof(conclusion));
        
        if (appointmentId == Guid.Empty)
            throw new ArgumentException("Appointment id cannot be empty.", nameof(appointmentId));
        
        Id = Guid.CreateVersion7();
        Conclusion = conclusion;
        Treatment = treatment;
        AppointmentId = appointmentId;
    }

    private MedicalRecord()
    {
        
    }
   
}