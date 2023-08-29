using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;

namespace AppointmentMaker.Infrastructure.Jobs;

[DisallowConcurrentExecution]
public class ScheduleShiftBackgroundJob : IJob
{
    private readonly ILogger<ScheduleShiftBackgroundJob> _logger;
    private readonly Mediator _mediator;

    public ScheduleShiftBackgroundJob(ILogger<ScheduleShiftBackgroundJob> logger,
        Mediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Job started at {time}", DateTime.UtcNow);
        await _mediator.Send(context);
        _logger.LogInformation("Job ended at {time}", DateTime.UtcNow);
    }
}
