using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaoQingWa_Work_IDAL;
using Dapper;
using XiaoQingWa_Work_Model.Entity;

namespace XiaoQingWa_Work_DAL
{
    public class ArrangerOrder : IArrangerOrder
    {
        protected readonly ITTaskRepository tTaskRepository = new TTaskRepository();
        protected readonly ITWorkScheduleRepository tWorkScheduleRepository = new TWorkScheduleRepository();
        protected readonly ITLineRepository tLineRepository = new TLineRepository();
        protected readonly ITStationRepository tStationRepository = new TStationRepository();
        protected readonly ITWorkerRepository tWorkerRepository = new TWorkerRepository();

        public void GetTaskToArrangerOrder()
        {
            var list = tTaskRepository.GetTOrderTaskList();//获取未排班任务
            if (list != null && list.Count > 0)
            {
                list.ForEach(l => { ToArrangerOrder(l.BillNO); });
            }

        }

        private bool ToArrangerOrder(string BillNO)
        {
            var model = tTaskRepository.GetSingle(BillNO);//任务单

            //判断流水线是否已经排班
            var schedul = tWorkScheduleRepository.GetTWorkScheduleList(model.LineCode, model.StartDateTime);
            if (schedul != null && schedul.Count > 0)//表示该流水线已经排过班
            {
                model.IsOrder = true;
                tTaskRepository.UpdateTTask(model);
                return false;
            }

            var lineModel = tLineRepository.GetTLine(model.LineCode);//流水线
            if (lineModel==null )
            {
                return false;
            }
            var worker = tWorkerRepository.GetTTaskWorkerListByLineCode(model.LineCode, model.StartDateTime);//可以参与流水线工人
            if (worker == null)
            {
                return false;
            }
            var station = tStationRepository.GetStationListByLineCode(model.LineCode);//工位
            if (station == null)
            {
                return false;
            }
            var paiban = PaiBan(lineModel.StationCount, worker.OrderBy(d => d.OrderIndex).ToList());//排班结果，排班后人的序号
            var paibancount = paiban[0].Count;
            var ScheduleList = new List<TWorkScheduleEntity>();
            for (int i = 0; i < paibancount && i < station.Count; i++)
            {
                var item = paiban[0].Dequeue();
                TWorkScheduleEntity workScheduleEntity = new TWorkScheduleEntity();
                workScheduleEntity.BillNO = BillNO;
                workScheduleEntity.WId = item.WId;
                workScheduleEntity.WName = item.WName;
                workScheduleEntity.StationCode = station[i].StationCode;
                workScheduleEntity.StationIndex = station[i].StationIndex;
                workScheduleEntity.StationName = station[i].StationName;
                workScheduleEntity.GoodBtnId = station[i].GoodBtnId;
                workScheduleEntity.BadBtnId = station[i].BadBtnId;
                workScheduleEntity.LineCode = model.LineCode;
                workScheduleEntity.Date = model.StartDateTime;
                ScheduleList.Add(workScheduleEntity);
            }
            var scheduleResult = tWorkScheduleRepository.AddTWorkScheduleTran(ScheduleList, paiban[1].ToList());
            if (scheduleResult)
            {
                tTaskRepository.UpdateTTask(model);
            }
            return scheduleResult;
        }

        private List<Queue<TWorkerEntity>> PaiBan(int station, List<TWorkerEntity> worker)
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

            return paiban;

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
