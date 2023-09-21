using AppointmentMaker.Domain.Entities;
using AppointmentMaker.Domain.Entities.Common;
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

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach(var entry in base.ChangeTracker.Entries<Entity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
        {
            entry.Entity.ModifiedDate = DateTime.UtcNow;
            
            if(entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
