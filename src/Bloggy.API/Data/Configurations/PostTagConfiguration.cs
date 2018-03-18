using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Bloggy.API.Entities;

namespace Bloggy.API.Data.Configurations
{
    class CommentConfiguration
        : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("PostTag");

            builder.HasKey(t => new { t.PostId, t.TagId });

            builder.HasOne(pt => pt.Post)
              .WithMany(p => p.PostTags)
              .HasForeignKey(pt => pt.PostId);

            builder.HasOne(pt => pt.Tag)
              .WithMany(t => t.PostTags)
              .HasForeignKey(pt => pt.TagId);
        }
    }
}
