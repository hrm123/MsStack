namespace Fastbank2.Api.Repo
{
    using Fastbank2.Api.Model;
    using Fastbank2.Api.Interfaces;
    using System.Collections.Generic;
    using System.Linq;


    public class BankFacade 
    {
        BaseRepository<Bank> _bankRepo;
        BaseRepository<User> _userRepo;
        BaseRepository<Account> _accountRepo;
        
        public BankFacade(ApiContext contxt)
        {
            _bankRepo = new BaseRepository<Model.Bank>(contxt);
            _userRepo = new BaseRepository<Model.User>(contxt);
            _accountRepo = new BaseRepository<Model.Account>(contxt);
        }

        public User GetUser(int id)
        {
            return _userRepo.Get((int)id);
        }

        public List<User> GetUsers(int id)
        {
            return _userRepo.All().ToList();
        }

        
    }
}