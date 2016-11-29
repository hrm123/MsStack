using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactMgmtApp.DAL.interfaces;

namespace ContactMgmtApp.DAL.Repos
{
    public class UnitOfWork : IUnitofWork
    {
        private ContactDataContext _context = new ContactDataContext();
        private ContactRepo _contactRepository;
        private GroupsRepo _groupsRepository;

        public ContactRepo ContactRepository
        {
            get
            {

                if (this._contactRepository == null)
                {
                    this._contactRepository = new ContactRepo(_context);
                }
                return _contactRepository;
            }
        }

        public GroupsRepo GroupsRepository
        {
            get
            {

                if (this._groupsRepository == null)
                {
                    this._groupsRepository = new GroupsRepo(_context);
                }
                return _groupsRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
