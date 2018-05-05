
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XiaoQingWa_Work.Controllers
{
    public class PictureController : BaseController
    {
        // GET: Picture
        public ActionResult PictureList()
        {
            return View();
        }
        public ActionResult PictureList_R()
        {
            var pictureList = pictureInfoRepository.GetList();
            return View(pictureList);
        }

        public ActionResult AddPicture()
        {
            return View();
        }
        public ActionResult PictureShow()
        {
            var pictureList = pictureInfoRepository.GetList();
            return View(pictureList);
        }

    }
}