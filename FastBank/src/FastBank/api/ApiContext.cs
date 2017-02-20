using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FastBank.api.Model;

namespace FastBank.api
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
            var builder = new DbContextOptionsBuilder<ApiContext>();
            builder.UseInMemoryDatabase();
            options = builder.Options;

        }

        public DbSet<User> Users { get; set; }
        public DbSet<User> Bank { get; set; }
        public DbSet<User> Account { get; set; }

        
    }
}
