using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;
using AutoMapper;

namespace AppointmentMaker.Application.Features.Appointment.Queries.GetDoctorAppointments;

public class AppointmentGetByDoctorQueryHandler
    : IResultRequestHandler<AppointmentGetByDoctorQuery, CursorResponse<AppointmentDto>>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IMapper _mapper;

    public AppointmentGetByDoctorQueryHandler(IAppointmentRepository appointmentRepository,
        IMapper mapper)
    {
        _appointmentRepository = appointmentRepository;
        _mapper = mapper;
    }

    public async Task<Result<CursorResponse<AppointmentDto>>> Handle
        (AppointmentGetByDoctorQuery query, CancellationToken cancellationToken)
    {
        var validator = new AppointmentGetByDoctorQueryValidator();

        var validationResult = await validator.ValidateAsync(query);

        if(!validationResult.IsValid)
        {
            return Result.Failure<CursorResponse<AppointmentDto>>
                (Error.FromValidationResult(validationResult));
        }

        IEnumerable<Domain.Entities.Appointment> appointments = await _appointmentRepository
            .GetDoctorAppointmentsAsync(query.DoctorId, query.Date, nameof(Domain.Entities.Appointment.DateTime));

        if (!appointments.Any())
        {
            return Result.Failure<CursorResponse<AppointmentDto>>(Error.NotFound(nameof(Domain.Entities.Appointment)));
        }

        //if (query.Cursor != null)
        //{
        //    appointments = appointments.Where(e => e.DateTime >= query.Cursor);
        //}

        //appointments = appointments.Take(query.PageSize + 1);

        //var appointmentDtos = _mapper.Map<List<AppointmentDto>>(appointments.Take(query.PageSize));

        //AppointmentGetByDoctorResponse response = new()
        //{
        //    Appointments = appointmentDtos,
        //    Cursor = appointments.Last().DateTime,
        //    HasNextPage = appointments.Count() == query.PageSize + 1
        //};

        var responseResult = CursorResponse<AppointmentDto>
            .GetCursorResponse(appointments,
            query.PageSize,
            query.Cursor,
            nameof(Domain.Entities.Appointment.DateTime),
            true,
            _mapper);

        if (responseResult.IsFailure)
        {
            return Result.Failure<CursorResponse<AppointmentDto>>
                (responseResult.Error);
        }

        return responseResult.Value;
    }
}
