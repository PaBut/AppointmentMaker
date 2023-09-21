using AppointmentMaker.Api.Extentsions;
using AppointmentMaker.Application.Features.Appointment.Commands.Cancel;
using AppointmentMaker.Application.Features.FileModel.Commands.Delete;
using AppointmentMaker.Application.Features.FileModel.Commands.RegisterCreate;
using AppointmentMaker.Application.Features.FileModel.Commands.Update;
using AppointmentMaker.Application.Features.FileModel.Queries.Get;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentMaker.Api.Controllers.Common;

[ApiVersionNeutral]
public class ProfilePictureController : ApplicationBaseController
{
    private readonly IMediator _mediator;

    public ProfilePictureController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Doctor")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new FileModelDeleteCommand(id));

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }

        return NoContent();
    }

    [HttpPost]
    [Authorize(Roles = "Doctor")]
    public async Task<ActionResult> Post(FileModelRegisterCreateCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }

        return CreatedAtAction(nameof(Get), result.Value);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Doctor")]
    public async Task<ActionResult> Put(FileModelUpdateCommand command)
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
    public async Task<ActionResult> Get(Guid id)
    {
        var result = await _mediator.Send(new GetFileModelQuery(id));

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }

        return File(result.Value.Content, result.Value.Extension);
    }
}
