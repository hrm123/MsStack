using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastBank.api.Interfaces;
using FastBank.api.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastBank.api.Model
{
    public class Bank : IDalEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual ICollection<User> Users { get; set; }

        public Bank()
        {
            this.Users = new HashSet<User>();
        }
        
    }
}
