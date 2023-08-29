using AppointmentMaker.Application.Features.Shared;
using Microsoft.AspNetCore.Http;

namespace AppointmentMaker.Application.Features.FileModel.Commands.Update;

public record FileModelUpdateCommand(Guid Id, IFormFile FormFile) : IResultRequest;
