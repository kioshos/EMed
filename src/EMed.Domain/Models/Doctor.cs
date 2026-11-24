namespace EMed.Domain.Models;

public class Doctor
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    
    public Doctor(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Doctor name cannot be null or whitespace.", nameof(name));
        Id = Guid.CreateVersion7();
        Name = name;
    }

    private Doctor()
    {
        
    }
   
    
}