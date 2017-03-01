namespace Fastbank2.Api.Repo
{
    using Fastbank2.Api.Interfaces;
    using Fastbank2.Api.Model;
    using System;
    using Microsoft.EntityFrameworkCore;
    public class UnitofWork : IUnitofWork
    {
        #region Fields
            private bool _disposed = false;
            private ApiContext _contx = null;
            public BaseRepository<Account> AccountsRepository { get; private set; }
            public BaseRepository<User> UsersRepository { get; private set;}
            public BaseRepository<Bank> BanksRepository { get; private set;}
        #endregion Fields

        public UnitofWork()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApiContext>();
            //optionsBuilder.UseSqlite("Filename=./blog.db");
            optionsBuilder.UseInMemoryDatabase();
            _contx = new ApiContext(optionsBuilder.Options);
            BanksRepository = new BaseRepository<Model.Bank>(_contx);
            UsersRepository = new BaseRepository<Model.User>(_contx);
            AccountsRepository = new BaseRepository<Model.Account>(_contx);

        }
        public UnitofWork(ApiContext contxt)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApiContext>();
            //optionsBuilder.UseSqlite("Filename=./blog.db");
            optionsBuilder.UseInMemoryDatabase();
            _contx = contxt;
            BanksRepository = new BaseRepository<Model.Bank>(_contx);
            UsersRepository = new BaseRepository<Model.User>(_contx);
            AccountsRepository = new BaseRepository<Model.Account>(_contx);

        }

        public void Save()
        {
            _contx.SaveChanges();
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
            if(!this._disposed)
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
                _disposed = true;

            }
        }
        
        
    }
}