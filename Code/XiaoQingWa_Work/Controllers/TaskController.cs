using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XiaoQingWa_Work_Model.Entity;

namespace XiaoQingWa_Work.Controllers
{
    public class ResponseResult
    {
        public bool Status { get; set; } = true;
        public string Message { get; set; } = "操作成功";
    }
    public class ModifyReq
    {
        public string BillNO { get; set; }
        public int Type { get; set; }
    }
    public class TaskController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult AddTask(TTaskEntity taskEntity)
        {
            var result = new ResponseResult();

            #region 参数验证
            if (string.IsNullOrWhiteSpace(taskEntity.BillNO))
            {
                result.Status = false;
                result.Message = "任务单号不能为空";
                return result;
            }
            if (string.IsNullOrWhiteSpace(taskEntity.TName))
            {
                result.Status = false;
                result.Message = "任务单名称不能为空";
                return result;
            }
            if (taskEntity.StartDateTime == null|| taskEntity.StartDateTime == DateTime.Parse("1900-1-1"))
            {
                result.Status = false;
                result.Message = "任务单开始时间不能为空";
                return result;
            }
            if (taskEntity.EndDateTime == null||taskEntity.EndDateTime == DateTime.Parse("1900-1-1"))
            {
                result.Status = false;
                result.Message = "任务单结束时间不能为空";
                return result;
            }
            if (string.IsNullOrWhiteSpace(taskEntity.LineCode))
            {
                result.Status = false;
                result.Message = "流水线号为空";
                return result;
            }
            if (string.IsNullOrWhiteSpace(taskEntity.ItemCode))
            {
                result.Status = false;
                result.Message = "物料代码不能为空";
                return result;
            }
            if (string.IsNullOrWhiteSpace(taskEntity.ItemName))
            {
                result.Status = false;
                result.Message = "物料名称不能为空";
                return result;
            }
            if (taskEntity.Qty <= 0)
            {
                result.Status = false;
                result.Message = "物料数量不能为0";
                return result;
            }
            if (taskEntity.OperateType <= 0|| taskEntity.OperateType > 2)
            {
                result.Status = false;
                result.Message = "类型错误";
                return result;
            }
            #endregion

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult ModifyStatus(ModifyReq taskEntity)
        {
            var result = new ResponseResult();

            #region 参数验证
            if (string.IsNullOrWhiteSpace(taskEntity.BillNO))
            {
                result.Status = false;
                result.Message = "任务单号不能为空";
                return result;
            }
            if (taskEntity.Type > 1 || taskEntity.Type < 0)
            {
                result.Status = false;
                result.Message = "类型错误";
                return result;
            }

            #endregion

            return result;
        }
    }
}
