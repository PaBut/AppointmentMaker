using AppointmentMaker.Application.Features.PatientVisit.Commands.Create;
using AppointmentMaker.Application.Features.PatientVisit.Commands.Delete;
using AppointmentMaker.Application.Features.PatientVisit.Commands.Update;
using AppointmentMaker.Application.Features.PatientVisit.Queries.GetVisitDetails;
using AppointmentMaker.Application.Features.PatientVisit.Queries.GetVisits;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentMaker.Api.Controllers;

[ApiVersionNeutral]
public class PatientVisitController : ApplicationBaseController
{
    private readonly IMediator _mediator;

    public PatientVisitController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> Post(PatientVisitCreateCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message);
        }

        return CreatedAtAction(nameof(Get), result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromQuery] Guid id)
    {
        var result = await _mediator.Send(new PatientVisitDeleteCommand(id));

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message);
        }

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(PatientVisitUpdateCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message);
        }

        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PatientVisitDetailsDto>> Get(Guid id)
    {
        var result = await _mediator.Send(new PatientVisitGetVisitDetailsQuery(id));

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message);
        }

        return result.Value;
    }

    [HttpGet]
    public async Task<ActionResult<List<PatientVisitDto>>> Get(string patientId)
    {
        var result = await _mediator.Send(new PatientVisitGetVisitsQuery(patientId));

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message);
        }

        return result.Value;
    }
}
