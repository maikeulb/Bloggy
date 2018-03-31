using Bloggy.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bloggy.API.Data.Configurations
{
    class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure (EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable ("Tags");

            builder.HasKey (t => t.Id);

            builder.Property (t => t.Id)
                .IsRequired ();

            builder.Property (t => t.Name)
                .IsRequired ()
                .HasMaxLength (100);
        }
    }
}
