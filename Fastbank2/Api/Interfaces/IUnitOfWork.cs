namespace Fastbank2.Api.Interfaces
{
    using System;
    
    public interface IUnitofWork : IDisposable
    {
        /*
        IRepository<IAccount> AccountsRepository { get; }
        IRepository<IUser> UsersRepository { get; }
        IRepository<IBank> BanksRepository { get; }
             */
        void Save();
    }

}