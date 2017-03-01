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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasOne(p => p.AccountUser);
            modelBuilder.Entity<User>()
                .HasMany(p => p.UserAccounts);
            modelBuilder.Entity<User>()
                .HasOne(p => p.UserBank)
                .WithMany(p => p.Users);
                /*
            modelBuilder.Entity<Bank>()
                .HasMany(p => p.Users);
                */

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