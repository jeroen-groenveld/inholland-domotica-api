using Microsoft.EntityFrameworkCore;
using Web_API.Models.TokenAuth;

namespace Web_API.Models
{
    public class DatabaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(env.CONNECTION_STRING);
        }

        //Add the tables here.
        public DbSet<User> Users { get; set; }
        public DbSet<AccessToken> AccessTokens { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        //Set the Default SQL value to the dates.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(p => p.created_at).ValueGeneratedOnAdd().HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<User>().Property(p => p.updated_at).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<AccessToken>().Property(p => p.created_at).ValueGeneratedOnAdd().HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<RefreshToken>().Property(p => p.expires_at).ValueGeneratedOnAdd().HasDefaultValueSql("GETDATE()");
        }
    }
}
