using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XiaoQingWa_Work_Model.Entity;
using XiaoQingWa_Work_Utility;

namespace XiaoQingWa_Work.Controllers
{
    public class StationController : BaseController
    {
        // GET: Station
        // GET: Station
        public ActionResult StationList()
        {
            return View();
        }
        public ActionResult StationListResult(StationQuery stationQuery)
        {
            var stationList = tStationRepository.GetStationListByQueryModel(stationQuery);
            return View(stationList);
        }

        public ActionResult CreateStation(string stationcode)
        {
            var model = new TStationEntity();
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

            if (!string.IsNullOrWhiteSpace(stationcode))
            {
                model = tStationRepository.GetSingle(stationcode);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateStation(TStationEntity stationInfoEntity)
        {
            var result = false;
            ReturnJsonMessage msg = new ReturnJsonMessage();
            var stationLsit = tStationRepository.GetList();
            if (stationInfoEntity != null)
            {
                #region 验证
                if (stationLsit != null && stationLsit.Count(l => l.StationCode == stationInfoEntity.StationCode && l.StationId != stationInfoEntity.StationId) > 0)
                {
                    msg.Text = "编码已经存在";
                    msg.Value = "error";
                    return Json(msg);
                }
                if (stationLsit != null && stationLsit.Count(l => l.StationName == stationInfoEntity.StationName && l.StationId != stationInfoEntity.StationId) > 0)
                {
                    msg.Text = "名称已经存在";
                    msg.Value = "error";
                    return Json(msg);
                }
                if (stationLsit != null && stationLsit.Count(l => l.StationFullName == stationInfoEntity.StationFullName && l.StationId != stationInfoEntity.StationId) > 0)
                {
                    msg.Text = "全称已经存在";
                    msg.Value = "error";
                    return Json(msg);
                }
                #endregion

                if (stationInfoEntity.StationId == 0)
                {
                    result = tStationRepository.AddReturnStr(stationInfoEntity) != null;
                }
                else
                {
                    var stationModel = stationLsit.FirstOrDefault(l => l.StationId == stationInfoEntity.StationId);
                    stationModel.StationCode = stationInfoEntity.StationCode;
                    stationModel.StationName = stationInfoEntity.StationName;
                    stationModel.StationFullName = stationInfoEntity.StationFullName;

                    stationModel.LineCode = stationInfoEntity.LineCode;
                    stationModel.LineName = stationInfoEntity.LineName;
                    stationModel.StationIndex = stationInfoEntity.StationIndex;
                    stationModel.JianKongPoint = stationInfoEntity.JianKongPoint;
                    stationModel.GoodBtnId = stationInfoEntity.GoodBtnId;
                    stationModel.BadBtnId = stationInfoEntity.BadBtnId;
                    //stationModel.BadBtnId = stationInfoEntity.StationFullName;

                    result = tStationRepository.Update(stationModel);
                }
            }


            msg.Text = result ? "保存成功" : "保存失败";
            msg.Value = result ? "success" : "error";

            return Json(msg);
        }

        [HttpPost]
        public ActionResult DeleteStation(int id)
        {
            var result = false;
            result = tStationRepository.DelTStation(id);

            ReturnJsonMessage msg = new ReturnJsonMessage();

            msg.Text = result ? "删除成功" : "删除失败";
            msg.Value = result ? "success" : "error";

            return Json(msg);
        }

        [HttpPost]
        public ActionResult DeleteStationBatch(int[] ids)
        {
            var result = false;
            ReturnJsonMessage msg = new ReturnJsonMessage();
            if (ids.Length > 0)
            {
                result = tStationRepository.DelTStationBatch(ids);
            }

            msg.Text = result ? "删除成功" : "删除失败";
            msg.Value = result ? "success" : "error";

            return Json(msg);
        }

        [HttpPost]
        public ActionResult UpdateStatu(int id, int statu)
        {
            var result = false;
            result = tStationRepository.UpdateStatu(id, statu);

            ReturnJsonMessage msg = new ReturnJsonMessage();

            msg.Text = result ? "操作成功" : "操作失败";
            msg.Value = result ? "success" : "error";

            return Json(msg);
        }
    }
}