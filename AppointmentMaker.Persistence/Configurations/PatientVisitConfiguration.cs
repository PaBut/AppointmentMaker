using AppointmentMaker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentMaker.Persistence.Configurations;

internal class PatientVisitConfiguration : IEntityTypeConfiguration<PatientVisit>
{
    private const int GuidLength = 50;
 
    public void Configure(EntityTypeBuilder<PatientVisit> builder)
    {
        builder.HasKey(pv => pv.Id);
        builder.Property(pv => pv.PatientProblem)
            .IsRequired();
        builder.Property(pv => pv.VisitResult)
            .IsRequired();
        builder.Property(pv => pv.DoctorId)
            .HasMaxLength(GuidLength)
            .IsRequired();
        builder.Property(pv => pv.PatientId)
            .HasMaxLength(GuidLength)
            .IsRequired();
        builder.HasOne(pv => pv.Appointment)
            .WithOne()
            .HasForeignKey<PatientVisit>(nameof(PatientVisit.AppointmentId));
    }
}
