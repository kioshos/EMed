using EMed.Domain.Models;

namespace EMed.Infrastructure;

public class DoctorSpeciality
{
    public Guid DoctorId { get; set; }
    public ushort SpecialityId { get; set; }
}