namespace EMed.Application.Dtos;

public sealed class AppointmentDto
{
    public DateTime Time { get; init; }
    public string DoctorName { get; init; }
    public string Description { get; init; }
}