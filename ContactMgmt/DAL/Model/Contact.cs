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
    public class Contact : IDalEntity
    {
        public Contact()
        {
            this.Groups = new HashSet<Group>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public byte[] Photo { get; set; }
        public string GroupIdsTemp { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
}
