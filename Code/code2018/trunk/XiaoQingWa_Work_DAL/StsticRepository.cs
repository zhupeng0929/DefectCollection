using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using XiaoQingWa_Work_IDAL;
using XiaoQingWa_Work_Model.Entity;
using Dapper;
namespace XiaoQingWa_Work_DAL
{
    public class StsticRepository : IStsticRepository
    {
        /// <summary>
        ///  测试方法，获取数据库连接字符串
        /// </summary>
        /// <returns></returns>
        public static string GetConnstr
        {
            get { return WebConfigurationManager.AppSettings["ConnectionString"].ToString(); }

        }

        public List<LineStatic> StaticByLine(TRecordDetailEntity taskEntity)
        {

            var mResult = new List<LineStatic>();
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                StringBuilder strSql = new StringBuilder(@"SELECT l.LCode,l.LName,t.Qty,
SUM(case CountType when '好件' then r.Count else 0 end) GoodCount,
SUM(case CountType when '坏件' then r.Count else 0 end) BadCount
  FROM[alhbFaultCountDB].[dbo].[tRecordDetail] r
  inner join[alhbFaultCountDB].[dbo].[tLine]  l on l.LCode = r.LineCode
  inner join[alhbFaultCountDB].[dbo].[tTask] t on t.LineCode = r.LineCode
  where r.Status = 0
");
                if (!string.IsNullOrWhiteSpace(taskEntity.BillNO))
                {
                    strSql.Append(" and  r.BillNO>=@BillNO ");
                }
                if (!string.IsNullOrWhiteSpace(taskEntity.LineCode))
                {
                    strSql.Append(" and  r.LineCode>=@LineCode ");
                }
                if (!string.IsNullOrWhiteSpace(taskEntity.StationCode))
                {
                    strSql.Append(" and  r.StationCode>=@StationCode ");
                }
                if (taskEntity.WId > 0)
                {
                    strSql.Append(" and  r.WId>=@WId ");
                }
                strSql.Append("   group by  l.LCode, l.LName, t.Qty ");
                var param = new
                {
                    taskEntity.BillNO,
                    taskEntity.LineCode,
                    taskEntity.StationCode,
                    taskEntity.WId,
                };

                mResult = conn.Query<LineStatic>(strSql.ToString(), param).ToList();
            }
            return mResult;
        }

        public List<TTaskEntity> StaticByTask(TRecordDetailEntity taskEntity)
        {

            var mResult = new List<TTaskEntity>();
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                StringBuilder strSql = new StringBuilder(@"SELECT t.BillNO,t.TName,t.LineCode,t.ItemName,t.ItemCode,t.Qty,
SUM(case CountType when '好件' then r.Count else 0 end) GoodCount,
SUM(case CountType when '坏件' then r.Count else 0 end) BadCount
  FROM[alhbFaultCountDB].[dbo].[tRecordDetail] r
  inner join[alhbFaultCountDB].[dbo].[tLine]  l on l.LCode = r.LineCode
  inner join[alhbFaultCountDB].[dbo].[tTask] t on t.LineCode = r.LineCode
  where r.Status = 0
");
                if (!string.IsNullOrWhiteSpace(taskEntity.BillNO))
                {
                    strSql.Append(" and  r.BillNO>=@BillNO ");
                }
                if (!string.IsNullOrWhiteSpace(taskEntity.LineCode))
                {
                    strSql.Append(" and  r.LineCode>=@LineCode ");
                }
                if (!string.IsNullOrWhiteSpace(taskEntity.StationCode))
                {
                    strSql.Append(" and  r.StationCode>=@StationCode ");
                }
                if (taskEntity.WId > 0)
                {
                    strSql.Append(" and  r.WId>=@WId ");
                }
                strSql.Append("   group by	 t.BillNO,t.TName,t.LineCode,t.ItemName,t.ItemCode,t.Qty ");
                var param = new
                {
                    taskEntity.BillNO,
                    taskEntity.LineCode,
                    taskEntity.StationCode,
                    taskEntity.WId,
                };

                mResult = conn.Query<TTaskEntity>(strSql.ToString(), param).ToList();
            }
            return mResult;
        }

        public List<WorkerStatic> StaticByWorker(TRecordDetailEntity taskEntity)
        {
            var mResult = new List<WorkerStatic>();
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                StringBuilder strSql = new StringBuilder(@"SELECT w.WNo,w.WName,r.BillNO TaskCode,COUNT(distinct r.BillNO)TaskCount,
SUM(case CountType when '好件' then r.Count else 0 end) GoodCount,
SUM(case CountType when '坏件' then r.Count else 0 end) BadCount
  FROM[alhbFaultCountDB].[dbo].[tRecordDetail] r
  inner join[alhbFaultCountDB].[dbo].[tLine]  l on l.LCode = r.LineCode
  inner join[alhbFaultCountDB].[dbo].[tTask] t on t.LineCode = r.LineCode
  inner join [alhbFaultCountDB].[dbo].[tWorker] w on w.WId=r.WId
  where r.Status = 0
");
                if (!string.IsNullOrWhiteSpace(taskEntity.BillNO))
                {
                    strSql.Append(" and  r.BillNO>=@BillNO ");
                }
                if (!string.IsNullOrWhiteSpace(taskEntity.LineCode))
                {
                    strSql.Append(" and  r.LineCode>=@LineCode ");
                }
                if (!string.IsNullOrWhiteSpace(taskEntity.StationCode))
                {
                    strSql.Append(" and  r.StationCode>=@StationCode ");
                }
                if (taskEntity.WId > 0)
                {
                    strSql.Append(" and  r.WId>=@WId ");
                }
                strSql.Append("     group by	w.WNo,w.WName,r.BillNO");
                var param = new
                {
                    taskEntity.BillNO,
                    taskEntity.LineCode,
                    taskEntity.StationCode,
                    taskEntity.WId,
                };

                mResult = conn.Query<WorkerStatic>(strSql.ToString(), param).ToList();
            }
            return mResult;
        }
    }
}
