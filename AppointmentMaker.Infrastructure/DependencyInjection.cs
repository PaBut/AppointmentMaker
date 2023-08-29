using AppointmentMaker.Infrastructure.Setups;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace AppointmentMaker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddQuartz(options =>
        {
            options.UseMicrosoftDependencyInjectionJobFactory();
        });
        
        services.AddQuartzHostedService();

        services.ConfigureOptions<ScheduleShiftBackgroundJobSetup>();

        return services;
    }
}