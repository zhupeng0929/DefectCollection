using System.Data;
using System.Collections.Generic;
using XiaoQingWa_Work_Model.Entity;
namespace XiaoQingWa_Work_IDAL
{
    public interface ITCounterRepository : IDependency<TCounterEntity>
    {
        bool AddTCounter(TCounterEntity model);
        bool AddTCounter(TCounterEntity model, IDbConnection conn, IDbTransaction trans);
        bool DelTCounter(int id);
        bool DelTCounterBatch(int[] ids);
        TCounterEntity GetTCounter(int id);
        List<TCounterEntity> GetTCounterList();
        bool UpdateTCounter(TCounterEntity entity);
        bool UpdateTCounter(TCounterEntity entity, IDbConnection conn, IDbTransaction trans);
        List<TCounterEntity> GetCounterListByQueryModel(string CountNo);
        bool UpdateStatu(int id, int state);
    }
}
