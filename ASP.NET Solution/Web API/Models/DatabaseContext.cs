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

        public DbSet<Game> Games { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Widget> Widgets { get; set; }
        public DbSet<ActiveWidget> ActiveWidgets { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Background> Backgrounds { get; set; }

        //Set the Default SQL value to the dates.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(p => p.created_at).ValueGeneratedOnAdd().HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<User>().Property(p => p.updated_at).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Bookmark>().Property(p => p.created_at).ValueGeneratedOnAdd().HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Bookmark>().Property(p => p.updated_at).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Game>().Property(p => p.created_at).ValueGeneratedOnAdd().HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Game>().Property(p => p.finished_at).ValueGeneratedOnAdd().HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<AccessToken>().Property(p => p.created_at).ValueGeneratedOnAdd().HasDefaultValueSql("GETDATE()");
        }
    }
}
