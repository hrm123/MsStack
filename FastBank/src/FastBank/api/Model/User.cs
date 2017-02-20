using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastBank.api.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastBank.api.Model
{
    public class User : IDalEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual ICollection<Account> UserAccounts { get; set; }

        public User()
        {
            this.UserAccounts = new HashSet<Account>();
        }
    }
}
