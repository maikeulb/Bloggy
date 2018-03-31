using Bloggy.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bloggy.API.Data.Configurations
{
    class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure (EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable ("Users");

            builder.HasKey (t => t.Id);

            builder.Property (t => t.Id)
                .IsRequired ();

            builder.Property (au => au.Username)
                .IsRequired ();

            builder.Property (au => au.Email)
                .IsRequired ();

            builder.Property (au => au.HashedPassword)
                .IsRequired ();

            builder.Property (au => au.Salt)
                .IsRequired ();
        }
    }
}
