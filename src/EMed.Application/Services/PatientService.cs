using EMed.Application.Dtos;
using EMed.Application.Interfaces;
using EMed.Application.Interfaces.Caching;
using EMed.Domain.Models;

namespace EMed.Application.Services;

public class PatientService
{
    private readonly IRepository<Patient, Guid> _patientRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly ICacheService _cacheService;
    private readonly ICacheInvalidator _cacheInvalidator;

    public PatientService(IRepository<Patient, Guid> patientRepository, 
        IAppointmentRepository appointmentRepository, IDoctorRepository doctorRepository, 
        IScheduleRepository scheduleRepository, ICacheService cacheService, ICacheInvalidator cacheInvalidator)
    {
        _patientRepository = patientRepository;
        _appointmentRepository = appointmentRepository;
        _doctorRepository = doctorRepository;
        _scheduleRepository = scheduleRepository;
        
        _cacheService = cacheService;
        _cacheInvalidator = cacheInvalidator;
    }
    

    public async Task<IEnumerable<Patient>> GetAllPatientsAsync(CancellationToken cancellationToken = default)
    {
        var patients = await _patientRepository.GetAllAsync();

        return patients;
    }
    public async Task<Patient> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cached = await _cacheService.GetAsync<Patient>($"patient:{id}", cancellationToken);
        
        if (cached != null)
           return cached; 
            
        var patient = await _patientRepository.GetByIdAsync(id, cancellationToken);
        
        if (patient == null)
            throw new Exception("Patient not found");
        
        await _cacheService.SetAsync(
            $"patient:{patient.Id.ToString()}", 
            patient, 
            TimeSpan.FromMinutes(10), 
            cancellationToken);

        return patient;
    }

    public async Task AddPatientAsync(PatientDto patientDto, CancellationToken cancellationToken = default)
    {
       
        var newPatient = new Patient(
            patientDto.TelephoneNumber,
            patientDto.Address,
            patientDto.Age,
            patientDto.Name
        );
        
        await _patientRepository.AddAsync(newPatient, cancellationToken);
    }
    public async Task<IEnumerable<AppointmentDto>> GetPatientAppointmentsAsync(Guid patientId,CancellationToken cancellationToken = default)
    {
        var patient = await _patientRepository.GetByIdAsync(patientId, cancellationToken);
        
        if (patient == null)
            throw new Exception("Patient not found");

        var appointments = await _appointmentRepository.GetByPatientId(patientId, cancellationToken);
        List<Guid> doctorIds = appointments.Select(p => p.DoctorId).ToList();
        var doctors = await _doctorRepository.GetDoctorsByIds(doctorIds, cancellationToken);
        var doctorsById = doctors.ToDictionary(d => d.Id);
        return appointments.Select(a => new AppointmentDto
        {
            Description = a.Description,
            DoctorName = doctorsById[a.DoctorId].Name,
            Time = a.Time
        });
    }

    public async Task AddAppointmentAsync(Guid patientId, CreateAppointmentRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        var schedule = await _scheduleRepository.GetScheduleByDoctorIdAsync(requestDto.DoctorId, cancellationToken);
        if (requestDto.Time.Hour < schedule.StartTime.Hour && requestDto.Time.Hour > schedule.EndTime.Hour)
            throw new Exception("Appointment time must be after schedule start time");
        
        var appointments = await _appointmentRepository.GetByDoctorIdWithTime(requestDto.DoctorId, requestDto.Time, cancellationToken);
        
        var appointment = new Appointment(patientId, requestDto.DoctorId, requestDto.Time, requestDto.Description);
        await _appointmentRepository.AddAsync(appointment,cancellationToken);
    }
    
    
}