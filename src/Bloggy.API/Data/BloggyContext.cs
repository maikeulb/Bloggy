using Bloggy.API.Data.Configurations;
using Bloggy.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.API.Data
{
    public class BloggyContext : DbContext
    {
        public BloggyContext (DbContextOptions<BloggyContext> options) : base (options) { }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }

        protected override void OnModelCreating (ModelBuilder builder)
        {
            builder.ApplyConfiguration (new UserConfiguration ());
            builder.ApplyConfiguration (new PostConfiguration ());
            builder.ApplyConfiguration (new CommentConfiguration ());
            builder.ApplyConfiguration (new CategoryConfiguration ());
            builder.ApplyConfiguration (new TagConfiguration ());
            builder.ApplyConfiguration (new PostTagConfiguration ());
        }
    }
}