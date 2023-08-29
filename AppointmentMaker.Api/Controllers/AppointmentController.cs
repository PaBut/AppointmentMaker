using AppointmentMaker.Application.Features.Appointement.Commands.Cancel;
using AppointmentMaker.Application.Features.Appointement.Commands.Create;
using AppointmentMaker.Application.Features.Appointement.Commands.Delete;
using AppointmentMaker.Application.Features.Appointement.Queries.GetDetails;
using AppointmentMaker.Application.Features.Appointement.Queries.GetDoctorAppointments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentMaker.Api.Controllers;

[ApiVersionNeutral]
public class AppointmentController : ApplicationBaseController
{
    private readonly IMediator _mediator;

    public AppointmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("/cancel/{id:guid}")]
    [Authorize(Roles = "Patient")]
    public async Task<ActionResult> Cancel([FromQuery]Guid id)
    {
        var result = await _mediator.Send(new AppointmentCancelCommand(id));

        if(result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message);
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
            return Problem(title: result.Error.Code, detail: result.Error.Message);
        }

        return CreatedAtAction(nameof(Get), result.Value);
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Delete([FromQuery]Guid id)
    {
        var result = await _mediator.Send(new AppointmentDeleteCommand(id));

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message);
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
            return Problem(title: result.Error.Code, detail: result.Error.Message);
        }

        return result.Value;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<AppointmentGetByDoctorResponse>> Get
        (string doctorId, int pageSize, DateTime? cursor = null, DateOnly? date = null)
    {
        var result = await _mediator.Send(new AppointmentGetAllByDoctorQuery(doctorId, pageSize, cursor, date));

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message);
        }

        return result.Value;
    }
}
