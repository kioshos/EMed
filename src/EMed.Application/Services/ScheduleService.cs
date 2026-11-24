using EMed.Application.Interfaces;
using EMed.Domain.Models;

namespace EMed.Application.Services;

public class ScheduleService
{
    private readonly IRepository<Schedule,Guid> _scheduleRepository;

    public ScheduleService()
    {
        
    }
}