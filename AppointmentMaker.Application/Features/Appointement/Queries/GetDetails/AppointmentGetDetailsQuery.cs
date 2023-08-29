using AppointmentMaker.Application.Features.Shared;

namespace AppointmentMaker.Application.Features.Appointement.Queries.GetDetails;

public record AppointmentGetDetailsQuery(Guid Id) : IResultRequest<AppointmentDetailsDto>;
