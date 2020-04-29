using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class DemoDbContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }

    }
}
