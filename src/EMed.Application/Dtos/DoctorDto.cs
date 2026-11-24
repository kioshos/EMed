namespace EMed.Application.Dtos;

public sealed class DoctorDto
{
    public string Name { get; init; }
    public List<ushort> SpecialitiyIds { get; init; }
}