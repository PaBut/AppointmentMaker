﻿using AppointmentMaker.Api.Controllers.Common;
using AppointmentMaker.Api.Extentsions;
using AppointmentMaker.Application.Features.Schedule.Commands.Create.WithBoolArray;
using AppointmentMaker.Application.Features.Schedule.Commands.UpdateTemplateDay.WithBoolArray;
using AppointmentMaker.Application.Features.Schedule.Commands.UpdateTemplateWeek.WithBoolArray;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentMaker.Api.Controllers.v1;

[ApiVersion("1.0")]
[Authorize(Roles = "Doctor")]
public class Schedule1Controller : ApplicationBaseController
{
    private readonly IMediator _mediator;

    public Schedule1Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> Post(ScheduleCreateWithBoolArrayCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }

        return NoContent();
    }

    [HttpPut("{id:guid}/day")]
    public async Task<ActionResult> PutDay(ScheduleTemplateUpdateDayWithBoolArrayCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }

        return NoContent();
    }

    [HttpPut("{id:guid}/week")]
    public async Task<ActionResult> PutDay(ScheduleTemplateUpdateWeekWithBoolArrayCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }

        return NoContent();
    }
}
