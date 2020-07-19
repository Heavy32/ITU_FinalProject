using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Models
{
    public class PostContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }

        public PostContext(DbContextOptions<PostContext> options) : base(options)
            => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
