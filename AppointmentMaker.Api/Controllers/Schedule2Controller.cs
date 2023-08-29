using AppointmentMaker.Application.Features.Schedule.Commands.Create.WithTimeIntervals;
using AppointmentMaker.Application.Features.Schedule.Commands.UpdateTemplateDay.WithTimeIntervals;
using AppointmentMaker.Application.Features.Schedule.Commands.UpdateTemplateWeek.WithTimeIntervals;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentMaker.Api.Controllers;

[ApiVersion("1.1")]
[Authorize(Roles = "Doctor")]
public class Schedule2Controller : ApplicationBaseController
{
    private readonly IMediator _mediator;

    public Schedule2Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> Post(ScheduleCreateWithTimeIntervalsCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message);
        }

        return NoContent();
    }

    [HttpPut("{id:guid}/day")]
    public async Task<ActionResult> PutDay(ScheduleTemplateUpdateDayWithTimeIntervalsCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message);
        }

        return NoContent();
    }

    [HttpPut("{id:guid}/week")]
    public async Task<ActionResult> PutDay(ScheduleTemplateUpdateWeekWithTimeIntervalsCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message);
        }

        return NoContent();
    }
}
