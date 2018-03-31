using Bloggy.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bloggy.API.Data.Configurations
{
    class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure (EntityTypeBuilder<Post> builder)
        {
            builder.ToTable ("Posts");

            builder.HasKey (p => p.Id);

            builder.Property (p => p.Id)
                .IsRequired ();

            builder.Property (p => p.Body)
                .IsRequired ()
                .HasMaxLength (140);

            builder.Property (p => p.CreatedDate)
                .HasDefaultValueSql ("now()");

            builder.HasOne (p => p.Author)
                .WithMany ()
                .HasForeignKey (p => p.AuthorId);
        }
    }
}
