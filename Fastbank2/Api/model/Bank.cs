namespace Fastbank2.Api.Model
{
    using System.Collections.Generic;
    using Fastbank2.Api.Interfaces;
    public class Bank : IDalEntity
    {
        public int Id { get; set; }
        public virtual ICollection<User> Users { get; set; }

        public Bank()
        {
            this.Users = new HashSet<User>();
        }
        
    }
}