using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Models
{
    public class LMSDbContext : DbContext
    {
        public LMSDbContext(DbContextOptions<LMSDbContext> Options) : base(Options)
        {

        }
           public DbSet<Users> Users {  get; set; }
        
    }
}
