using System;
using System.Collections.Generic;
using System.Linq;
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

            var workerList = CommonHelper.SelectListEntity("WId", "WName", tWorkerRepository.GetList(), null, false);
            workerList.Insert(0, new SelectListItem { Text = "--请选择--", Value = "-1" });
            ViewBag.Worker = workerList;

            return View();
        }

        public ActionResult StaticResult(int StaticType, TRecordDetailEntity recordDetailEntity)
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
    }
}