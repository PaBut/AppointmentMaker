using AppointmentMaker.Api.Swagger.Models;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AppointmentMaker.Api.Swagger;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddAndConfigureSwagger(this IServiceCollection services,
        IConfiguration configuration,
        string xmlPathAndFile,
        bool addBasicSecurity)
    {
        services.Configure<SwaggerApplicationSettings>(configuration.GetSection(nameof(SwaggerApplicationSettings)));
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        return services;
    }
}
