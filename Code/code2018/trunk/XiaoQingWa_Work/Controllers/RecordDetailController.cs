using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XiaoQingWa_Work_Model.Entity;
using XiaoQingWa_Work_Utility;

namespace XiaoQingWa_Work.Controllers
{
    public class RecordDetailController : BaseController
    {
        public ActionResult RecordDetailList()
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
        public ActionResult RecordDetailResult(TRecordDetailEntity recordDetailEntity)
        {
            var workerList = tRecordDetailRepository.GetRecordDetailListByQueryModel(recordDetailEntity);
            return View(workerList);
        }
        [HttpPost]
        public ActionResult UpdateRecordStatu(int id, int statu)
        {
            var result = false;
            result = tRecordDetailRepository.UpdateRecordStatu(id, statu);

            ReturnJsonMessage msg = new ReturnJsonMessage();

            msg.Text = result ? "操作成功" : "操作失败";
            msg.Value = result ? "success" : "error";

            return Json(msg);
        }

        public ActionResult UpdateTask(string ids)
        {
            var taskList = CommonHelper.SelectListEntity("BillNO", "TName", tTaskRepository.GetList(), null, false);
            taskList.Insert(0, new SelectListItem { Text = "--请选择--", Value = "" });
            ViewBag.Task = taskList;

            ViewBag.ids = ids;
            return View();
        }
        [HttpPost]
        public ActionResult UpdateTask(string ids, string BillNO)
        {

            ReturnJsonMessage msg = new ReturnJsonMessage();
            string[] sid;
            if (string.IsNullOrWhiteSpace(ids))
            {
                msg.Text = "请选择数据";
                msg.Value = "error";
                return Json(msg);
            }
            else
            {
                sid = ids.Split(',');
            }
            if (string.IsNullOrWhiteSpace(BillNO))
            {
                msg.Text = "请选择任务单";
                msg.Value = "error";
                return Json(msg);
            }
            var result = tRecordDetailRepository.UpdateTask(sid, BillNO);


            msg.Text = result ? "保存成功" : "保存失败";
            msg.Value = result ? "success" : "error";

            return Json(msg);
        }
    }
}