using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using XiaoQingWa_Work_Model.Entity;
using XiaoQingWa_Work_Utility;

namespace XiaoQingWa_Work.Controllers
{
    public class StaticController : BaseController
    {
        // GET: Static
        public ActionResult Index()
        {
            var taskList = CommonHelper.SelectListEntity("BillNO", "TName", tTaskRepository.GetList(), null, false);
            taskList.Insert(0, new SelectListItem { Text = "--请选择--", Value = "" });
            ViewBag.Task = taskList;

            var lineList = CommonHelper.SelectListEntity("LCode", "LName", tLineRepository.GetList(), null, false);
            lineList.Insert(0, new SelectListItem { Text = "--请选择--", Value = "" });
            ViewBag.Line = lineList;

            var stationList = CommonHelper.SelectListEntity("StationCode", "StationName", tStationRepository.GetList(), null, false);
            stationList.Insert(0, new SelectListItem { Text = "--请选择--", Value = "" });
            ViewBag.Station = stationList;

            var workerList = new List<SelectListItem>();

            tWorkerRepository.GetList().ForEach(w =>
            {

                workerList.Add(new SelectListItem()
                {
                    Text = w.WNo + w.WName,
                    Value = w.WId.ToString(),
                });

            });
            workerList.Insert(0, new SelectListItem { Text = "--请选择--", Value = "-1" });
            ViewBag.Worker = workerList;

            return View();
        }

        public ActionResult StaticResult(int StaticType, StsticQuery recordDetailEntity)
        {
            if (StaticType == 0)
            {
                var result = ststicRepository.StaticByTask(recordDetailEntity);
                return View("TaskResult", result);
            }
            if (StaticType == 1)
            {
                var result = ststicRepository.StaticByWorker(recordDetailEntity);
                return View("WorkerResult", result);
            }
            if (StaticType == 2)
            {
                var result = ststicRepository.StaticByLine(recordDetailEntity);
                return View("LineResult", result);
            }
            return View();
        }


        public void Export(int StaticType, StsticQuery recordDetailEntity)
        {
            if (StaticType == 0)
            {
                var result = ststicRepository.StaticByTask(recordDetailEntity);
                ExportTask(result);
            }
            if (StaticType == 1)
            {
                var result = ststicRepository.StaticByWorker(recordDetailEntity);
                ExportWorker(result);
            }
            if (StaticType == 2)
            {
                var result = ststicRepository.StaticByLine(recordDetailEntity);
                ExportLine(result);
            }

        }

        public void ExportTask(List<TTaskEntity> taskEntities)
        {


            DataTable dtNew = new DataTable();
            dtNew.Columns.Add(new DataColumn("任务单编码"));
            dtNew.Columns.Add(new DataColumn("任务单名称"));
            dtNew.Columns.Add(new DataColumn("流水线编码"));
            dtNew.Columns.Add(new DataColumn("任务时间"));
            dtNew.Columns.Add(new DataColumn("物料名称"));
            dtNew.Columns.Add(new DataColumn("物料代码"));
            dtNew.Columns.Add(new DataColumn("物料数量"));
            dtNew.Columns.Add(new DataColumn("好件统计"));
            dtNew.Columns.Add(new DataColumn("坏件统计"));
            dtNew.Columns.Add(new DataColumn("汇总差额"));


            foreach (var rows in taskEntities)
            {

                DataRow dr = dtNew.NewRow();
                dr["任务单编码"] = rows.BillNO.ToString();
                dr["任务单名称"] = rows.TName.ToString();
                dr["流水线编码"] = rows.LineCode.ToString();
                dr["任务时间"] = rows.StartDateTime.ToShortDateString();
                dr["物料名称"] = rows.ItemName.ToString();
                dr["物料代码"] = rows.ItemCode.ToString();
                dr["物料数量"] = rows.Qty.ToString();
                dr["好件统计"] = rows.GoodCount.ToString();
                dr["坏件统计"] = rows.BadCount.ToString();
                dr["汇总差额"] = rows.DiffCount.ToString();
                dtNew.Rows.Add(dr);
            }
            DataRow drtotle = dtNew.NewRow();
            drtotle["任务单编码"] = "汇总";
            drtotle["任务单名称"] = "--";
            drtotle["流水线编码"] = "--";
            drtotle["任务时间"] = "--";
            drtotle["物料名称"] = "--";
            drtotle["物料代码"] = "--";
            drtotle["物料数量"] = taskEntities.Sum(rows => rows.Qty).ToString();
            drtotle["好件统计"] = taskEntities.Sum(rows => rows.GoodCount).ToString();
            drtotle["坏件统计"] = taskEntities.Sum(rows => rows.BadCount).ToString();
            drtotle["汇总差额"] = taskEntities.Sum(rows => rows.DiffCount).ToString();
            dtNew.Rows.Add(drtotle);
            ExportExcel(dtNew, "任务单统计");
        }


