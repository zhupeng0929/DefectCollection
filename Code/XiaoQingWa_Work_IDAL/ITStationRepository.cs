using System.Data;
using System.Collections.Generic;
using XiaoQingWa_Work_Model.Entity;
namespace XiaoQingWa_Work_IDAL
{
    public interface ITStationRepository : IDependency<TStationEntity>
    {
        bool AddTStation(TStationEntity model);
        bool AddTStation(TStationEntity model, IDbConnection conn, IDbTransaction trans);
        bool DelTStation(int id);
        bool DelTStationBatch(int[] ids);
        TStationEntity GetTStation(int id);
        List<TStationEntity> GetTStationList();
        bool UpdateTStation(TStationEntity entity);
        bool UpdateTStation(TStationEntity entity, IDbConnection conn, IDbTransaction trans);
    }
}