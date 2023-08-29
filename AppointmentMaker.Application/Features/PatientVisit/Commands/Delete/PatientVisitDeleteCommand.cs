using AppointmentMaker.Application.Features.Shared;

namespace AppointmentMaker.Application.Features.PatientVisit.Commands.Delete;

public record PatientVisitDeleteCommand(Guid Id) : IResultRequest;
