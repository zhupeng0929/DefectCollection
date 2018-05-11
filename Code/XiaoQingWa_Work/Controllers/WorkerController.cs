using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XiaoQingWa_Work_Model.Entity;
using XiaoQingWa_Work_Utility;

namespace XiaoQingWa_Work.Controllers
{
    public class WorkerController : BaseController
    {
        public ActionResult WorkerList()
        {
            return View();
        }
        public ActionResult WorkerListResult(WorkerQuery workerQuery)
        {
            var workerList = tWorkerRepository.GetWorkerInfoListByQueryModel(workerQuery);
            return View(workerList);
        }

        public ActionResult CreateWorker(int? workerid)
        {
            var model = new TWorkerEntity();
            if (workerid.HasValue)
            {
                model = tWorkerRepository.GetSingle(workerid.Value);
            }
            List<SelectListItem> spanSltlist = new List<SelectListItem>();
            spanSltlist.Add(new SelectListItem { Text = "--请选择--", Value = "" });
            var lineList = tLineRepository.GetList().Distinct(l => l.LCode);
            if (lineList != null)
            {
                foreach (var info in lineList)
                {
                    spanSltlist.Add(new SelectListItem { Text = info.LName, Value = info.LCode.ToString() });
                }
            }
            ViewBag.LineList = spanSltlist;
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateWorker(TWorkerEntity workerInfoEntity)
        {
            var result = false;
            ReturnJsonMessage msg = new ReturnJsonMessage();
            if (workerInfoEntity != null)
            {
                var model = tWorkerRepository.GetList().FirstOrDefault(w => w.WNo == workerInfoEntity.WNo && w.WId != workerInfoEntity.WId);
                if (model != null )
                {
                    msg.Text = "员工编号不能重复";
                    msg.Value = "error";
                    return Json(msg);
                }

                if (workerInfoEntity.WId == 0)
                {

                    result = tWorkerRepository.AddReturnInt(workerInfoEntity) > 0;
                }
                else
                {
                    var uerModel = tWorkerRepository.GetSingle(workerInfoEntity.WId);
                    uerModel.WName = workerInfoEntity.WName;
                    uerModel.WNo = workerInfoEntity.WNo;
                    uerModel.WSex = workerInfoEntity.WSex;
                    uerModel.WDescript = workerInfoEntity.WDescript;
                    uerModel.LineCode = workerInfoEntity.LineCode;
                    result = tWorkerRepository.Update(uerModel);
                }
            }


            msg.Text = result ? "保存成功" : "保存失败";
            msg.Value = result ? "success" : "error";

            return Json(msg);
        }

        [HttpPost]
        public ActionResult DeleteWorker(int id)
        {
            var result = false;
            result = tWorkerRepository.DelTWorker(id);

            ReturnJsonMessage msg = new ReturnJsonMessage();

            msg.Text = result ? "删除成功" : "删除失败";
            msg.Value = result ? "success" : "error";

            return Json(msg);
        }

        [HttpPost]
        public ActionResult DeleteWorkerBatch(int[] ids)
        {
            var result = false;
            ReturnJsonMessage msg = new ReturnJsonMessage();
            if (ids.Length > 0)
            {
                result = tWorkerRepository.DelTWorkerBatch(ids);
            }

            msg.Text = result ? "删除成功" : "删除失败";
            msg.Value = result ? "success" : "error";

            return Json(msg);
        }

        [HttpPost]
        public ActionResult UpdateWorkerStatu(int id, int statu)
        {
            var result = false;
            result = tWorkerRepository.UpdateWorkerStatu(id, statu);

            ReturnJsonMessage msg = new ReturnJsonMessage();

            msg.Text = result ? "操作成功" : "操作失败";
            msg.Value = result ? "success" : "error";

            return Json(msg);
        }
    }
}