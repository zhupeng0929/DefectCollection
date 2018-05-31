using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XiaoQingWa_Work_DAL;
using XiaoQingWa_Work_IDAL;
using XiaoQingWa_Work_Model.Entity;
using XiaoQingWa_Work_Utility;

namespace XiaoQingWa_Work.Controllers
{
    [Auth]
    public class BaseController : Controller
    {

        /// <summary>
        /// 登陆用户
        /// </summary>
        public UserInfoEntity LoginUser
        {
            get
            {
                try
                {
                    if (Session[CommonHelper.SessionUserKey] != null)
                    {
                        return Session[CommonHelper.SessionUserKey] as UserInfoEntity;
                    }
                }
                catch (Exception)
                {
                    //Session为空时，继续走，取Cookie
                }
                return GetUser();
            }
        }
        protected UserInfoEntity GetUser()
        {
            string key = CommonHelper.Md5(CommonHelper.COOKIE_KEY_USERINFO);
            string data = CookieHelper.GetCookieValue(key);
            if (!string.IsNullOrEmpty(data))
            {
                try
                {
                    data = CommonHelper.DesDecrypt(data, CommonHelper.COOKIE_KEY_ENCRYPT);
                    var userInfo = JsonHelper.Deserialize<UserInfoEntity>(data);
                    Session[CommonHelper.SessionUserKey] = userInfo;
                    return userInfo;
                }
                catch (System.Exception ex)
                {
                    return null;
                }
            }
            return null;
        }
        protected readonly IUserInfoRepository userInfoRepository = new UserInfoRepository();

        protected readonly ITLineRepository  tLineRepository = new TLineRepository();
        protected readonly ITStationRepository tStationRepository = new TStationRepository();
        protected readonly ITCounterRepository tCounterRepository = new TCounterRepository();
        protected readonly ITTaskWorkerRepository tTaskWorkerRepository = new TTaskWorkerRepository();
        protected readonly ITWorkerRepository tWorkerRepository = new TWorkerRepository();
        protected readonly ITWorkerLineRepository tWorkerLineRepository = new TWorkerLineRepository();
        protected readonly ITRecordDetailRepository tRecordDetailRepository = new TRecordDetailRepository();

        protected readonly ITTaskRepository  tTaskRepository = new TTaskRepository();

        protected readonly IStsticRepository ststicRepository = new StsticRepository();
        protected readonly ITWorkScheduleRepository tWorkScheduleRepository = new TWorkScheduleRepository();
        //protected readonly IPictureInfoRepository pictureInfoRepository = new PictureInfoRepository();
        //protected readonly IPictureInfoRepository pictureInfoRepository = new PictureInfoRepository();








    }
}