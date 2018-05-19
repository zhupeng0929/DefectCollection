using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XiaoQingWa_Work_DAL;
using XiaoQingWa_Work_IDAL;
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

        protected readonly ITTaskRepository tTaskRepository = new TTaskRepository();
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
            if (taskEntity.StartDateTime == null || taskEntity.StartDateTime == DateTime.Parse("1900-1-1"))
            {
                result.Status = false;
                result.Message = "任务单开始时间不能为空";
                return result;
            }
            if (taskEntity.EndDateTime == null || taskEntity.EndDateTime == DateTime.Parse("1900-1-1"))
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
            if (taskEntity.OperateType <= 0 || taskEntity.OperateType > 2)
            {
                result.Status = false;
                result.Message = "类型错误";
                return result;
            }
            #endregion

            if (tTaskRepository.GetTTaskList().Where(t => t.TName == taskEntity.TName && t.BillNO != taskEntity.BillNO) != null)
            {
                result.Status = false;
                result.Message = "任务单已存在！";
                return result;
            }
            var actionresult = false;
            try
            {
                switch (taskEntity.OperateType)
                {
                    case 0://新增
                        {
                            Add(taskEntity, result);
                            break;
                        }
                    case 1://修改
                        {
                            Update(taskEntity, result);
                            break;
                        }
                    case 2://删除
                        {
                            Delete(taskEntity, result);
                            break;
                        }
                    default:
                        { break; }
                }

            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "接口内部错误！";
                return result;
            }



            return result;
        }
        private void Delete(TTaskEntity taskEntity, ResponseResult result)
        {
            var model = tTaskRepository.GetSingle(taskEntity.BillNO);
            if (model == null)
            {
                result.Status = false;
                result.Message = "任务单不存在或已删除";
            }
            else
            {
                if (tTaskRepository.Del(model))
                {
                    result.Status = true;
                    result.Message = "删除成功";
                }
                else
                {
                    result.Status = false;
                    result.Message = "删除失败";
                }

            }

        }

        private void Update(TTaskEntity taskEntity, ResponseResult result)
        {
            var model = tTaskRepository.GetSingle(taskEntity.BillNO);
            if (model == null)
            {
                result.Status = false;
                result.Message = "任务单不存在或已删除";
            }
            else
            {
                if (tTaskRepository.UpdateTTask(model))
                {
                    result.Status = true;
                    result.Message = "更新成功";
                }
                else
                {
                    result.Status = false;
                    result.Message = "更新失败";
                }

            }

        }

        public void Add(TTaskEntity taskEntity, ResponseResult result)
        {
            var model = tTaskRepository.GetSingle(taskEntity.BillNO);
            if (model != null)
            {
                result.Status = false;
                result.Message = "任务单号已存在";
            }



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
            var mdoel = tTaskRepository.GetSingle(taskEntity.BillNO);
            if (mdoel == null)
            {
                result.Status = false;
                result.Message = "任务单不存在";
                return result;
            }
            mdoel.Status = taskEntity.Type;
            if (tTaskRepository.UpdateTTask(mdoel))
            {
                result.Status = true;
                result.Message = "操作成功";
                return result;
            }
            else
            {
                result.Status = false;
                result.Message = "操作失败";
                return result;
            }


        }
    }
}
;