using AppointmentMaker.Identity.Entities.Users;
using AppointmentMaker.Identity.Entities.Users.Base;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppointmentMaker.Identity.DatabaseContext;

public class ApplicationIdentityDbContext : IdentityDbContext
{
    public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
        : base(options)
    {
        
    }

    public new DbSet<AbstractUser> Users { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationIdentityDbContext).Assembly);
        base.OnModelCreating(builder);
    }
}
