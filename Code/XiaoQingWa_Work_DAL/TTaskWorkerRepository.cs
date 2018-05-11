

/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////数据操作类//////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////

//---------------------------------------------------------------------------------
// <copyright company="XiaoQingWa" file="tTaskWorkerRespository.cs" >
//      Author: Peng.Zhu
//      Create Time: 2018/5/5 21:10:03
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
    /// TTaskWorkerDAL数据访问类
    /// </summary>
    public partial class TTaskWorkerRepository : BaseRepository<TTaskWorkerEntity>, ITTaskWorkerRepository
    {
        /// <summary>
        /// 新增实体
        /// </summary>
        public bool AddTTaskWorker(TTaskWorkerEntity model)
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
        public bool AddTTaskWorker(TTaskWorkerEntity model, IDbConnection conn, IDbTransaction trans)
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
        public bool DelTTaskWorker(int id)
        {
            if (id > 0)
            {
                using (IDbConnection conn = new SqlConnection(GetConnstr))
                {
                    string strSql = "delete from TTaskWorker where ID=@ID";
                    var param = new { ID = id };
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
        public bool DelTTaskWorkerBatch(int[] ids)
        {
            if (ids.Length > 0)
            {
                using (IDbConnection conn = new SqlConnection(GetConnstr))
                {
                    string strSql = "delete from  TTaskWorker where ID in @ID ";

                    DynamicParameters param = new DynamicParameters();
                    param.Add("ID", ids);
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
        public TTaskWorkerEntity GetTTaskWorker(int id)
        {
            var mResult = new TTaskWorkerEntity();
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                mResult = conn.Get<TTaskWorkerEntity>(id);
            }
            return mResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TTaskWorkerEntity> GetTTaskWorkerList()
        {
            var mResult = new List<TTaskWorkerEntity>();
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                mResult = conn.GetList<TTaskWorkerEntity>().ToList();
            }
            return mResult;
        }
        /// <summary>
        /// 更新实体列表
        /// </summary>
        /// <returns></returns>
        public bool UpdateTTaskWorker(TTaskWorkerEntity entity)
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
        public bool UpdateTTaskWorker(TTaskWorkerEntity entity, IDbConnection conn, IDbTransaction trans)
        {
            int row;
            row = conn.Update(entity, trans);
            return row > 0;
        }



    }
}
