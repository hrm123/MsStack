namespace Fastbank2.Api.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Fastbank2.Api.Model;
    
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> All();
        IEnumerable<T> GetPage(int startPage, int pageSize, ref int totalCount);
        T Get(int Id);
        void Insert(T obj);
        void Delete(int id);
        void Update(T obj);
    }

    public interface IUsersRepository:IRepository<User>
    {

    }
    public interface IBankRepository:IRepository<Bank>
    {

    }
    public interface IAccountRepository:IRepository<Account>
    {

    }
}