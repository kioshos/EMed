using EMed.Application.Dtos;
using EMed.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EMed.Api.Controllers;

[ApiController]
[Route("patients")]
public class PatientController : ControllerBase
{
    private readonly PatientService _patientService;
    public PatientController(PatientService patientService)
    {
        _patientService = patientService;
    }
    [HttpPost]
    public async Task<IActionResult> AddPatient([FromBody] PatientDto patientDto, CancellationToken ct)
    {
        await _patientService.AddPatientAsync(patientDto, ct);
        return Ok(new { message = "Patient has been added" });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken ct)
    {
        var result = await _patientService.GetAllPatientsAsync(ct);
        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _patientService.GetByIdAsync(id,ct);
        return Ok(result);
    }
}

