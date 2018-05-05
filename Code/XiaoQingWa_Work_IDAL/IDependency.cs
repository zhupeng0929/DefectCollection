using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaoQingWa_Work_IDAL
{
    public interface IDependency<T> where T : class
    {
        int Add(T model, IDbConnection conn = null, IDbTransaction trans=null);
        Task<int> AddAsync(T model, IDbConnection conn = null, IDbTransaction trans = null);
        bool Del(T model, IDbConnection conn = null, IDbTransaction trans = null);
        T GetSingle(int id);
        List<T> GetList();
        Task<IEnumerable<T>> GetListAsync();
        bool Update(T entity, IDbConnection conn = null, IDbTransaction trans = null);
    }
}
