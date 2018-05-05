using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XiaoQingWa_Work_DAL;
using XiaoQingWa_Work_IDAL;

namespace XiaoQingWa_Work.Controllers
{
    [Auth]
    public class BaseController : Controller
    {
        protected readonly IUserInfoRepository userInfoRepository = new UserInfoRepository();
        protected readonly IPictureInfoRepository pictureInfoRepository = new PictureInfoRepository();
    }
}