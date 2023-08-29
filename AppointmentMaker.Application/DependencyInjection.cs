using AppointmentMaker.Domain.Configuration;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AppointmentMaker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly));
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.Configure<ScheduleConfiguration>(configuration.GetSection("scheduleConfig"));
        return services;
    }
}