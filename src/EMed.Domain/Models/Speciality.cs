namespace EMed.Domain.Models;

public class Speciality
{
    public ushort Id { get; set; }
    public Guid DoctorId { get; set; }
    public string SpecialityName { get; set; }
    
    public Speciality(ushort id, Guid doctorId, string specialityName)
    {
        if (doctorId == Guid.Empty)
            throw new ArgumentException("Doctor id cannot be empty.", nameof(doctorId));
        
        if (string.IsNullOrEmpty(specialityName))
            throw new ArgumentException("Speciality name cannot be empty.", nameof(specialityName));
        
        Id = id;
        DoctorId = doctorId;
        SpecialityName = specialityName;
    }

    private Speciality()
    {
        
    }
 
}