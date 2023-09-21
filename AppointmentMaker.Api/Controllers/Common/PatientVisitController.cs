using AppointmentMaker.Api.Extentsions;
using AppointmentMaker.Application.Features.PatientVisit.Commands.Create;
using AppointmentMaker.Application.Features.PatientVisit.Commands.Delete;
using AppointmentMaker.Application.Features.PatientVisit.Commands.Update;
using AppointmentMaker.Application.Features.PatientVisit.Queries.GetVisitDetails;
using AppointmentMaker.Application.Features.PatientVisit.Queries.GetVisits;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentMaker.Api.Controllers.Common;

[ApiVersionNeutral]
public class PatientVisitController : ApplicationBaseController
{
    private readonly IMediator _mediator;

    public PatientVisitController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Doctor")]
    public async Task<ActionResult> Post(PatientVisitCreateCommand command)
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
    [Authorize(Roles = "Doctor")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new PatientVisitDeleteCommand(id));

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Doctor")]
    public async Task<ActionResult> Put(PatientVisitUpdateCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }

        return NoContent();
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<PatientVisitDetailsDto>> Get(Guid id)
    {
        var result = await _mediator.Send(new PatientVisitGetVisitDetailsQuery(id));

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }

        return result.Value;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<PatientVisitDto>>> GetByPatient(string patientId)
    {
        var result = await _mediator.Send(new PatientVisitGetVisitsQuery(patientId));

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }

        return result.Value;
    }
}
