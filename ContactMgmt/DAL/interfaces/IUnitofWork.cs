using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactMgmtApp.DAL.Repos;

namespace ContactMgmtApp.DAL.interfaces
{
    public interface IUnitofWork : IDisposable
    {
        ContactRepo ContactRepository {get; }
        GroupsRepo GroupsRepository { get;  }

        void Save();
    }
}
