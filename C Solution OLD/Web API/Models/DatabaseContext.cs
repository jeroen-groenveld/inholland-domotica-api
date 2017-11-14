using Microsoft.EntityFrameworkCore;

namespace Web_API.Models
{
    public class DatabaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(@"Server=127.0.0.1;Port=3306;Database=domotica;Uid=root;Pwd=root;");
        }

        //Add the tables here.
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}
