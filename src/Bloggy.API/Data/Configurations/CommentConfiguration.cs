using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Bloggy.API.Entities;

namespace Bloggy.API.Data.Configurations
{
    class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure (EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable ("Comments");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
               .IsRequired();

            builder.Property(c => c.Body)
                .IsRequired()
                .HasMaxLength(140);

            builder.Property(c => c.CreatedDate)
                .HasDefaultValueSql("now()");
        }
    }
}
