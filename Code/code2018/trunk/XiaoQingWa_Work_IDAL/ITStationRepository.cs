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
        List<TStationEntity> GetStationListByQueryModel(StationQuery lineQuery);
        bool UpdateStatu(int id, int state);
        bool BondCounter(int StationId, string GoodBtnId, string BadBtnId, int type);
        bool UnBindStation(int id, string code, int type);
        List<TStationEntity> GetStationListByLineCode(string lineCode);
    }
}