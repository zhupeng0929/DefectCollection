using System.Data;
using System.Collections.Generic;
using XiaoQingWa_Work_Model.Entity;
using System;

namespace XiaoQingWa_Work_IDAL
{
    public interface ITWorkerRepository : IDependency<TWorkerEntity>
    {
        bool AddTWorker(TWorkerEntity model);
        bool AddTWorker(TWorkerEntity model, string[] LineCode);
        bool AddTWorker(TWorkerEntity model, IDbConnection conn, IDbTransaction trans);
        bool DelTWorker(int id);
        bool DelTWorkerBatch(int[] ids);
        TWorkerEntity GetTWorker(int id);
        List<TWorkerEntity> GetTWorkerList();
        bool UpdateTWorker(TWorkerEntity entity);
        bool UpdateTWorker(TWorkerEntity entity, IDbConnection conn, IDbTransaction trans);
        List<TWorkerEntity> GetWorkerInfoListByQueryModel(WorkerQuery workerQuery);
        List<TWorkerEntity> GetTTaskWorkerListByLineCode(string linecode);
        List<TWorkerEntity> GetTTaskWorkerListByLineCode(string linecode, DateTime dateTime);
        bool UpdateWorkerStatu(int id, int state);
    }
}
