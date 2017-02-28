    
    namespace Fastbank2.Api.Model
    {
        using Fastbank2.Api.Interfaces;
        public class Account : IDalEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public User AccountUser { get; set; }
        }
    }