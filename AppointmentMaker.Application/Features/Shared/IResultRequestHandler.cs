using AppointmentMaker.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentMaker.Application.Features.Shared;

internal interface IResultRequestHandler<in TIn> : IRequestHandler<TIn, Result>
    where TIn : IResultRequest
{
}

internal interface IResultRequestHandler<in TIn, TOut> : IRequestHandler<TIn, Result<TOut>>
    where TIn : IResultRequest<TOut>
{
}