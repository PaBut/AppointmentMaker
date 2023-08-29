using AppointmentMaker.Application.Features.FileModel.Queries.Shared;
using AppointmentMaker.Application.Features.Shared;

namespace AppointmentMaker.Application.Features.FileModel.Queries.Get;

public record GetFileModelQuery(Guid Id) : IResultRequest<FileModelDto>;

