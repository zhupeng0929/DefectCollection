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
        protected readonly ITLineRepository tLineRepository = new TLineRepository();
        protected readonly ITStationRepository tStationRepository = new TStationRepository();
        protected readonly ITCounterRepository tCounterRepository = new TCounterRepository();
        protected readonly ITTaskWorkerRepository tTaskWorkerRepository = new TTaskWorkerRepository();
        protected readonly ITWorkerRepository tWorkerRepository = new TWorkerRepository();
        protected readonly ITRecordDetailRepository tRecordDetailRepository = new TRecordDetailRepository();
        protected readonly ITWorkScheduleRepository tWorkScheduleRepository = new TWorkScheduleRepository();

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
                result.Message = "任务单开始时间不能为空";// + taskEntity.StartDateTime + taskEntity.BillNO+ taskEntity.TName;
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
            if (taskEntity.OperateType < 0 || taskEntity.OperateType > 2)
            {
                result.Status = false;
                result.Message = "类型错误";
                return result;
            }
            #endregion
            var model = tTaskRepository.GetTTaskList().FirstOrDefault(t =>  t.BillNO == taskEntity.BillNO);
            if (model != null)
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
                if (tTaskRepository.UpdateTTask(taskEntity))
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

        private void Add(TTaskEntity taskEntity, ResponseResult result)
        {
            var model = tTaskRepository.GetSingle(taskEntity.BillNO);//任务单
            if (model != null)
            {
                result.Status = false;
                result.Message = "当前任务单编码已存在";
                return;
            }
            //var lineModel = tLineRepository.GetTLine(taskEntity.LineCode);//流水线
            //if (lineModel == null)
            //{
            //    result.Status = false;
            //    result.Message = "该流水线不存在";
            //    return;
            //}
            //var worker = tWorkerRepository.GetTTaskWorkerListByLineCode(taskEntity.LineCode);//流水线工人
            //if (worker == null || worker.Count == 0)
            //{
            //    result.Status = false;
            //    result.Message = "该流水线暂未分配工人";
            //    return;
            //}
            //var station = tStationRepository.GetStationListByLineCode(taskEntity.LineCode);//工位

            //if (worker == null || worker.Count == 0)
            //{
            //    result.Status = false;
            //    result.Message = "该流水线暂无工位";
            //    return;
            //}



            #region 排班


            //var paiban = PaiBan(lineModel.StationCount, worker.OrderBy(d => d.OrderIndex).ToList());//排班结果，排班后人的序号
            //var paibancount = paiban[0].Count;
            //var ScheduleList = new List<TWorkScheduleEntity>();
            //for (int i = 0; i < paibancount && i < station.Count; i++)
            //{
            //    var item = paiban[0].Dequeue();
            //    TWorkScheduleEntity workScheduleEntity = new TWorkScheduleEntity();
            //    workScheduleEntity.BillNO = taskEntity.BillNO;
            //    workScheduleEntity.WId = item.WId;
            //    workScheduleEntity.WName = item.WName;
            //    workScheduleEntity.StationCode = station[i].StationCode;
            //    workScheduleEntity.StationIndex = station[i].StationIndex;
            //    workScheduleEntity.StationName = station[i].StationName;
            //    workScheduleEntity.GoodBtnId = station[i].GoodBtnId;
            //    workScheduleEntity.BadBtnId = station[i].BadBtnId;
            //    workScheduleEntity.LineCode = taskEntity.LineCode;
            //    workScheduleEntity.Date = DateTime.Now;
            //    ScheduleList.Add(workScheduleEntity);
            //}
            //var scheduleResult = tWorkScheduleRepository.AddTWorkScheduleTran(taskEntity, ScheduleList, paiban[1].ToList());

            var scheduleResult = tTaskRepository.AddReturnStr(taskEntity);
            if (!string.IsNullOrWhiteSpace(scheduleResult))
            {
                result.Status = true;
                result.Message = "新增任务成功";
                return;
            }
            else
            {
                result.Status = false;
                result.Message = "新增任务失败";
                return;
            }
            #endregion

        }


       
    }

}
