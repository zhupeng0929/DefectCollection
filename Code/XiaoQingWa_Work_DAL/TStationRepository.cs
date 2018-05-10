

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
                using (IDbConnection conn = new SqlConnection(GetConnstr))
                {
                    string strSql = "delete from TStation where StationId=@StationId";
                    var param = new { StationId = id };
                    var result = conn.Execute(strSql, param);
                    if (result > 0)
                        return true;
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
                    string strSql = "delete from  TStation where StationId in @StationId ";

                    DynamicParameters param = new DynamicParameters();
                    param.Add("StationId", ids);
                    var result = conn.Execute(strSql, param);
                    if (result > 0)
                        return true;
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
                mResult = conn.Get<TStationEntity>(id);
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
    }
}