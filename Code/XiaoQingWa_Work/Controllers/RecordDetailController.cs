using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XiaoQingWa_Work_Model.Entity;

namespace XiaoQingWa_Work.Controllers
{
    public class RecordDetailController : BaseController
    {
        public ActionResult RecordDetailList()
        {
            return View();
        }
        public ActionResult RecordDetailResult(TRecordDetailEntity recordDetailEntity)
        {
            var workerList = tRecordDetailRepository.GetRecordDetailListByQueryModel(recordDetailEntity);
            return View(workerList);
        }
    }
}