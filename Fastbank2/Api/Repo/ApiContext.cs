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
 
        public DbSet<User> Users { get; set; }
 
        public DbSet<Account> Posts { get; set; }

        public DbSet<Account> Banks { get; set; }
    }
}