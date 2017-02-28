namespace Fastbank2.Api.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Fastbank2.Api.Model;
    using System.Linq;
    using System.Linq.Expressions;
    
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> All();
        IEnumerable<T> GetPage(int startPage, int pageSize, ref int totalCount);
        T Get(int Id);

        IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
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