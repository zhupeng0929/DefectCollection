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
                    result = tStationRepository.AddTStation(stationInfoEntity);
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

                    result = tStationRepository.UpdateTStation(stationModel);
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
           

            ReturnJsonMessage msg = new ReturnJsonMessage();
            var model = tStationRepository.GetTStation(id);

            if (model != null && (!string.IsNullOrWhiteSpace(model.GoodBtnId) || !string.IsNullOrWhiteSpace(model.BadBtnId)))
            {
                msg.Text = "请先解绑设备后再进行删除";
                msg.Value = msg.Value = "error";
                return Json(msg);
            }
            else
            {
                result = tStationRepository.DelTStation(id);
            }
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
                foreach (var id in ids)
                {
                    var model = tStationRepository.GetTStation(id);
                    if (model != null && (!string.IsNullOrWhiteSpace(model.GoodBtnId) || !string.IsNullOrWhiteSpace(model.BadBtnId)))
                    {
                        msg.Text = "请先解绑设备后再进行删除";
                        msg.Value = msg.Value = "error";
                        return Json(msg);
                    }
                }
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

        public ActionResult BondCounter(string stationcode, int type = 0)
        {
            List<SelectListItem> spanSltlist = new List<SelectListItem>();
            spanSltlist.Add(new SelectListItem { Text = "--请选择--", Value = "" });
            var counterList = tCounterRepository.GetList().Where(c => c.Status == 0);
            if (counterList != null)
            {
                foreach (var info in counterList)
                {
                    spanSltlist.Add(new SelectListItem { Text = info.CountNo, Value = info.CountNo.ToString() });
                }
            }
            ViewBag.CounterList = spanSltlist;
            ViewBag.Type = type;
            var model = new TStationEntity();
            if (!string.IsNullOrWhiteSpace(stationcode))
            {
                model = tStationRepository.GetSingle(stationcode);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult BondCounter(int StationId, string GoodBtnId, string BadBtnId, int type)
        {
            ReturnJsonMessage msg = new ReturnJsonMessage();
            var result = false;
            if (string.IsNullOrWhiteSpace(GoodBtnId) && string.IsNullOrWhiteSpace(BadBtnId))
            {
                msg.Text = "请选择设备";
                msg.Value = "error";
                return Json(msg);
            }
            result = tStationRepository.BondCounter(StationId, GoodBtnId, BadBtnId, type);
            msg.Text = result ? "绑定成功" : "绑定失败";
            msg.Value = result ? "success" : "error";
            return Json(msg);
        }
        [HttpPost]
        public ActionResult UnBindStation(int id, string code, int type)
        {
            ReturnJsonMessage msg = new ReturnJsonMessage();
            var result = false;

            result = tStationRepository.UnBindStation(id, code, type);
            msg.Text = result ? "解绑成功" : "解绑失败";
            msg.Value = result ? "success" : "error";
            return Json(msg);

        }

    }
}