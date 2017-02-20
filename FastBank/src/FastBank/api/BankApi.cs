using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastBank.api.Repos;
using FastBank.api.Model;

namespace FastBank.api
{
    public class BankApi
    {
        BaseRepository<Bank> _bankRepo;
        BaseRepository<User> _userRepo;
        BaseRepository<Account> _accountRepo;
        
        public BankApi(ApiContext contxt)
        {
            _bankRepo = new Repos.BaseRepository<Model.Bank>(contxt);
            _userRepo = new Repos.BaseRepository<Model.User>(contxt);
            _accountRepo = new Repos.BaseRepository<Model.Account>(contxt);
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
