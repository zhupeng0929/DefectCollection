using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XiaoQingWa_Work_IDAL;
using XiaoQingWa_Work_DAL;
using XiaoQingWa_Work_Model.Entity;
using XiaoQingWa_Work_Utility;
using System.Configuration;

namespace XiaoQingWa_Work.Controllers
{
    public class AccountController : Controller
    {
        protected readonly IUserInfoRepository userInfoRepository = new UserInfoRepository();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            UserInfoEntity loginUserInfo = new UserInfoEntity { UserName = username, PassWord = password };

            if (!ModelState.IsValid)
            {
                return View(loginUserInfo);
            }

            var userInfo = UserLogin(loginUserInfo.UserName, loginUserInfo.PassWord);
            if (userInfo != null)
            {
                //写入cookie
                string key = CommonHelper.Md5(CommonHelper.COOKIE_KEY_USERINFO);
                string data = JsonHelper.Serializer<UserInfoEntity>(userInfo);

                CookieHelper.SetCookie(key, CommonHelper.DesEncrypt(data, CommonHelper.COOKIE_KEY_ENCRYPT));
                Session[CommonHelper.SessionUserKey] = userInfo;

                return Request["ReturnUrl"] == null ? Redirect("~") : Redirect(Request["ReturnUrl"]);
            }

            return View(userInfo);
        }
        private UserInfoEntity UserLogin(string userName, string password)
        {
            var user = userInfoRepository.GetUserInfo(userName);
            if (user != null && user.PassWord == CommonHelper.Md5(password))
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            Session[CommonHelper.SessionUserKey] = null;
            CookieHelper.ClearCookie(CommonHelper.Md5(CommonHelper.COOKIE_KEY_USERINFO));
            return RedirectToAction("Login");
        }
    }
}