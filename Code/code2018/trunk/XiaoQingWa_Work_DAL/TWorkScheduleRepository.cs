
/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////数据操作类//////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////

//---------------------------------------------------------------------------------
// <copyright company="XiaoQingWa" file="tWorkScheduleRespository.cs" >
//      Author: Peng.Zhu
//      Create Time: 2018/5/5 21:17:27
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
using XiaoQingWa_Work_Model.Model;

namespace XiaoQingWa_Work_DAL
{
    /// <summary>
    /// TWorkScheduleDAL数据访问类
    /// </summary>
    public partial class TWorkScheduleRepository : BaseRepository<TWorkScheduleEntity>, ITWorkScheduleRepository
    {
        /// <summary>
        /// 新增实体
        /// </summary>
        public bool AddTWorkSchedule(TWorkScheduleEntity model)
        {
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                var result = conn.Insert<int>(model);
                if (result > 0)
                    return true;
            }
            return false;
        }

        public bool AddTWorkScheduleTran(List<TWorkScheduleEntity> list, List<TWorkerEntity> workerEntities)
        {
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {

                        foreach (var item in list)
                        {
                            AddTWorkSchedule(item, conn, trans);
                        }
                        foreach (var worker in workerEntities)
                        {
                            var workerScheduleDetail = new WorkerScheduleDetail();
                            workerScheduleDetail.LineCode = list.FirstOrDefault().LineCode;
                            workerScheduleDetail.WId = worker.WId;
                            workerScheduleDetail.StationIndex = worker.OrderIndex;

                            conn.Insert<int>(worker, trans);
                        }
                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return false;

                    }
                }
            }

        }


        /// <summary>
        /// 新增实体--事物
        /// </summary>
        public bool AddTWorkSchedule(TWorkScheduleEntity model, IDbConnection conn, IDbTransaction trans)
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
        public bool DelTWorkSchedule(int id)
        {
            if (id > 0)
            {
                using (IDbConnection conn = new SqlConnection(GetConnstr))
                {
                    string strSql = "delete from TWorkSchedule where WId=@WId";
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
        public bool DelTWorkScheduleBatch(int[] ids)
        {
            if (ids.Length > 0)
            {
                using (IDbConnection conn = new SqlConnection(GetConnstr))
                {
                    string strSql = "delete from  TWorkSchedule where WId in @WId ";

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
        public TWorkScheduleEntity GetTWorkSchedule(int id)
        {
            var mResult = new TWorkScheduleEntity();
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                mResult = conn.Get<TWorkScheduleEntity>(id);
            }
            return mResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TWorkScheduleEntity> GetTWorkScheduleList(string billno)
        {
            var mResult = new List<TWorkScheduleEntity>();
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                var strsql = "select * from tWorkSchedule where BillNO=@BillNO ";
                var param = new
                {
                    BillNO = billno
                };
                mResult = conn.Query<TWorkScheduleEntity>(strsql, param).ToList();
            }
            return mResult;
        }

        public List<PaiBanViewModel> GetTWorkScheduleList()
        {
            var mResult = new List<PaiBanViewModel>();
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                var strsql = "select * from tWorkSchedule w inner join  tLine l on w.LineCode=l.LCode where Date>=@StartDate and Date< @EndDate";
                var param = new
                {
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.Date.AddDays(1)
                };
                mResult = conn.Query<PaiBanViewModel>(strsql, param).ToList();
            }
            return mResult;
        }

        public List<TWorkScheduleEntity> GetTWorkScheduleList(string linecode, DateTime dateTime)
        {
            var mResult = new List<TWorkScheduleEntity>();
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                var strsql = "select * from tWorkSchedule w where LineCode=@LineCode and  Date>=@StartDate and Date< @EndDate";
                var param = new
                {
                    LineCode = linecode,
                    StartDate = dateTime.Date,
                    EndDate = dateTime.Date.AddDays(1)
                };
                mResult = conn.Query<TWorkScheduleEntity>(strsql, param).ToList();
            }
            return mResult;
        }

        /// <summary>
        /// 更新实体列表
        /// </summary>
        /// <returns></returns>
        public bool UpdateTWorkSchedule(TWorkScheduleEntity entity)
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
        public bool UpdateTWorkSchedule(TWorkScheduleEntity entity, IDbConnection conn, IDbTransaction trans)
        {
            int row;
            row = conn.Update(entity, trans);
            return row > 0;
        }
    }
}
