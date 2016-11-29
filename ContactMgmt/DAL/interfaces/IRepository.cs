using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactMgmtApp.DAL.interfaces
{
    interface IRepository<T>  where T:IDalEntity
    {
        IEnumerable<T> All();
        IEnumerable<T> GetPage(int startPage, int pageSize, ref int totalCount);
        T Get(int Id);
        void Insert(T obj);
        void Delete(int id);
        void Update(T obj);
    }
}
