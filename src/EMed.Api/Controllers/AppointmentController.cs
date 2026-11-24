using EMed.Application.Dtos;
using EMed.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EMed.Api.Controllers;

[ApiController]
[Route("patients/{id:guid}/appointments")]
public class AppointmentController : ControllerBase
{
    private readonly PatientService _patientService;

    public AppointmentController(PatientService patientService)
    {
        _patientService = patientService;
    }
    [HttpGet]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _patientService.GetPatientAppointmentsAsync(id,ct);
        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] Guid id, [FromBody] CreateAppointmentRequestDto request,CancellationToken ct)
    {
         await _patientService.AddAppointmentAsync(id,request);

         return Created();


    }
}