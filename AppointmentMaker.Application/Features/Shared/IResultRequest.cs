using AppointmentMaker.Domain.Shared;
using MediatR;

namespace AppointmentMaker.Application.Features.Shared;

internal interface IResultRequest<TOut> : IRequest<Result<TOut>>
{
}

internal interface IResultRequest : IRequest<Result>
{

}
