using AppointmentMaker.Application.Features.Shared;
using Microsoft.AspNetCore.Http;

namespace AppointmentMaker.Application.Features.FileModel.Commands.RegisterCreate;

public record FileModelRegisterCreateCommand(IFormFile FormFile) : IResultRequest<Guid>;
