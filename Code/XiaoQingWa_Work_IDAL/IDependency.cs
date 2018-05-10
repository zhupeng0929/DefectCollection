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
        int AddReturnInt(T model, IDbConnection conn = null, IDbTransaction trans=null);
        string AddReturnStr(T model, IDbConnection conn = null, IDbTransaction trans = null);
        bool Del(T model, IDbConnection conn = null, IDbTransaction trans = null);
        T GetSingle(object id);
        List<T> GetList();
        bool Update(T entity, IDbConnection conn = null, IDbTransaction trans = null);
    }
}
