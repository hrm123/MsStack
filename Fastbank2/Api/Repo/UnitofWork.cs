namespace Fastbank2.Api.Repo
{
    using Fastbank2.Api.Interfaces;
    using Fastbank2.Api.Model;
    using System;
    using Microsoft.EntityFrameworkCore;
    public class UnitofWork : IUnitofWork
    {
        #region Fields
            private bool disposed = false;
            public IRepository<Account> AccountsRepository { get; private set; }
            public IRepository<User> UsersRepository { get; private set;}
            public IRepository<Bank> BanksRepository { get; private set;}
        #endregion Fields

        public UnitofWork()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApiContext>();
            //optionsBuilder.UseSqlite("Filename=./blog.db");
            optionsBuilder.UseInMemoryDatabase();
            ApiContext contx = new ApiContext(optionsBuilder.Options);
            BanksRepository = new BaseRepository<Model.Bank>(contx);
            UsersRepository = new BaseRepository<Model.User>(contx);
            AccountsRepository = new BaseRepository<Model.Account>(contx);

        }

        public void Save()
        {

        }

        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if(!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if(disposing)
                {
                    // Dispose managed resources.
                    
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.
                

                // Note disposing has been done.
                disposed = true;

            }
        }
        
        
    }
}