using auth.Models;
using Microsoft.EntityFrameworkCore;

namespace auth.Data.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Users> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().ToTable("Users");

            modelBuilder.Entity<Users>()
                .HasKey(u => u.user_id);

            modelBuilder.Entity<Users>()
                .Property(u => u.user_id)
                .ValueGeneratedOnAdd();
        }
    }
}