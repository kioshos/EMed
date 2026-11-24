namespace EMed.Domain.Models;

public class Appointment
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public DateTime Time { get; set; }
    public string Description { get; set; }

    private Appointment()
    {
        
    }
    public Appointment( Guid patientId, Guid doctorId, DateTime time, string description)
    {
        if (patientId == Guid.Empty || doctorId == Guid.Empty)
            throw new ArgumentException("Invalid patient or doctor id");
        
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Invalid description");
        
        if (time.Date.Day < DateTime.Now.Day)
            throw new ArgumentException("Invalid time");
        
        Id = Guid.CreateVersion7();
        PatientId = patientId;
        DoctorId = doctorId;
        Time = time;
        Description = description;
    }
}