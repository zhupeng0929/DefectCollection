using System.Data;
using System.Collections.Generic;
using XiaoQingWa_Work_Model.Entity;
namespace XiaoQingWa_Work_IDAL
{
    public interface ITTaskWorkerRepository : IDependency<TTaskWorkerEntity>
    {
        bool AddTTaskWorker(TTaskWorkerEntity model);
        bool AddTTaskWorker(TTaskWorkerEntity model, IDbConnection conn, IDbTransaction trans);
        bool DelTTaskWorker(int id);
        bool DelTTaskWorkerBatch(int[] ids);
        TTaskWorkerEntity GetTTaskWorker(int id);
        List<TTaskWorkerEntity> GetTTaskWorkerList();
        bool UpdateTTaskWorker(TTaskWorkerEntity entity);
        bool UpdateTTaskWorker(TTaskWorkerEntity entity, IDbConnection conn, IDbTransaction trans);
    }
}
