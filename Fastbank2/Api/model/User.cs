
namespace Fastbank2.Api.Model
{
    using System.Collections.Generic;
    using Fastbank2.Api.Interfaces;
    public class User : IDalEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Bank UserBank { get; set; }
        public virtual ICollection<Account> UserAccounts { get; set; }

        public User()
        {
            this.UserAccounts = new HashSet<Account>();
        }
    }
}
