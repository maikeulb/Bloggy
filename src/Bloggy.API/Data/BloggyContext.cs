using System.Data;
using System.Threading.Tasks;
using Bloggy.API.Data.Configurations;
using Bloggy.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Bloggy.API.Data
{
    public class BloggyContext : DbContext
    {
        private IDbContextTransaction _currentTransaction;

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

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync().ConfigureAwait(false);

                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}
