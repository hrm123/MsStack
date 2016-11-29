using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ContactMgmtApp.DAL.Model;

namespace ContactMgmtApp.DAL
{
    public class ContactDataInitializer : DropCreateDatabaseAlways<ContactDataContext>
    {
        protected override void Seed(ContactDataContext context)
        {
            IList<Group> defaultGroups = new List<Group>();

            defaultGroups.Add(new Group() { GroupName = "Family"});
            defaultGroups.Add(new Group() { GroupName = "Friend" });
            defaultGroups.Add(new Group() { GroupName = "Colleague"});
            defaultGroups.Add(new Group() { GroupName = "Associate" });

            foreach (Group grp in defaultGroups)
                context.Groups.Add(grp);

            base.Seed(context);
        }
    }
}
