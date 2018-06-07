using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XiaoQingWa_Work_Model.Model;
using XiaoQingWa_Work_Utility;

namespace XiaoQingWa_Work.Controllers
{
    public class PaiBanController : BaseController
    {
        // GET: PaiBan
        public ActionResult PaiBanShow()
        {
            var paiBanList = new List<PaiBanViewModel>();



            var list = tWorkScheduleRepository.GetTWorkScheduleList().OrderBy(s => s.LineCode).OrderBy(s => s.StationIndex).ToList();

            var linelist = tLineRepository.GetList().OrderBy(s => s.LCode).Select(s => s.LName).Distinct().ToList();
            var workerList = CommonHelper.SelectListEntity("WId", "WName", tWorkerRepository.GetTWorkerList(), null, false);
            workerList.Insert(0, new SelectListItem { Text = "--请选择--", Value = "-1" });
            ViewBag.Worker = workerList;
            ViewBag.linelist = linelist;
            return View(list);
        }
        public ActionResult ChangeWorker(int id, int wid, string wname)
        {
            var result = false;
            ReturnJsonMessage msg = new ReturnJsonMessage();
            if (wid == 0 || string.IsNullOrWhiteSpace(wname))
            {
                msg.Text = "参数错误";
                msg.Value = "error";
                return Json(msg);
            }

            var schedule = tWorkScheduleRepository.GetSingle(id);
            schedule.WId = wid;
            schedule.WName = wname;
            result = tWorkScheduleRepository.UpdateTWorkSchedule(schedule);
            msg.Text = result ? "保存成功" : "保存失败";
            msg.Value = result ? "success" : "error";

            return Json(msg);
        }
    }
}