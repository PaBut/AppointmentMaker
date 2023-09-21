using System.Net;
using AppointmentMaker.Api.Extentsions;
using AppointmentMaker.Application.Features.Appointment.Commands.Cancel;
using AppointmentMaker.Application.Features.Appointment.Commands.Create;
using AppointmentMaker.Application.Features.Appointment.Commands.Delete;
using AppointmentMaker.Application.Features.Appointment.Queries.GetDetails;
using AppointmentMaker.Application.Features.Appointment.Queries.GetDoctorAppointments;
using AppointmentMaker.Application.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentMaker.Api.Controllers.Common;

[ApiVersionNeutral]
public class AppointmentController : ApplicationBaseController
{
    private readonly IMediator _mediator;

    public AppointmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{id:guid}/cancel")]
    [Authorize(Roles = "Patient")]
    public async Task<ActionResult> Cancel([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new AppointmentCancelCommand(id));

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message, 
                statusCode: result.Error.GetErrorStatusCode());
        }

        return NoContent();
    }

    [HttpPost]
    [Authorize(Roles = "Patient")]
    public async Task<ActionResult> Post(AppointmentCreateCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }

        return CreatedAtAction(nameof(Get), result.Value);
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new AppointmentDeleteCommand(id));

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }

        return NoContent();
    }

    //TODO: add Put request

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<AppointmentDetailsDto>> Get(Guid id)
    {
        var result = await _mediator.Send(new AppointmentGetDetailsQuery(id));

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }

        return result.Value;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<CursorResponse<AppointmentDto>>> GetByDoctor
        (string doctorId, int pageSize, string? cursor = null, DateOnly? date = null)
    {
        var result = await _mediator.Send(new AppointmentGetByDoctorQuery(doctorId, pageSize, cursor, date));

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }

        return result.Value;
    }
}
