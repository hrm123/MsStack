namespace Fastbank2.Api.Repo
{
    using Fastbank2.Api.Model;
    using Fastbank2.Api.Interfaces;
    using System.Collections.Generic;
    using System.Linq;


    public class ApiHelper 
    {
        
        IUnitofWork _contxt = null;
        public ApiHelper(IUnitofWork contxt)
        {
            _contxt = contxt;
        }

        public User GetUser(int id)
        {
            return _contxt.UsersRepository.Get((int)id);
        }

        public List<User> GetUsers(int id)
        {
            return _contxt.BanksRepository.Get(b => b.Id == id,null,"Users").ToList()[0].Users.ToList();
        }

        
    }
}