using EMed.Application.Interfaces;
using EMed.Application.Interfaces.Kafka;
using EMed.Application.Services;
using EMed.BackgroundServices;
using EMed.Domain.Models;
using EMed.Infrastructure.Postgres.Kafka;
using EMed.Infrastructure.Postgres.Persistence;
using EMed.Infrastructure.Postgres.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddCaching(builder.Configuration);
builder.Services.AddSingleton<IEventProducer, KafkaProducer>();
builder.Services.AddBackgroundWorkers();

builder.Services.AddScoped<IRepository<MedicalRecord, Guid>,MedicalRecordRepository>();
builder.Services.AddScoped<IRepository<Patient, Guid>,PatientRepository>();
builder.Services.AddScoped<IRepository<Schedule, Guid>,ScheduleRepository>();
builder.Services.AddScoped<IRepository<Speciality, ushort>,SpecialityRepository>();


builder.Services.AddScoped<ISpecialityRepository, SpecialityRepository>();
builder.Services.AddScoped<IAppointmentRepository,AppointmentRepository>();
builder.Services.AddScoped<IDoctorRepository,DoctorRepository>();
builder.Services.AddScoped<IScheduleRepository,ScheduleRepository>();

builder.Services.AddScoped<PatientService>();
builder.Services.AddScoped<DoctorService>();
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program
{
    
}