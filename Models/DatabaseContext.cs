using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Domotica_API.Models.TokenAuth;
using Microsoft.EntityFrameworkCore;

namespace Domotica_API.Models
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
        public DbSet<Move> Moves { get; set; }
        public DbSet<Widget> Widgets { get; set; }
        public DbSet<ActiveWidget> ActiveWidgets { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Background> Backgrounds { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<User>().HasMany(s => s.GamesPlayer1).WithOne(s => s.User1).HasForeignKey(s => s.user1_id).OnDelete(DeleteBehavior.Restrict);
            mb.Entity<User>().HasMany(s => s.GamesPlayer2).WithOne(s => s.User2).HasForeignKey(s => s.user2_id).OnDelete(DeleteBehavior.Restrict);
            mb.Entity<User>().HasMany(s => s.GamesWon).WithOne(s => s.UserWinner).HasForeignKey(s => s.user_winner_id).OnDelete(DeleteBehavior.Restrict);

           

        }

        public override int SaveChanges()
        {
            var createdEntries = ChangeTracker.Entries<Date.IModelCreatedAt>();
            var updatedEntries = ChangeTracker.Entries<Date.IModelUpdatedAt>();
            DateTime now = DateTime.Now;

            //When adding entries update the created_at property.
            foreach (var entry in createdEntries.Where(x => x.State == EntityState.Added))
            {
                entry.Entity.created_at = now;
            }

            //If the entity state is not unchanged, update the updated_at property.
            foreach (var entry in updatedEntries.Where(x => x.State != EntityState.Unchanged))
            {
                entry.Entity.updated_at = now;
            }

            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            var createdEntries = ChangeTracker.Entries<Date.IModelCreatedAt>();
            var updatedEntries = ChangeTracker.Entries<Date.IModelUpdatedAt>();
            DateTime now = DateTime.Now;

            //When adding entries update the created_at property.
            foreach (var entry in createdEntries.Where(x => x.State == EntityState.Added))
            {
                entry.Entity.created_at = now;
            }

            //If the entity state is not unchanged, update the updated_at property.
            foreach (var entry in updatedEntries.Where(x => x.State != EntityState.Unchanged))
            {
                entry.Entity.updated_at = now;
            }

            return await base.SaveChangesAsync();
        }
    }
}
