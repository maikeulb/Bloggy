using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Bloggy.API.Entities;

namespace Bloggy.API.Data.Configurations
{
    class PostTagConfiguration : IEntityTypeConfiguration<PostTag>
    {
        public void Configure (EntityTypeBuilder<PostTag> builder)
        {
            builder.ToTable("PostTag");

            builder.HasKey(t => new { t.PostId, t.TagId });

            builder.HasOne(pt => pt.Post)
              .WithMany(p => p.PostTags)
              .HasForeignKey(pt => pt.PostId)
              .IsRequired();

            builder.HasOne(pt => pt.Tag)
              .WithMany(t => t.PostTags)
              .HasForeignKey(pt => pt.TagId)
              .IsRequired();
        }
    }
}
