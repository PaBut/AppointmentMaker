using AppointmentMaker.Persistence;
using AppointmentMaker.Application;
using AppointmentMaker.Identity;
using AppointmentMaker.Infrastructure;
using AppointmentMaker.Api.ApiVersionSupport;
using AppointmentMaker.Api.Swagger;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

IConfiguration configuration = builder.Configuration;
builder.Services.AddApplication(configuration)
    .AddPersistence(configuration)
    .AddIdentity(configuration)
    .AddInfrastructure();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersionConfiguration();
builder.Services.AddAndConfigureSwagger(builder.Configuration,
    Path.Combine(AppContext.BaseDirectory,
    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"), true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        using var scope = app.Services.CreateScope();
        var versionProvider = scope.ServiceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var description in versionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
