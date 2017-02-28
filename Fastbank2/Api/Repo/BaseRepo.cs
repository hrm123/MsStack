namespace Fastbank2.Api.Repo
{
    using System;
    using Fastbank2.Api.Interfaces;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Linq.Expressions;


    public class BaseRepository<T> : IRepository<T> where T: class, IDalEntity
    {
        private ApiContext _context;
        private DbSet<T> _dbSet;

        internal BaseRepository(ApiContext contxt)
        {
            this._dbSet = contxt.Set<T>();
            this._context = contxt;
        }

        public IEnumerable<T> All()
        {
            return _dbSet.ToList<T>();
        }
        public IEnumerable<T> GetPage(int startPage, int pageSize, ref int totalCount)
        {
            totalCount = _dbSet.Count();
            return _dbSet.Skip(startPage * pageSize).Take(pageSize);
        }
        public T Get(int Id)
        {
            return _dbSet.Find(Id);
        }


        public virtual IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }

        }

        public void Insert(T obj)
        {
            //_dbSet.Attach(obj);
            //_context.Entry(obj).State = EntityState.Added;
            _dbSet.Add(obj);
        }
        public void Delete(int id)
        {
            T entityToDelete = _dbSet.Find(id);
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }
        public void Update(T obj)
        {
            _dbSet.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;

        }
    }

}