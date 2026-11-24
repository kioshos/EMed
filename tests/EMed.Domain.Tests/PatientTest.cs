using EMed.Domain.Models;

namespace EMed.Domain.Tests;

public class PatientTest
{
    
    [Fact]
    public void Constructor_WithValidData_CreatesInstance()
    {
        //arrange
        var telephoneNumber = "+380983133544";
        var address = "Pokrovska 5";
        var age = 23;
        var name = "Vlad Babir";
        
        //act
        
        var patient = new Patient(telephoneNumber, address, age, name);
        
        //assert
        Assert.NotEqual(Guid.Empty, patient.Id);
        Assert.Equal(telephoneNumber, patient.TelephoneNumber);
        Assert.Equal(address, patient.Address);
        Assert.Equal(age, patient.Age);
        Assert.Equal(name, patient.Name);
    }
    
    [Fact]
    public void Constructor_WithInvalidTelephoneNumber_CreatesInstance()
    {
        //arrange
        var telephoneNumber = "12";
        var address = "Pokrovska 5";
        var age = 23;
        var name = "Vlad Babir";
        
        //act
        
        var exception = Assert.Throws<ArgumentException>(()=> new Patient(telephoneNumber, address, age, name));
        
        //assert
        Assert.Equal("Telephone number is invalid", exception.Message);
       
    }
}
