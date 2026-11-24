using EMed.Application.Interfaces;
using EMed.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace EMed.Tests.Integration;

public class PatientTest : BaseIntegrationTest
{
    private readonly IRepository<Patient,Guid> _patientRepository;
    
    public PatientTest(IntegrationTestWebFactory factory) 
        : base(factory)
    {
        _patientRepository = ServiceProvider.GetRequiredService<IRepository<Patient, Guid>>();
    }

    [Fact]
    public async Task CreatePatient_ShouldSavePatientToDb()
    {
        //arrange
        var telephoneNumber = "+380983133544";
        var address = "Pokrovska 5";
        var age = 23;
        var name = "Vlad Babir";
        var patient = new Patient(telephoneNumber, address, age, name);
        
        //act
        await _patientRepository.AddAsync(patient);
        
        //assert
        var dbPatient = await _patientRepository.GetByIdAsync(patient.Id);
        Assert.NotNull(dbPatient);
        Assert.Equal(telephoneNumber, dbPatient.TelephoneNumber);
        Assert.Equal(address, dbPatient.Address);
        Assert.Equal(age, dbPatient.Age);
        Assert.Equal(name, dbPatient.Name); 
        Assert.Equal(patient.Id, dbPatient.Id);
    }
}