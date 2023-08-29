using AppointmentMaker.Identity.Entities.Role;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentMaker.Identity.Configuration;

public class ConfigureRoles : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData(
            new ApplicationRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Doctor",
                NormalizedName = "DOCTOR"
            },
            new ApplicationRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Patient",
                NormalizedName = "PATIENT"
            });
    }
}
