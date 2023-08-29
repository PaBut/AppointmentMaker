using AppointmentMaker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentMaker.Persistence.Configurations;

internal class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    private const int GuidLength = 50;
 
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.DoctorId)
            .HasMaxLength(GuidLength)
            .IsRequired();
        builder.Property(s => s.ScheduleSlots)
            .IsRequired();
        builder.Property(s => s.ScheduleTemplate)
            .IsRequired();
    }
}
