using AppointmentMaker.Application.Features.Shared;

namespace AppointmentMaker.Application.Features.PatientVisit.Queries.GetVisits;

public record PatientVisitGetVisitsQuery(string PatientId) : IResultRequest<List<PatientVisitDto>>;
