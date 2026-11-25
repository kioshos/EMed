namespace EMed.Application.Dtos;

public sealed record CreateAppointmentRequestDto
{
    public Guid DoctorId { get; init; }
    public DateTime Time { get; init; }
    public string Description { get; init; }
}