using AppointmentMaker.Api.Swagger.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AppointmentMaker.Api.Swagger;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;
    private readonly SwaggerApplicationSettings _swaggerApplicationSettings;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider, 
        IOptions<SwaggerApplicationSettings> swaggerApplicationSettingsOptions)
    {
        _apiVersionDescriptionProvider = apiVersionDescriptionProvider;
        _swaggerApplicationSettings = swaggerApplicationSettingsOptions.Value;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach(var description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName,
                CreateInfoForApiVersion(description, _swaggerApplicationSettings));
        }
    }

    internal static OpenApiInfo CreateInfoForApiVersion
        (ApiVersionDescription description, SwaggerApplicationSettings settings)
    {
        var versionDescription = settings.Descriptions.FirstOrDefault(x =>
        x.MajorVersion == (description.ApiVersion.MajorVersion ?? 0)
        && x.MinorVersion == (description.ApiVersion.MinorVersion ?? 0)
        && (string.IsNullOrEmpty(description.ApiVersion.Status)
        || x.Status == description.ApiVersion.Status));

        var info = new OpenApiInfo
        {
            Title = settings.Title,
            Version = description.ApiVersion.ToString(),
            Description = $"{versionDescription?.Description}",
            Contact = new OpenApiContact
            {
                Name = settings.ContactName,
                Email = settings.ContactEmail,
            },
            TermsOfService = new Uri("https://www.linktotermsofuse.com"),
            License = new OpenApiLicense
            {
                Name = "MIT",
                Url = new Uri("https://opensource.org/licenses/MIT")
            }
        };

        if (description.IsDeprecated)
        {
            info.Description += "<p><font color='red'>This API version has been deprecated</font></p>";
        }

        return info;
    }
}
