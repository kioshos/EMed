namespace EMed.Domain.Models;

public class Schedule
{
    public Guid Id { get; set; }
    public Guid DoctorId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    
    public Schedule( Guid doctorId, DateTime startTime, DateTime endTime)
    {
        
        if (doctorId == Guid.Empty)
            throw new ArgumentException("Doctor id cannot be empty.", nameof(doctorId));
        
        if (startTime > endTime || startTime == endTime)
            throw new ArgumentException("Start time must be less than end time.", nameof(startTime));
        Id = Guid.CreateVersion7();
        DoctorId = doctorId;
        StartTime = startTime;
        EndTime = endTime;
    }

    private Schedule()
    {
        
    }
    
}