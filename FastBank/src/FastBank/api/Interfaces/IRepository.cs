using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastBank.api.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> All();
        IEnumerable<T> GetPage(int startPage, int pageSize, ref int totalCount);
        T Get(int Id);
        void Insert(T obj);
        void Delete(int id);
        void Update(T obj);
    }
}
