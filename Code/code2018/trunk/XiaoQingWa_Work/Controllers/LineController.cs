using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XiaoQingWa_Work_Model.Entity;
using XiaoQingWa_Work_Utility;

namespace XiaoQingWa_Work.Controllers
{
    public class LineController : BaseController
    {
        // GET: Line
        public ActionResult LineList()
        {
            return View();
        }
        public ActionResult LineListResult(LineQuery lineQuery)
        {
            var lineList = tLineRepository.GetLineInfoListByQueryModel(lineQuery);
            return View(lineList);
        }

        public ActionResult CreateLine(string linecode)
        {
            var model = new TLineEntity();
            if (!string.IsNullOrWhiteSpace(linecode))
            {
                model = tLineRepository.GetSingle(linecode);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateLine(TLineEntity lineInfoEntity)
        {
            var result = false;
            ReturnJsonMessage msg = new ReturnJsonMessage();
            var lineLsit = tLineRepository.GetList();
            if (lineInfoEntity != null)
            {
                #region 验证
                if (lineLsit != null && lineLsit.Count(l => l.LCode == lineInfoEntity.LCode && l.LId != lineInfoEntity.LId) > 0)
                {
                    msg.Text = "编码已经存在";
                    msg.Value = "error";
                    return Json(msg);
                }
                if (lineLsit != null && lineLsit.Count(l => l.LName == lineInfoEntity.LName && l.LId != lineInfoEntity.LId) > 0)
                {
                    msg.Text = "名称已经存在";
                    msg.Value = "error";
                    return Json(msg);
                }
                if (lineLsit != null && lineLsit.Count(l => l.LFullName == lineInfoEntity.LFullName && l.LId != lineInfoEntity.LId) > 0)
                {
                    msg.Text = "全称已经存在";
                    msg.Value = "error";
                    return Json(msg);
                }
                #endregion

                if (lineInfoEntity.LId == 0)
                {
                    result = tLineRepository.AddReturnInt(lineInfoEntity) > 0;
                }
                else
                {
                    var lineModel = lineLsit.FirstOrDefault(l => l.LCode == lineInfoEntity.LCode);
                    lineModel.LName = lineInfoEntity.LName;
                    lineModel.LFullName = lineInfoEntity.LFullName;
                    lineModel.LType = lineInfoEntity.LType;

                    result = tLineRepository.Update(lineModel);
                }
            }


            msg.Text = result ? "保存成功" : "保存失败";
            msg.Value = result ? "success" : "error";

            return Json(msg);
        }

        [HttpPost]
        public ActionResult DeleteLine(int id)
        {
            var result = false;
            result = tLineRepository.DelTLine(id);

            ReturnJsonMessage msg = new ReturnJsonMessage();

            msg.Text = result ? "删除成功" : "删除失败";
            msg.Value = result ? "success" : "error";

            return Json(msg);
        }

        [HttpPost]
        public ActionResult DeleteLineBatch(int[] ids)
        {
            var result = false;
            ReturnJsonMessage msg = new ReturnJsonMessage();
            if (ids.Length > 0)
            {
                result = tLineRepository.DelTLineBatch(ids);
            }

            msg.Text = result ? "删除成功" : "删除失败";
            msg.Value = result ? "success" : "error";

            return Json(msg);
        }

    }
}