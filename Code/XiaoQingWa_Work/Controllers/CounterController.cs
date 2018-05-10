using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XiaoQingWa_Work_Model.Entity;
using XiaoQingWa_Work_Utility;

namespace XiaoQingWa_Work.Controllers
{
    public class CounterController : BaseController
    {
        // GET: Counter
        // GET: Counter
        public ActionResult CounterList()
        {
            return View();
        }
        public ActionResult CounterListResult(string CountNo)
        {
            var counterList = tCounterRepository.GetCounterListByQueryModel(CountNo);
            return View(counterList);
        }

        public ActionResult CreateCounter(int? cid)
        {
            var model = new TCounterEntity();

            if (cid > 0)
            {
                model = tCounterRepository.GetSingle(cid);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateCounter(TCounterEntity counterInfoEntity)
        {
            var result = false;
            ReturnJsonMessage msg = new ReturnJsonMessage();
            var counterLsit = tCounterRepository.GetList();
            if (counterInfoEntity != null)
            {
                #region 验证
                if (counterLsit != null && counterLsit.Count(l => l.CountNo == counterInfoEntity.CountNo && l.CId != counterInfoEntity.CId) > 0)
                {
                    msg.Text = "编码已经存在";
                    msg.Value = "error";
                    return Json(msg);
                }

                #endregion

                if (counterInfoEntity.CId == 0)
                {
                    result = tCounterRepository.AddReturnInt(counterInfoEntity) > 0;
                }
                else
                {
                    var counterModel = counterLsit.FirstOrDefault(l => l.CId == counterInfoEntity.CId);
                    counterModel.CountNo = counterInfoEntity.CountNo;
                    counterModel.Status = counterInfoEntity.Status;

                    result = tCounterRepository.Update(counterModel);
                }
            }

            msg.Text = result ? "保存成功" : "保存失败";
            msg.Value = result ? "success" : "error";

            return Json(msg);
        }

        [HttpPost]
        public ActionResult DeleteCounter(int id)
        {
            var result = false;
            result = tCounterRepository.DelTCounter(id);

            ReturnJsonMessage msg = new ReturnJsonMessage();

            msg.Text = result ? "删除成功" : "删除失败";
            msg.Value = result ? "success" : "error";

            return Json(msg);
        }

        [HttpPost]
        public ActionResult DeleteCounterBatch(int[] ids)
        {
            var result = false;
            ReturnJsonMessage msg = new ReturnJsonMessage();
            if (ids.Length > 0)
            {
                result = tCounterRepository.DelTCounterBatch(ids);
            }

            msg.Text = result ? "删除成功" : "删除失败";
            msg.Value = result ? "success" : "error";

            return Json(msg);
        }

        [HttpPost]
        public ActionResult UpdateStatu(int id, int statu)
        {
            var result = false;
            result = tCounterRepository.UpdateStatu(id, statu);

            ReturnJsonMessage msg = new ReturnJsonMessage();

            msg.Text = result ? "操作成功" : "操作失败";
            msg.Value = result ? "success" : "error";

            return Json(msg);
        }
    }
}