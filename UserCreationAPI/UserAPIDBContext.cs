using Microsoft.EntityFrameworkCore;
using UserCreationAPI.Models;

namespace UserCreationAPI
{
    public class UserAPIDBContext(DbContextOptions<UserAPIDBContext> options):DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(User.CreateAdmin());
        }
    }
}
