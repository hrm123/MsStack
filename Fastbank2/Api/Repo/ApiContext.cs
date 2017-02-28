using Microsoft.EntityFrameworkCore;
using Fastbank2.Api.Model;
 
namespace Fastbank2.Api.Repo
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }
 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase();
        }
        public DbSet<User> Users { get; set; }
 
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Bank> Banks { get; set; }
    }
}