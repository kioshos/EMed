using EMed.Application.Dtos;
using EMed.Application.Interfaces;
using EMed.Domain.Models;

namespace EMed.Application.Services;

public class DoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly ISpecialityRepository _specialityRepository;

    public DoctorService( IDoctorRepository doctorRepository, ISpecialityRepository specialityRepository)
    {
        _doctorRepository = doctorRepository;
        _specialityRepository =  specialityRepository;
    }
    

    public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync(CancellationToken cancellationToken = default)
    {
        var doctors = await _doctorRepository.GetAllAsync();

        return doctors;
    }

    public async Task AddDoctorAsync(DoctorDto doctorDto, CancellationToken cancellationToken = default)
    {
       
        var newDoctor = new Doctor(
            doctorDto.Name
        );
        
        await _doctorRepository.AddAsync(newDoctor, cancellationToken);
    }
    
    public async Task<DoctorDto> GetDoctorAsync(Guid doctorId, CancellationToken cancellationToken = default)
    {
        var doctor = await _doctorRepository.GetByIdAsync(doctorId, cancellationToken);
        var specialities = await _specialityRepository.GetSpecialitiesByDoctorIdAsync(doctorId, cancellationToken);
        return new DoctorDto
        {
            Name = doctor.Name,
            SpecialitiyIds = specialities.Select(s => s.Id).ToList()
        };
    }
}