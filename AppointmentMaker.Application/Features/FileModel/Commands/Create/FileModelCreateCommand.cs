using AppointmentMaker.Application.Features.Shared;
using Microsoft.AspNetCore.Http;

namespace AppointmentMaker.Application.Features.FileModel.Commands.Create;

public record FileModelCreateCommand(IFormFile FormFile, string doctorId) : IResultRequest<Guid>;
