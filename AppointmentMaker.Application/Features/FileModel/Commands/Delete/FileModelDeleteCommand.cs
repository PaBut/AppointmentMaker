using AppointmentMaker.Application.Features.Shared;
using Microsoft.AspNetCore.Http;

namespace AppointmentMaker.Application.Features.FileModel.Commands.Delete;

public record FileModelDeleteCommand(Guid Id) : IResultRequest;
