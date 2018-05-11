using System.Data;
using System.Collections.Generic;
using XiaoQingWa_Work_Model.Entity;
namespace XiaoQingWa_Work_IDAL
{
    public interface ITRecordDetailRepository : IDependency<TRecordDetailEntity>
    {
        bool AddTRecordDetail(TRecordDetailEntity model);
        bool AddTRecordDetail(TRecordDetailEntity model, IDbConnection conn, IDbTransaction trans);
        bool DelTRecordDetail(int id);
        bool DelTRecordDetailBatch(int[] ids);
        TRecordDetailEntity GetTRecordDetail(int id);
        List<TRecordDetailEntity> GetTRecordDetailList();
        bool UpdateTRecordDetail(TRecordDetailEntity entity);
        bool UpdateTRecordDetail(TRecordDetailEntity entity, IDbConnection conn, IDbTransaction trans);
    }
}