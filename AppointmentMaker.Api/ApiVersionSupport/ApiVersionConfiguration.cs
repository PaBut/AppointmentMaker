using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace AppointmentMaker.Api.ApiVersionSupport;

public static class ApiVersionConfiguration
{
    public static IServiceCollection AddApiVersionConfiguration(this IServiceCollection services,
        ApiVersion? defaultVersion = null)
    {
        defaultVersion ??= ApiVersion.Default;
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = defaultVersion;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
            options.ReportApiVersions = true;
            options.UseApiBehavior = true;
        })
        .AddVersionedApiExplorer(options =>
        {
            options.DefaultApiVersion = defaultVersion;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        return services;
    }
}
