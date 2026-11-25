namespace EMed.Application.Dtos;

public sealed record AppointmentDto
{
    public DateTime Time { get; init; }
    public string DoctorName { get; init; }
    public string Description { get; init; }
}