using EMed.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EMed.Api.Controllers;

[Route("doctors/{id:guid}/schedules")]
public class ScheduleController : ControllerBase
{
    private readonly ScheduleService _scheduleService;
}