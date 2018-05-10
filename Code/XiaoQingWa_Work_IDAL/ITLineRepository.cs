using System.Data;
using System.Collections.Generic;
using XiaoQingWa_Work_Model.Entity;
namespace XiaoQingWa_Work_IDAL
{
    public interface ITLineRepository : IDependency<TLineEntity>
    {
        bool AddTLine(TLineEntity model);
        bool AddTLine(TLineEntity model, IDbConnection conn, IDbTransaction trans);
        bool DelTLine(int id);
        bool DelTLineBatch(int[] ids);
        TLineEntity GetTLine(int id);
        bool UpdateTLine(TLineEntity entity);
        bool UpdateTLine(TLineEntity entity, IDbConnection conn, IDbTransaction trans);

        List<TLineEntity> GetLineInfoListByQueryModel(LineQuery lineQuery);
    }
}
