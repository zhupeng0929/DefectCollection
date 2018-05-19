using System.Data;
using System.Collections.Generic;
using XiaoQingWa_Work_Model.Entity;
namespace XiaoQingWa_Work_IDAL
{
    public interface ITWorkScheduleRepository : IDependency<TWorkScheduleEntity>
    {
        bool AddTWorkSchedule(TWorkScheduleEntity model);
        bool AddTWorkSchedule(TWorkScheduleEntity model, IDbConnection conn, IDbTransaction trans);
        bool DelTWorkSchedule(int id);
        bool DelTWorkScheduleBatch(int[] ids);
        TWorkScheduleEntity GetTWorkSchedule(int id);
        List<TWorkScheduleEntity> GetTWorkScheduleList();
        bool UpdateTWorkSchedule(TWorkScheduleEntity entity);
        bool UpdateTWorkSchedule(TWorkScheduleEntity entity, IDbConnection conn, IDbTransaction trans);
        bool AddTWorkScheduleTran(TTaskEntity taskEntity, List<TWorkScheduleEntity> list,List<TWorkerEntity> workerEntities);
    }
}

