namespace EMed.Application.Events;

public record AppointmentCreatedEvent(
    Guid Id,
    Guid PatientId,
    Guid DoctorId,
    DateTime Time,
    string Description);