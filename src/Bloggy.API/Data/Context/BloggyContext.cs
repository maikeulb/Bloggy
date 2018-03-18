using Bloggy.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.API.Data.Context
{
    public class BloggyContext : DbContext
    {
        public BloggyContext(DbContextOptions<BloggyContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new PostConfiguration());
            builder.ApplyConfiguration(new CommentConfiguration());
            builder.ApplyConfiguration(new TagConfiguration());
            builder.ApplyConfiguration(new PostTagConfiguration());
        }
    }
}
