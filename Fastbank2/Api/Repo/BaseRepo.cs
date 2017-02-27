namespace Fastbank2.Api.Repo
{
    using Fastbank2.Api.Interfaces;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;


    public class BaseRepository<T> : IRepository<T> where T: class, IDalEntity
    {
        private ApiContext _context;
        private DbSet<T> _dbSet;

        public BaseRepository(ApiContext contxt)
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
        public void Insert(T obj)
        {
            _dbSet.Attach(obj);
            _context.Entry(obj).State = EntityState.Added;
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