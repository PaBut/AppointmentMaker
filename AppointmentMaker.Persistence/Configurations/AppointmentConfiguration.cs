using AppointmentMaker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentMaker.Persistence.Configurations;

internal class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    private const int GuidLength = 50;
 
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.PatientProblem)
            .IsRequired();
        builder.Property(a => a.PatientProblem)
            .IsRequired();
        builder.Property(a => a.DoctorId)
            .HasMaxLength(GuidLength)
            .IsRequired();
        builder.Property(a => a.PatientId)
            .HasMaxLength(GuidLength)
            .IsRequired();
        builder.HasOne(a => a.Schedule)
            .WithMany()
            .HasForeignKey(nameof(Appointment.ScheduleId));
    }
}
