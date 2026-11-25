namespace EMed.Application.Dtos;

public sealed record PatientDto
{
    public string Name { get; init; }
    public int Age { get; init; }
    public string TelephoneNumber { get; init; }
    public string Address { get; init; }
    
    
}