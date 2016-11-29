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
    public class ContactRepo : IRepository<Contact>
    {
        private ContactDataContext _context;
        private DbSet<Contact> _dbSet;

        public ContactRepo(ContactDataContext context)
        {
            this._context = context;
            this._dbSet = context.Set<Contact>();
        }

        public void Delete(int id)
        {
            Contact entityToDelete = _dbSet.Find(id);
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public Contact Get(int Id)
        {
            return _dbSet.Find(Id);
        }

        public IEnumerable<Contact> All()
        {
            return _dbSet.ToList<Contact>();
        }

        public void Insert(Contact obj)
        {
            _dbSet.Attach(obj);
            _context.Entry(obj).State = EntityState.Added;
        }

        public void Update(Contact obj)
        {
            _dbSet.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }


        public IEnumerable<Contact> GetPage(int startPage, int pageSize, ref int totalCount)
        {
            totalCount = _dbSet.Count();
            return _dbSet.OrderBy(c => c.LastName).Skip(startPage * pageSize).Take(pageSize);

        }
    }
}
