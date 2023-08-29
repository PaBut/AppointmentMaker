using AppointmentMaker.Application.Features.Shared;

namespace AppointmentMaker.Application.Features.PatientVisit.Commands.Create;

public record PatientVisitCreateCommand(Guid AppointmentId, string VisitResult) : IResultRequest<Guid>;
