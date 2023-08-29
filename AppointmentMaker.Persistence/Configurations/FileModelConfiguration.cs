using AppointmentMaker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentMaker.Persistence.Configurations;

internal class FileModelConfiguration : IEntityTypeConfiguration<FileModel>
{
    public void Configure(EntityTypeBuilder<FileModel> builder)
    {
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Extention)
            .HasMaxLength(10);
        builder.Property(f => f.Name)
            .HasMaxLength(100);
    }
}
