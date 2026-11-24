using EMed.Application.Dtos;
using EMed.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EMed.Api.Controllers;

[ApiController]
[Route("doctors")]
public class DoctorController : ControllerBase
{
    private readonly DoctorService _doctorService;
    public DoctorController(DoctorService doctorService)
    {
        _doctorService = doctorService;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddDoctorAsync([FromBody] DoctorDto doctorDto, CancellationToken ct = default)
    {
        _doctorService.AddDoctorAsync(doctorDto,ct);
        return Ok(new { message = "Doctor has been added" });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken ct)
    {
        var result = await _doctorService.GetAllDoctorsAsync(ct);
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute]Guid id,CancellationToken ct)
    {
        var result = await _doctorService.GetDoctorAsync(id, ct);
        return Ok(result);
    }
}