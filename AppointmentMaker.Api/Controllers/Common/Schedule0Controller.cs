using AppointmentMaker.Api.Extentsions;
using AppointmentMaker.Application.Features.Schedule.Commands.Delete;
using AppointmentMaker.Application.Features.Schedule.Queries.GetFreeDays;
using AppointmentMaker.Application.Features.Schedule.Queries.GetFreeTimeSlots;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentMaker.Api.Controllers.Common;

[ApiVersionNeutral]
public class Schedule0Controller : ApplicationBaseController
{
    protected readonly IMediator _mediator;

    public Schedule0Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new ScheduleDeleteCommand(id));

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }

        return NoContent();
    }

    [HttpGet("{id:guid}/freedays")]
    [Authorize]
    public async Task<ActionResult<List<int>>> GetFreeDays(ScheduleGetFreeDaysQuery query)
    {
        var result = await _mediator.Send(query);

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }

        return result.Value;
    }

    [HttpGet("{id:guid}/freetimeslots")]
    [Authorize]
    public async Task<ActionResult<List<TimeOnly>>> GetFreeTimeSlots(ScheduleGetFreeTimeSlotsQuery query)
    {
        var result = await _mediator.Send(query);

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }

        return result.Value;
    }
}
