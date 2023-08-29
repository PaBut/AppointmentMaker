using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Domain.Entities;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;
using AutoMapper;

namespace AppointmentMaker.Application.Features.Appointement.Queries.GetDoctorAppointments;

public class AppointmentGetByDoctorQueryHandler
    : IResultRequestHandler<AppointmentGetAllByDoctorQuery, AppointmentGetByDoctorResponse>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IMapper _mapper;

    public AppointmentGetByDoctorQueryHandler(IAppointmentRepository appointmentRepository,
        IMapper mapper)
    {
        _appointmentRepository = appointmentRepository;
        _mapper = mapper;
    }

    public async Task<Result<AppointmentGetByDoctorResponse>> Handle
        (AppointmentGetAllByDoctorQuery query, CancellationToken cancellationToken)
    {
        var validator = new AppointmentGetAllByDoctorQueryValidator();

        var validationResult = await validator.ValidateAsync(query);

        if(!validationResult.IsValid)
        {
            return Result.Failure<AppointmentGetByDoctorResponse>
                (Error.FromValidationResult(validationResult));
        }

        IEnumerable<Appointment> appointments = await _appointmentRepository
            .GetDoctorAppointmentsAsync(query.DoctorId, query.Date, nameof(Appointment.DateTime));

        if (!appointments.Any())
        {
            return Result.Failure<AppointmentGetByDoctorResponse>(new Error("Appointment.Get",
                "Appointments with specified doctor not found"));
        }

        if (query.Cursor != null)
        {
            appointments.Where(e => e.DateTime >= query.Cursor);
        }

        appointments = appointments.Take(query.PageSize + 1);

        var appointmentDtos = _mapper.Map<List<AppointmentDto>>(appointments.Take(query.PageSize));

        AppointmentGetByDoctorResponse response = new()
        {
            Appointments = appointmentDtos,
            Cursor = appointments.Last().DateTime
        };

        return response;
    }
}
