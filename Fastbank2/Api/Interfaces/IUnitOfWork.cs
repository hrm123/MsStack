namespace Fastbank2.Api.Interfaces
{
    using System;
    using Fastbank2.Api.Repo;
    using Fastbank2.Api.Model;


    public interface IUnitofWork : IDisposable
    {
        BaseRepository<Account> AccountsRepository { get; }
        BaseRepository<User> UsersRepository { get; }
        BaseRepository<Bank> BanksRepository { get; }
        
        void Save();
    }

}