
/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////数据操作类//////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////

//---------------------------------------------------------------------------------
// <copyright company="XiaoQingWa" file="tRecordDetailRespository.cs" >
//      Author: Peng.Zhu
//      Create Time: 2018/5/5 20:59:35
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
    /// TRecordDetailDAL数据访问类
    /// </summary>
    public partial class TRecordDetailRepository : BaseRepository<TRecordDetailEntity>, ITRecordDetailRepository
    {
        /// <summary>
        /// 新增实体
        /// </summary>
        public bool AddTRecordDetail(TRecordDetailEntity model)
        {
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                var result = conn.Insert<int>(model);
                if (result > 0)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 新增实体--事物
        /// </summary>
        public bool AddTRecordDetail(TRecordDetailEntity model, IDbConnection conn, IDbTransaction trans)
        {
            var result = conn.Insert<int>(model, trans);
            if (result > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns></returns>
        public bool DelTRecordDetail(int id)
        {
            if (id > 0)
            {
                using (IDbConnection conn = new SqlConnection(GetConnstr))
                {
                    string strSql = "delete from TRecordDetail where RId=@RId";
                    var param = new { RId = id };
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
        public bool DelTRecordDetailBatch(int[] ids)
        {
            if (ids.Length > 0)
            {
                using (IDbConnection conn = new SqlConnection(GetConnstr))
                {
                    string strSql = "delete from  TRecordDetail where RId in @RId ";

                    DynamicParameters param = new DynamicParameters();
                    param.Add("RId", ids);
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
        public TRecordDetailEntity GetTRecordDetail(int id)
        {
            var mResult = new TRecordDetailEntity();
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                mResult = conn.Get<TRecordDetailEntity>(id);
            }
            return mResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TRecordDetailEntity> GetTRecordDetailList()
        {
            var mResult = new List<TRecordDetailEntity>();
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                mResult = conn.GetList<TRecordDetailEntity>().ToList();
            }
            return mResult;
        }
        /// <summary>
        /// 更新实体列表
        /// </summary>
        /// <returns></returns>
        public bool UpdateTRecordDetail(TRecordDetailEntity entity)
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
        public bool UpdateTRecordDetail(TRecordDetailEntity entity, IDbConnection conn, IDbTransaction trans)
        {
            int row;
            row = conn.Update(entity, trans);
            return row > 0;
        }


        public List<TRecordDetailEntity> GetRecordDetailListByQueryModel(TRecordDetailEntity recordDetailQuery)
        {
            var mResult = new List<TRecordDetailEntity>();
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                StringBuilder strSql = new StringBuilder("Select * from tRecordDetail   where 1=1 ");

                if (!string.IsNullOrWhiteSpace(recordDetailQuery.BillNO))
                {
                    strSql.Append(" and BillNO=@BillNO  ");
                }
                if (!string.IsNullOrWhiteSpace(recordDetailQuery.LineCode))
                {
                    strSql.Append(" and LineCode=@LineCode  ");
                }
                if (!string.IsNullOrWhiteSpace(recordDetailQuery.StationCode))
                {
                    strSql.Append(" and StationCode=@StationCode  ");
                }
                if (recordDetailQuery.WId > 0)
                {
                    strSql.Append(" and WId=@WId  ");
                }
                var param = new
                {
                    recordDetailQuery.BillNO,
                    recordDetailQuery.LineCode,
                    recordDetailQuery.StationCode,
                    recordDetailQuery.WId
                };

                mResult = conn.Query<TRecordDetailEntity>(strSql.ToString(), param).ToList();
            }
            return mResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool UpdateRecordStatu(int id, int state)
        {
            if (id > 0)
            {
                using (IDbConnection conn = new SqlConnection(GetConnstr))
                {
                    string strSql = "update  tRecordDetail   set Status=@Status where RId	=@RId	";
                    var param = new { RId = id, Status = state };
                    var result = conn.Execute(strSql, param);
                    if (result > 0)
                        return true;
                }
            }
            return false;
        }
        public bool UpdateTask(string[] ids, string BillNO)
        {

            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                conn.Open();
                var taskModel = conn.Get<TTaskEntity>(BillNO);
                if (taskModel != null)
                {
                    using (IDbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {

                            foreach (var id in ids)
                            {
                                string strSql3 = "update  tRecordDetail    set BillNO=@BillNO,TName=@TName where RId=@RId ";
                                var param3 = new { BillNO, taskModel.TName, RId = id };
                                conn.Execute(strSql3, param3, trans);
                            }
                            trans.Commit();
                            return true;

                        }
                        catch
                        {
                            trans.Rollback();
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }

            }
            return false;
        }
    }
}
