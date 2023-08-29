using AppointmentMaker.Application.Features.Shared;

namespace AppointmentMaker.Application.Features.PatientVisit.Queries.GetVisitDetails;

public record PatientVisitGetVisitDetailsQuery(Guid Id) : IResultRequest<PatientVisitDetailsDto>;
