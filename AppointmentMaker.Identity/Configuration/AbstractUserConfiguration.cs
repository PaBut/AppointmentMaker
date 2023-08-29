using AppointmentMaker.Identity.Entities.Users.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentMaker.Identity.Configuration
{
    internal class AbstractUserConfiguration : IEntityTypeConfiguration<AbstractUser>
    {
        public void Configure(EntityTypeBuilder<AbstractUser> builder)
        {
            builder.HasIndex(e => e.Email)
                .IsUnique();
            builder.HasIndex(e => e.PhoneNumber)
                .IsUnique();
            builder.Property(e => e.FullName)
                .IsRequired();
            builder.Property(e => e.Birthday)
                .IsRequired();
        }
    }
}
