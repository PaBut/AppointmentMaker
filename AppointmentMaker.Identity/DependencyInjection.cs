using AppointmentMaker.Application.Models.Identity;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Identity.DatabaseContext;
using AppointmentMaker.Identity.Entities.Role;
using AppointmentMaker.Identity.Entities.Users;
using AppointmentMaker.Identity.Entities.Users.Base;
using AppointmentMaker.Identity.Services;
using AppointmentMaker.Identity.SignInManagers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace AppointmentMaker.Identity;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentity(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("jwtSettings"));

        services.AddDbContext<ApplicationIdentityDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Default"));
        });

        ConfigureUser<Doctor>(services);
        ConfigureUser<Patient>(services);

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).
        AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = configuration["jwtSettings:Audience"],
                ValidIssuer = configuration["jwtSettings:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwtSettings:Key"] 
                ?? throw new ArgumentNullException()))
            };
        });

        services.AddScoped<IAuthPatientService, AuthPatientService>();
        services.AddScoped<IAuthDoctorService, AuthDoctorService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IPatientDoctorConnectionService, PatientDoctorConnectionService>();

        return services;
    }

    private static void ConfigureIdentityOptions(IdentityOptions options)
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
    }

    private static void ConfigureUser<TUser>(IServiceCollection services) where TUser : AbstractUser
    {
        services.AddIdentityCore<TUser>(ConfigureIdentityOptions)
            .AddRoles<ApplicationRole>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationIdentityDbContext>();

        services.AddScoped<SignInManager<TUser>, ApplicationSignInManager<TUser>>();
    }
}
