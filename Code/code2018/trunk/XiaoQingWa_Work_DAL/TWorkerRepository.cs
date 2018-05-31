/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////数据操作类//////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////

//---------------------------------------------------------------------------------
// <copyright company="XiaoQingWa" file="tWorkerRespository.cs" >
//      Author: Peng.Zhu
//      Create Time: 2018/5/5 20:40:19
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
    /// TWorkerDAL数据访问类
    /// </summary>
    public partial class TWorkerRepository : BaseRepository<TWorkerEntity>, ITWorkerRepository
    {
        /// <summary>
        /// 新增实体
        /// </summary>
        public bool AddTWorker(TWorkerEntity model)
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
        public bool AddTWorker(TWorkerEntity model, IDbConnection conn, IDbTransaction trans)
        {
            var result = conn.Insert<int>(model, trans);
            if (result > 0)
                return true;
            else
                return false;
        }

        public bool AddTWorker(TWorkerEntity model, string[] LineCode)
        {
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                conn.Open();
                using (IDbTransaction tran = conn.BeginTransaction())
                {
                    try
                    {

                        var result = conn.Insert<int>(model, tran);

                        foreach (var code in LineCode)
                        {

                            var linemode = new TWorkerLineEntity()
                            {
                                WId = result,
                                LCode = code

                            };

                            conn.Insert<int>(linemode, tran);
                        }
                        tran.Commit();
                        return true;
                    }

                    catch
                    {
                        tran.Rollback();
                        return false;
                    }
                }
            }
        }




        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns></returns>
        public bool DelTWorker(int id)
        {
            if (id > 0)
            {
                using (IDbConnection conn = new SqlConnection(GetConnstr))
                {
                    string strSql = "delete from TWorker where WId=@WId";
                    var param = new { WId = id };
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
        public bool DelTWorkerBatch(int[] ids)
        {
            if (ids.Length > 0)
            {
                using (IDbConnection conn = new SqlConnection(GetConnstr))
                {
                    string strSql = "delete from  TWorker where WId in @WId ";

                    DynamicParameters param = new DynamicParameters();
                    param.Add("WId", ids);
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
        public TWorkerEntity GetTWorker(int id)
        {
            var mResult = new TWorkerEntity();
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                mResult = conn.Get<TWorkerEntity>(id);
            }
            return mResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TWorkerEntity> GetTWorkerList()
        {
            var mResult = new List<TWorkerEntity>();
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                mResult = conn.GetList<TWorkerEntity>().ToList();
            }
            return mResult;
        }
        /// <summary>
        /// 更新实体列表
        /// </summary>
        /// <returns></returns>
        public bool UpdateTWorker(TWorkerEntity entity)
        {
            int row;
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                conn.Open();
                using (IDbTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        row = conn.Update(entity, tran);
                        if (entity.LineList != null)
                        {
                            conn.DeleteList<TWorkerLineEntity>(new { WId = entity.WId }, tran);
                            entity.LineList.ForEach(l => { conn.Insert(l, tran); });
                        }
                        else
                        {
                            conn.DeleteList<TWorkerLineEntity>(new { WId = entity.WId }, tran);
                        }
                        tran.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return false;
                    }
                }
            }
        }
        /// <summary>
        /// 更新实体列表--事物
        /// </summary>
        /// <returns></returns>
        public bool UpdateTWorker(TWorkerEntity entity, IDbConnection conn, IDbTransaction trans)
        {
            int row;
            row = conn.Update(entity, trans);
            return row > 0;
        }

        public List<TWorkerEntity> GetWorkerInfoListByQueryModel(WorkerQuery workerQuery)
        {
            var mResult = new List<TWorkerEntity>();
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                StringBuilder strSql = new StringBuilder("Select * from tWorker  where 1=1 ");


                if (!string.IsNullOrWhiteSpace(workerQuery.keyWords))
                {
                    strSql.Append(" and  (WName=@keyWords or WNo=@keyWords or WDescript = @keyWords) ");
                }
                var param = new
                {
                    keyWords = workerQuery.keyWords
                };
                mResult = conn.Query<TWorkerEntity>(strSql.ToString(), param).ToList();
                if (mResult != null)
                {
                    mResult.ForEach(t =>
                    {
                        var workerLines = new List<TWorkerLineEntity>();
                        var str = "select * from tWorkerLine where WId=@WId";
                        var wlparam = new { WId = t.WId };
                        workerLines = conn.Query<TWorkerLineEntity>(str, wlparam).ToList();
                        t.LineList = workerLines;
                    });
                }
            }
            return mResult;
        }
        /// <summary>
        /// 根据流水线获取员工
        /// </summary>
        /// <param name="linecode"></param>
        /// <returns></returns>
        public List<TWorkerEntity> GetTTaskWorkerListByLineCode(string linecode)
        {
            var mResult = new List<TWorkerEntity>();
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                if (!string.IsNullOrWhiteSpace(linecode))
                {
                    StringBuilder strSql = new StringBuilder("Select * from tWorker  where 1=1 ");

                    strSql.Append(" and  LineCode=@LineCode ");

                    var param = new
                    {
                        LineCode = linecode
                    };

                    mResult = conn.Query<TWorkerEntity>(strSql.ToString(), param).ToList();
                }
            }
            return mResult;
        }


        /// <summary>
        /// 获取可参与排班的工人
        /// </summary>
        /// <param name="linecode"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public List<TWorkerEntity> GetTTaskWorkerListByLineCode(string linecode, DateTime dateTime)
        {
            var mResult = new List<TWorkerEntity>();
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                if (!string.IsNullOrWhiteSpace(linecode))
                {
                    StringBuilder strSql = new StringBuilder(@"Select w.WId,w.WName,isnull(d.StationIndex,0) OrderIndex from tWorker   w
left join [dbo].[tWorkerScheduleDetail] d on w.WId=d.WID
inner join [dbo].[tWorkerLine] wl on wl.WId=w.WId and   wl.LCode=@LineCode
where 1=1 and w.WId not in(select [WId] from [dbo].[tWorkSchedule] where LineCode=@LineCode and Date>=@StartDate and Date<@Enddate ) ");


                    var param = new
                    {
                        LineCode = linecode,
                        StartDate = dateTime.Date,
                        Enddate = dateTime.Date.AddDays(1)
                    };

                    mResult = conn.Query<TWorkerEntity>(strSql.ToString(), param).ToList();
                }
            }
            return mResult;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool UpdateWorkerStatu(int id, int state)
        {
            if (id > 0)
            {
                using (IDbConnection conn = new SqlConnection(GetConnstr))
                {
                    string strSql = "update  tWorker  set Status=@Status where WId	=@WId	";
                    var param = new { WId = id, Status = state };
                    var result = conn.Execute(strSql, param);
                    if (result > 0)
                        return true;
                }
            }
            return false;
        }
    }
}
