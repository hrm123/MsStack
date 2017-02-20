using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastBank.api.Interfaces
{
    public interface IUnitofWork : IDisposable
    {
        //ContactRepo ContactRepository { get; }
        //GroupsRepo GroupsRepository { get; }

        void Save();
    }
}
