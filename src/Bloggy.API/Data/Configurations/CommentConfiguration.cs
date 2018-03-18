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
            builder.ToTable("Post");
        }
    }
}