        public void ExportWorker(List<WorkerStatic> workerStatics)
        {


            DataTable dtNew = new DataTable();

            dtNew.Columns.Add(new DataColumn("员工编码"));
            dtNew.Columns.Add(new DataColumn("员工名称"));
            dtNew.Columns.Add(new DataColumn("任务编码"));
            dtNew.Columns.Add(new DataColumn("任务名称"));
            dtNew.Columns.Add(new DataColumn("任务日期"));
            dtNew.Columns.Add(new DataColumn("任务数量"));
            dtNew.Columns.Add(new DataColumn("好件统计"));
            dtNew.Columns.Add(new DataColumn("坏件统计"));
            dtNew.Columns.Add(new DataColumn("合计汇总"));
            foreach (var rows in workerStatics)
            {

                DataRow dr = dtNew.NewRow();
                dr["员工编码"] = rows.WNo.ToString();
                dr["员工名称"] = rows.WName.ToString();
                dr["任务编码"] = rows.TaskCode.ToString();
                dr["任务名称"] = rows.TaskName.ToString();
                dr["任务日期"] = rows.StartDateTime.ToShortDateString();
                dr["任务数量"] = rows.TaskCount.ToString();
                dr["好件统计"] = rows.GoodCount.ToString();
                dr["坏件统计"] = rows.BadCount.ToString();
                dr["合计汇总"] = (rows.GoodCount + rows.GoodCount).ToString();

                dtNew.Rows.Add(dr);
            }
            DataRow drtotle = dtNew.NewRow();
            drtotle["员工编码"] = "汇总";
            drtotle["员工名称"] = "--";
            drtotle["任务编码"] = "--";
            drtotle["任务名称"] = "--";
            drtotle["任务日期"] = "--";
            drtotle["任务数量"] = workerStatics.Sum(rows => rows.TaskCount).ToString();
            drtotle["好件统计"] = workerStatics.Sum(rows => rows.GoodCount).ToString();
            drtotle["坏件统计"] = workerStatics.Sum(rows => rows.BadCount).ToString();
            drtotle["合计汇总"] = workerStatics.Sum(rows => rows.GoodCount + rows.GoodCount).ToString();

            dtNew.Rows.Add(drtotle);
            ExportExcel(dtNew, "员工统计");

        }

        public void ExportLine(List<LineStatic> lineStatics)
        {
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add(new DataColumn("流水线编码"));
            dtNew.Columns.Add(new DataColumn("流水线名称"));
            dtNew.Columns.Add(new DataColumn("总量"));
            dtNew.Columns.Add(new DataColumn("好件统计"));
            dtNew.Columns.Add(new DataColumn("坏件统计"));
            dtNew.Columns.Add(new DataColumn("合计"));
            dtNew.Columns.Add(new DataColumn("汇总差额"));
            foreach (var rows in lineStatics)
            {
                DataRow dr = dtNew.NewRow();
                dr["流水线编码"] = rows.LCode.ToString();
                dr["流水线名称"] = rows.LName.ToString();
                dr["总量"] = rows.Qty.ToString();
                dr["好件统计"] = rows.GoodCount.ToString();
                dr["坏件统计"] = rows.BadCount.ToString();
                dr["合计"] = (rows.GoodCount + rows.BadCount).ToString();
                dr["汇总差额"] = (rows.Qty - rows.GoodCount - rows.BadCount).ToString();
                dtNew.Rows.Add(dr);
            }

            DataRow drtotle = dtNew.NewRow();
            drtotle["流水线编码"] = "汇总";
            drtotle["流水线名称"] = "--";
            drtotle["总量"] = lineStatics.Sum(rows => rows.Qty).ToString();
            drtotle["好件统计"] = lineStatics.Sum(rows => rows.GoodCount).ToString();
            drtotle["坏件统计"] = lineStatics.Sum(rows => rows.BadCount).ToString();
            drtotle["合计"] = lineStatics.Sum(rows => rows.GoodCount + rows.BadCount).ToString();
            drtotle["汇总差额"] = lineStatics.Sum(rows => rows.Qty - rows.GoodCount - rows.BadCount).ToString();
            dtNew.Rows.Add(drtotle);

            ExportExcel(dtNew, "流水线统计");

        }



        public void ExportExcel(DataTable dt, string name)
        {
            //创建一个内存流
            MemoryStream ms = new MemoryStream();

            //以指定的字符编码向指定的流写入字符
            StreamWriter sw = new StreamWriter(ms, Encoding.GetEncoding("GB2312"));

            StringBuilder strbu = new StringBuilder();

            strbu.Append("\t" + name);
            strbu.Append(Environment.NewLine);

            //写入标题
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                strbu.Append(dt.Columns[i].ColumnName.ToString() + "\t");
            }
            //加入换行字符串
            strbu.Append(Environment.NewLine);

            //写入内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    strbu.Append(dt.Rows[i][j].ToString() + "\t");
                }
                strbu.Append(Environment.NewLine);
            }

            sw.Write(strbu.ToString());
            sw.Flush();

            sw.Close();
            sw.Dispose();

            //转换为字节数组
            byte[] bytes = ms.ToArray();

            ms.Close();
            ms.Dispose();

            OutputClient(bytes, name);
        }
        public void OutputClient(byte[] bytes, string name)
        {
            var response = HttpContext.Response;

            response.Buffer = true;

            response.Clear();
            response.ClearHeaders();
            response.ClearContent();

            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", name));

            response.Charset = "GB2312";
            response.ContentEncoding = Encoding.GetEncoding("GB2312");

            response.BinaryWrite(bytes);
            response.Flush();

            response.Close();
        }

    }
}