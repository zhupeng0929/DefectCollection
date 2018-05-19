

/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////数据操作类//////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////

//---------------------------------------------------------------------------------
// <copyright company="XiaoQingWa" file="tStationRespository.cs" >
//      Author: Peng.Zhu
//      Create Time: 2018/5/5 21:01:56
//      Description: 
// </copyright>
//---------------------------------------------------------------------------------

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XiaoQingWa_Work_IDAL;
using XiaoQingWa_Work_Model.Entity;

namespace XiaoQingWa_Work_DAL
{
    /// <summary>
    /// TStationDAL数据访问类
    /// </summary>
    public partial class TStationRepository : BaseRepository<TStationEntity>, ITStationRepository
    {
        /// <summary>
        /// 新增实体
        /// </summary>
        public bool AddTStation(TStationEntity model)
        {
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                var result = conn.Insert<string>(model);
                if (model != null && !string.IsNullOrWhiteSpace(model.LineCode))
                {
                    string strSql4 = "update  tLine    set StationCount=StationCount+1 where LCode=@LCode ";
                    var param4 = new { LCode = model.LineCode };
                    conn.Execute(strSql4, param4);
                }
                if (result != null)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 新增实体--事物
        /// </summary>
        public bool AddTStation(TStationEntity model, IDbConnection conn, IDbTransaction trans)
        {
            var result = conn.Insert<string>(model, trans);

            if (result != null)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns></returns>
        public bool DelTStation(int id)
        {
            if (id > 0)
            {
                var model = GetTStation(id);
                using (IDbConnection conn = new SqlConnection(GetConnstr))
                {
                    conn.Open();
                    using (IDbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            string strSql = "delete from TStation where StationId=@StationId";
                            var param = new { StationId = id };

                            if (model != null && (!string.IsNullOrWhiteSpace(model.GoodBtnId) || !string.IsNullOrWhiteSpace(model.BadBtnId)))
                            {
                                string strSql3 = "update  tCounter   set Status=0 where CountNo=@GoodBtnId or CountNo=@BadBtnId";
                                var param3 = new { model.GoodBtnId, model.BadBtnId };
                                conn.Execute(strSql3, param3, trans);
                            }
                            if (model != null && !string.IsNullOrWhiteSpace(model.LineCode))
                            {
                                string strSql4 = "update  tLine    set StationCount=StationCount-1 where LCode=@LCode ";
                                var param4 = new { LCode = model.LineCode };
                                conn.Execute(strSql4, param4, trans);
                            }
                            var result = conn.Execute(strSql, param, trans);
                            if (result > 0)
                            {
                                trans.Commit();
                                return true;
                            }
                            else
                            {
                                trans.Rollback();
                                return false;
                            }
                        }
                        catch
                        {
                            trans.Rollback();
                            return false;
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DelTStationBatch(int[] ids)
        {
            if (ids.Length > 0)
            {
                using (IDbConnection conn = new SqlConnection(GetConnstr))
                {
                    conn.Open();
                    using (IDbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (var id in ids)
                            {
                                var model = GetTStation(id);
                                if (model != null && (!string.IsNullOrWhiteSpace(model.GoodBtnId) || !string.IsNullOrWhiteSpace(model.BadBtnId)))
                                {
                                    string strSql3 = "update  tCounter   set Status=0 where CountNo=@GoodBtnId or CountNo=@BadBtnId";
                                    var param3 = new { GoodBtnId = model.GoodBtnId ?? "", BadBtnId = model.BadBtnId ?? "" };
                                    conn.Execute(strSql3, param3, trans);
                                }
                                if (model != null && !string.IsNullOrWhiteSpace(model.LineCode))
                                {
                                    string strSql4 = "update  tLine    set StationCount=StationCount-1 where LCode=@LCode ";
                                    var param4 = new { LCode = model.LineCode };
                                    conn.Execute(strSql4, param4, trans);
                                }
                            }

                            string strSql = "delete from  TStation where StationId in @StationId ";
                            DynamicParameters param = new DynamicParameters();
                            param.Add("StationId", ids);
                            var result = conn.Execute(strSql, param, trans);
                            if (result > 0)
                            {
                                trans.Commit();
                                return true;
                            }
                            else
                            {
                                trans.Rollback();
                                return false;
                            }
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            return false;
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 获取类别实体根据ID
        /// </summary>
        /// <returns></returns>
        public TStationEntity GetTStation(int id)
        {
            var mResult = new TStationEntity();
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                StringBuilder strSql = new StringBuilder("Select top 1 * from tStation  where 1=1 and StationId=@StationId ");

                var param = new { StationId = id };
                mResult = conn.Query<TStationEntity>(strSql.ToString(), param).FirstOrDefault();
            }
            return mResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TStationEntity> GetTStationList()
        {
            var mResult = new List<TStationEntity>();
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                mResult = conn.GetList<TStationEntity>().ToList();
            }
            return mResult;
        }
        /// <summary>
        /// 更新实体列表
        /// </summary>
        /// <returns></returns>
        public bool UpdateTStation(TStationEntity entity)
        {
            int row;
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {

                var baseModel = GetSingle(entity.StationCode);
                if (baseModel.LineCode != entity.LineCode)
                {
                    if (baseModel != null && !string.IsNullOrWhiteSpace(baseModel.LineCode))
                    {
                        string strSql4 = "update  tLine    set StationCount=StationCount-1 where LCode=@LCode ";
                        var param4 = new { LCode = baseModel.LineCode };
                        conn.Execute(strSql4, param4);
                    }
                    if (entity != null && !string.IsNullOrWhiteSpace(entity.LineCode))
                    {
                        string strSql4 = "update  tLine    set StationCount=StationCount+1 where LCode=@LCode ";
                        var param4 = new { LCode = entity.LineCode };
                        conn.Execute(strSql4, param4);
                    }
                }


                row = conn.Update(entity);




            }
            return row > 0;
        }
        /// <summary>
        /// 更新实体列表--事物
        /// </summary>
        /// <returns></returns>
        public bool UpdateTStation(TStationEntity entity, IDbConnection conn, IDbTransaction trans)
        {
            int row;
            row = conn.Update(entity, trans);
            return row > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool UpdateStatu(int id, int state)
        {
            if (id > 0)
            {
                using (IDbConnection conn = new SqlConnection(GetConnstr))
                {
                    string strSql = "update  tStation  set Status=@Status where StationId=@StationId";
                    var param = new { StationId = id, Status = state };
                    var result = conn.Execute(strSql, param);
                    if (result > 0)
                        return true;
                }
            }
            return false;
        }

        public List<TStationEntity> GetStationListByQueryModel(StationQuery lineQuery)
        {
            var mResult = new List<TStationEntity>();
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                StringBuilder strSql = new StringBuilder("Select * from tStation  where 1=1 ");

                if (!string.IsNullOrWhiteSpace(lineQuery.keyWords))
                {
                    strSql.Append(" and  (StationCode=@keyWords or StationName=@keyWords or StationFullName=@keyWords or LineCode=@keyWords or LineName=@keyWords) ");
                }
                var param = new
                {
                    keyWords = lineQuery.keyWords
                };

                mResult = conn.Query<TStationEntity>(strSql.ToString(), param).ToList();
            }
            return mResult;
        }
        public List<TStationEntity> GetStationListByLineCode(string lineCode)
        {
            var mResult = new List<TStationEntity>();
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                StringBuilder strSql = new StringBuilder("Select * from tStation  where 1=1 ");

                if (!string.IsNullOrWhiteSpace(lineCode))
                {
                    strSql.Append(" and  LineCode=@lineCode ");
                }
                var param = new
                {
                    lineCode
                };
                strSql.Append(" order by StationIndex ");
                mResult = conn.Query<TStationEntity>(strSql.ToString(), param).ToList();
            }
            return mResult;
        }
        /// <summary>
        /// 绑定设备
        /// </summary>
        /// <param name="StationId">工位id</param>
        /// <param name="GoodBtnId">设备code</param>
        /// <param name="BadBtnId">设备code</param>
        /// <param name="type">0好件，1坏件</param>
        /// <returns></returns>
        public bool BondCounter(int StationId, string GoodBtnId, string BadBtnId, int type)
        {
            var result = false;
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        int n, m;
                        if (type == 0)
                        {
                            string strSql1 = "update  [tStation]   set [GoodBtnId]=@GoodBtnId where StationId=@StationId";
                            var param1 = new { StationId, GoodBtnId };
                            n = conn.Execute(strSql1, param1, trans);

                            string strSql3 = "update  tCounter   set Status=@Status where CountNo=@CountNo";
                            var param3 = new { CountNo = GoodBtnId, Status = 1 };
                            m = conn.Execute(strSql3, param3, trans);
                        }
                        else
                        {
                            string strSql2 = "update  [tStation]   set [BadBtnId]=@BadBtnId where StationId=@StationId";
                            var param2 = new { StationId, BadBtnId };
                            n = conn.Execute(strSql2, param2, trans);

                            string strSql3 = "update  tCounter   set Status=@Status where CountNo=@CountNo";
                            var param3 = new { CountNo = BadBtnId, Status = 1 };
                            m = conn.Execute(strSql3, param3, trans);
                        }


                        if (n > 0 && m > 0)
                        {
                            trans.Commit();
                            result = true;
                        }
                        else
                        {
                            trans.Rollback();
                            result = false;
                        }


                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        result = false;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 解除绑定
        /// </summary>
        /// <param name="id">工位id</param>
        /// <param name="code">设备code</param>
        /// <param name="type">0好件，1坏件</param>
        /// <returns></returns>
        public bool UnBindStation(int id, string code, int type)
        {
            var result = false;
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        int n, m;
                        if (type == 0)
                        {
                            string strSql1 = "update  [tStation]   set [GoodBtnId]='' where StationId=@StationId";
                            var param1 = new { StationId = id };
                            n = conn.Execute(strSql1, param1, trans);
                        }
                        else
                        {
                            string strSql2 = "update  [tStation]   set [BadBtnId]='' where StationId=@StationId";
                            var param2 = new { StationId = id };
                            n = conn.Execute(strSql2, param2, trans);
                        }

                        string strSql3 = "update  tCounter   set Status=@Status where CountNo=@CountNo";
                        var param3 = new { CountNo = code, Status = 0 };
                        m = conn.Execute(strSql3, param3, trans);

                        if (n > 0 && m > 0)
                        {
                            trans.Commit();
                            result = true;
                        }
                        else
                        {
                            trans.Rollback();
                            result = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        result = false;
                    }
                }
            }
            return result;
        }
    }
}