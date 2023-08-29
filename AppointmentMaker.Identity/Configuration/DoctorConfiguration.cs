using AppointmentMaker.Identity.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentMaker.Identity.Configuration;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.Property(e => e.About)
            .IsRequired();

        builder.HasMany(e => e.PatientsList)
            .WithOne(e => e.FamilyDoctor)
            .HasForeignKey(e => e.FamilyDoctorId);
    }
}
