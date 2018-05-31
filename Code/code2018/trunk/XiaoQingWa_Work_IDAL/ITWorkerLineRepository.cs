using System.Data;
using System.Collections.Generic;
using XiaoQingWa_Work_Model.Entity;
namespace XiaoQingWa_Work_IDAL
{
    public interface ITWorkerLineRepository : IDependency<TWorkerLineEntity>
    {
        bool AddTWorkerLine(TWorkerLineEntity model);
        bool AddTWorkerLine(TWorkerLineEntity model, IDbConnection conn, IDbTransaction trans);
        bool DelTWorkerLine(int id);
        bool DelTWorkerLineBatch(int[] ids);
        List<TWorkerLineEntity> GetTWorkerLineList(int wid);
        List<TWorkerLineEntity> GetTWorkerLineList(string lineCode);
        bool UpdateTWorkerLine(TWorkerLineEntity entity);
        bool UpdateTWorkerLine(TWorkerLineEntity entity, IDbConnection conn, IDbTransaction trans);
    }
}
