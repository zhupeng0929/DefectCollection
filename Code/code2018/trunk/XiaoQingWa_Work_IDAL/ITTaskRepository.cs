using System.Data;
using System.Collections.Generic;
using XiaoQingWa_Work_Model.Entity;
namespace XiaoQingWa_Work_IDAL
{
    public interface ITTaskRepository : IDependency<TTaskEntity>
    {
        bool AddTTask(TTaskEntity model);
        bool AddTTask(TTaskEntity model, IDbConnection conn, IDbTransaction trans);
        bool DelTTask(int id);
        bool DelTTaskBatch(int[] ids);
        TTaskEntity GetTTask(int id);
        List<TTaskEntity> GetTTaskList();
        bool UpdateTTask(TTaskEntity entity);
        List<TTaskEntity> GetTTaskList(LineQuery lineQuery);
        bool UpdateTTask(TTaskEntity entity, IDbConnection conn, IDbTransaction trans);
        List<TTaskEntity> GetTTaskList(string sql);
        List<TTaskEntity> GetTOrderTaskList();

    }
}
