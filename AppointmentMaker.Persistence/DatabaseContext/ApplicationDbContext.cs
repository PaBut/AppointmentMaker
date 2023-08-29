using AppointmentMaker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppointmentMaker.Persistence.DatabaseContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<PatientVisit> PatientVisits { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<FileModel> Files { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
