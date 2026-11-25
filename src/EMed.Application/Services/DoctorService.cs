using EMed.Application.Dtos;
using EMed.Application.Interfaces;
using EMed.Application.Interfaces.Caching;
using EMed.Domain.Models;

namespace EMed.Application.Services;

public class DoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly ISpecialityRepository _specialityRepository;
    private readonly ICacheService _cacheService;
    private readonly ICacheInvalidator _cacheInvalidator;

    public DoctorService( IDoctorRepository doctorRepository, ISpecialityRepository specialityRepository
    , ICacheService cacheService, ICacheInvalidator cacheInvalidator)
    {
        _doctorRepository = doctorRepository;
        _specialityRepository =  specialityRepository;
        _cacheService = cacheService;
        _cacheInvalidator = cacheInvalidator;
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
        var cached = await _cacheService.GetAsync<DoctorDto>($"doctor:{doctorId}", cancellationToken);
        
        if (cached != null)
            return cached; 
        
        var doctor = await _doctorRepository.GetByIdAsync(doctorId, cancellationToken);
        var specialities = await _specialityRepository.GetSpecialitiesByDoctorIdAsync(doctorId, cancellationToken);
        
        
        
       var doctorDto = new DoctorDto
        {
            Name = doctor.Name,
            SpecialitiyIds = specialities.Select(s => s.Id).ToList()
        };
        
        await _cacheService.SetAsync(
            $"doctor:{doctor.Id.ToString()}", 
            doctorDto, 
            TimeSpan.FromMinutes(10), 
            cancellationToken);
        
        return doctorDto;
    }
}