using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactMgmtApp.DAL.interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactMgmtApp.DAL.Model
{
    public class Group : IDalEntity
    {
        public Group()
        {
            this.Contacts = new HashSet<Contact>();
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
