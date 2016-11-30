using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ContactMgmtApp.DAL.Model;

namespace ContactMgmtApp.DAL
{
    public class ContactDataContext : DbContext
    {
        public ContactDataContext() : base()
        {
            Database.SetInitializer(new ContactDataInitializer());
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Contact>()
               .HasMany<Group>(c => c.Groups)
               .WithMany(g => g.Contacts)
               .Map(cg =>
               {
                   cg.MapLeftKey("ContactId");
                   cg.MapRightKey("GroupId");
                   cg.ToTable("ContactGroup");
               });
        }
    }
}
