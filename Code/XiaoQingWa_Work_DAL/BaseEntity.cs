using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using XiaoQingWa_Work_IDAL;

namespace XiaoQingWa_Work_DAL
{
    public class BaseRepository<T> : IDependency<T> where T : class
    {
        /// <summary>
        ///  测试方法，获取数据库连接字符串
        /// </summary>
        /// <returns></returns>
        public static string GetConnstr
        {
            get { return WebConfigurationManager.AppSettings["ConnectionString"].ToString(); }

        }
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="model"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public int AddReturnInt(T model, IDbConnection conn = null, IDbTransaction trans = null)
        {
            if (conn == null)
            {
                using (conn = new SqlConnection(GetConnstr))
                {
                    return conn.Insert<int>(model, trans);
                }
            }
            else
            {
                return conn.Insert<int>(model, trans);
            }
        }
        public string AddReturnStr(T model, IDbConnection conn = null, IDbTransaction trans = null)
        {
            if (conn == null)
            {
                using (conn = new SqlConnection(GetConnstr))
                {
                    return conn.Insert<string>(model, trans);
                }
            }
            else
            {
                return conn.Insert<string>(model, trans);
            }
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="model"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool Del(T model, IDbConnection conn = null, IDbTransaction trans = null)
        {
            if (conn == null)
            {
                using (conn = new SqlConnection(GetConnstr))
                {
                    return conn.Delete(model, trans) > 0;
                }
            }
            else
            {
                return conn.Delete(model, trans) > 0;
            }

        }
        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns></returns>
        public T GetSingle(object id)
        {
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                return conn.Get<T>(id);
            }
        }
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        public List<T> GetList()
        {
            using (IDbConnection conn = new SqlConnection(GetConnstr))
            {
                return conn.GetList<T>().ToList();
            }
        }
        
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool Update(T entity, IDbConnection conn=null, IDbTransaction trans = null)
        {

            if (conn == null)
            {
                using (conn = new SqlConnection(GetConnstr))
                {
                    return conn.Update(entity, trans) > 0;
                }
            }
            else
            {
                return conn.Update(entity, trans) > 0;
            }
        }


    }

}
