using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactMgmtApp.DAL.interfaces;
using System.Linq.Expressions;
using System.Data;
using System.Data.Entity;
using ContactMgmtApp.DAL.Model;

namespace ContactMgmtApp.DAL.Repos
{
    public class GroupsRepo : IRepository<Group>
    {
        private ContactDataContext _context;
        private DbSet<Group> _dbSet;

        public GroupsRepo(ContactDataContext context)
        {
            this._context = context;
            this._dbSet = context.Set<Group>();
        }

        public void Delete(int id)
        {
            Group entityToDelete = _dbSet.Find(id);
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public Group Get(int Id)
        {
            return _dbSet.Find(Id);
        }

        public IEnumerable<Group> All()
        {
            return _dbSet.ToList<Group>();
        }

        public void Insert(Group obj)
        {
            _dbSet.Attach(obj);
            _context.Entry(obj).State = EntityState.Added;
        }

        public void Update(Group obj)
        {
            _dbSet.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

        public IEnumerable<Group> GetPage(int startPage, int pageSize, ref int totalCount)
        {
            totalCount = _dbSet.Count();
            return _dbSet.OrderBy(g => g.GroupName  ).Skip(startPage * pageSize).Take(pageSize);

        }

    }
}
