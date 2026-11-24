using System.Text.RegularExpressions;

namespace EMed.Domain.Models;

public class Patient
{
    public Guid Id { get; set; }
    public string TelephoneNumber { get; set; }
    public string Address { get; set; }
    public int Age { get; set; }
    public string Name { get; set; }
    public Patient(string telephoneNumber, string address, int age, string name)
    {
        if (!IsValidPhoneNumber(telephoneNumber))
            throw new ArgumentException("Telephone number is invalid");
        
        
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address cannot be null or whitespace.");
        
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Doctor name cannot be null or whitespace.");
        
        if (age <= 0)
            throw new ArgumentException("Age cannot be zero or negative.");
        
        Id = Guid.CreateVersion7();
        TelephoneNumber = telephoneNumber;
        Address = address;
        Age = age;
        Name = name;
    }

    private Patient()
    {
        
    }

    private static bool IsValidPhoneNumber(string phoneNumber) => 
        Regex.IsMatch(phoneNumber,@"^\+380(?:50|63|66|67|68|73|93|95|96|97|98|99)\d{7}$", 
        RegexOptions.IgnoreCase);
}