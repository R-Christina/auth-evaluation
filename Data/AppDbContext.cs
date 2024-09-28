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
        public DbSet<Emp> Emp { get; set; }
        public DbSet<Role> Role { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .ToTable("Users")
                .HasKey(u => u.user_id);

            modelBuilder.Entity<Users>()
                .Property(u => u.user_id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Users>()
                .HasOne(u => u.emp)
                .WithMany(e => e.users)
                .HasForeignKey(u => u.emp_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Users>()
                .HasOne(u => u.role)
                .WithMany(r => r.users)
                .HasForeignKey(u => u.role_id)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}