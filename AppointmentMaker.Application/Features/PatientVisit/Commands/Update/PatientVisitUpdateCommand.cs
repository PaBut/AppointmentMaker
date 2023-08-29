using AppointmentMaker.Application.Features.Shared;

namespace AppointmentMaker.Application.Features.PatientVisit.Commands.Update;

public record PatientVisitUpdateCommand(Guid Id, string VisitResult) : IResultRequest;
