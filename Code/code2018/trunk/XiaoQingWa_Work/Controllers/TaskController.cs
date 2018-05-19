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

        private void Add(TTaskEntity taskEntity, ResponseResult result)
        {
            var model = tTaskRepository.GetSingle(taskEntity.BillNO);//任务单
            if (model != null)
            {
                result.Status = false;
                result.Message = "任务单号已存在";
                return;
            }
            var lineModel = tLineRepository.GetSingle(taskEntity.LineCode);//流水线
            if (lineModel == null)
            {
                result.Status = false;
                result.Message = "该流水线不存在";
                return;
            }
            var worker = tWorkerRepository.GetTTaskWorkerListByLineCode(taskEntity.LineCode);//流水线工人
            if (worker == null || worker.Count == 0)
            {
                result.Status = false;
                result.Message = "该流水线暂未分配工人";
                return;
            }
            var station = tStationRepository.GetStationListByLineCode(taskEntity.LineCode);//工位

            if (worker == null || worker.Count == 0)
            {
                result.Status = false;
                result.Message = "该流水线暂无工位";
                return;
            }



            #region 排班


            var paiban = PaiBan(lineModel.StationCount, worker.OrderBy(d => d.OrderIndex).ToList());//排班结果，排班后人的序号
            var paibancount = paiban[0].Count;
            var ScheduleList = new List<TWorkScheduleEntity>();
            for (int i = 0; i < paibancount && i < station.Count; i++)
            {
                var item = paiban[0].Dequeue();
                TWorkScheduleEntity workScheduleEntity = new TWorkScheduleEntity();
                workScheduleEntity.BillNO = taskEntity.BillNO;
                workScheduleEntity.WId = item.WId;
                workScheduleEntity.WName = item.WName;
                workScheduleEntity.StationCode = station[i].StationCode;
                workScheduleEntity.StationIndex = station[i].StationIndex;
                workScheduleEntity.StationName = station[i].StationName;
                workScheduleEntity.GoodBtnId = station[i].GoodBtnId;
                workScheduleEntity.BadBtnId = station[i].BadBtnId;
                workScheduleEntity.LineCode = taskEntity.LineCode;
                workScheduleEntity.Date = DateTime.Now;
                ScheduleList.Add(workScheduleEntity);
            }
            var scheduleResult = tWorkScheduleRepository.AddTWorkScheduleTran(taskEntity, ScheduleList, paiban[1].ToList());

            if (scheduleResult)
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


        public List<Queue<TWorkerEntity>> PaiBan(int station, List<TWorkerEntity> worker)
        {
            var last = new List<TWorkerEntity>();//上次排班

            var next = new List<TWorkerEntity>();//剩余未排班



            var maxnum = worker.Max(w => w.OrderIndex);
            int s = 0;
            if (maxnum == 0)
            {
                for (int i = 0; i < station && i < worker.Count(); i++)
                {
                    worker[i].OrderIndex = i + 1;
                    last.Add(worker[i]);
                    s++;
                    maxnum = i + 1;
                }
                for (int i = s; i < worker.Count(); i++)
                {
                    worker[i].OrderIndex = i + 1;
                    if (worker[i].Status == 0)
                    {
                        next.Add(worker[i]);

                    }
                    maxnum = i + 1;
                }
            }
            else
            {
                for (int i = 0; i < station && i < worker.Count(); i++)
                {
                    if (worker[i].OrderIndex == 0)
                    {
                        worker[i].OrderIndex = maxnum + 1;
                        maxnum += 1;
                    }

                    s++;
                    last.Add(worker[i]);
                }
                for (int i = s; i < worker.Count(); i++)
                {

                    if (worker[i].OrderIndex == 0)
                    {
                        worker[i].OrderIndex = maxnum + 1;
                        maxnum += 1;
                    }
                    next.Add(worker[i]);

                }

            }


            Framer framer = new Framer(station, last, next);

            var paiban = framer.Assign();


            for (int index = 0; index < paiban[1].Count; index++)
            {
                worker.FirstOrDefault(w => w.WName == paiban[1].ElementAt(index).WName).OrderIndex = index + 1;

            }
            //for (int index = 0; index < paiban[0].Count; index++)
            //{
            //    Console.WriteLine(string.Format("工位: {0}的员工是{1}", index + 1, paiban[1].ElementAt(index).WName));
            //}
            return paiban;

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
    public class Framer
    {
        public Queue<TWorkerEntity> Station { get; set; }
        public Queue<TWorkerEntity> Employees { get; set; }
        public int StationCount = 0;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="station"></param>
        /// <param name="employ"></param>
        public Framer(int stationcount, List<TWorkerEntity> stations, List<TWorkerEntity> employs)
        {
            StationCount = stationcount;
            var station = stations.Count;
            var employ = employs.Count;

            this.Station = new Queue<TWorkerEntity>(station);
            this.Employees = new Queue<TWorkerEntity>(employ);
            stations.ForEach(s => { this.Station.Enqueue(s); });
            employs.ForEach(s => { this.Employees.Enqueue(s); });

        }

        public List<Queue<TWorkerEntity>> Assign()
        {
            var re = this.Station.Dequeue();
            this.Employees.Enqueue(re);


            int e = this.Employees.Count;
            for (int i = 0; i < e; i++)
            {
                var next = this.Employees.Dequeue();

                if (next.Status == 1)//请假跳过
                {
                    this.Employees.Enqueue(next);//放回队列
                    continue;
                }
                else
                {
                    this.Station.Enqueue(next);//放入工位
                    break;
                }
            }
            List<Queue<TWorkerEntity>> workerEntities = new List<Queue<TWorkerEntity>>();

            var result = new Queue<TWorkerEntity>();
            var paidui = new Queue<TWorkerEntity>();

            while (this.Station.Count > 0)
            {
                var s = this.Station.Dequeue();
                if (s.Status == 1)
                {

                    this.Employees.Enqueue(s);
                }
                else
                {
                    result.Enqueue(s);
                    paidui.Enqueue(s);

                }

            }
            for (int j = paidui.Count; j < StationCount; j++)
            {
                e = this.Employees.Count;
                for (int i = 0; i < e; i++)
                {
                    var next = this.Employees.Dequeue();

                    if (next.Status == 1)//请假跳过
                    {
                        this.Employees.Enqueue(next);//放回队列
                        continue;
                    }
                    else
                    {
                        result.Enqueue(next);//放入工位
                        paidui.Enqueue(next);
                        break;
                    }
                }

            }


            while (this.Employees.Count > 0)
            {
                result.Enqueue(this.Employees.Dequeue());
            }

            workerEntities.Add(paidui);
            workerEntities.Add(result);

            return workerEntities;
        }

    }
}
