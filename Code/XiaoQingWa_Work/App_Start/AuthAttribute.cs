using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XiaoQingWa_Work_Model.Entity;
using XiaoQingWa_Work_Utility;

namespace XiaoQingWa_Work
{
    public class AuthAttribute: ActionFilterAttribute
    {
        /// <summary> 
        /// 验证权限
        /// </summary> 
        /// <param name="filterContext"></param> 
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            UserInfoEntity userLogin = GetUserInfo(filterContext);
           
            if (userLogin == null || userLogin.UserId == 0)
            {
                string returnUrl = filterContext.HttpContext.Request.Url.PathAndQuery;

                filterContext.Result = LoginResult();
                return;
            }

            //TODO:暂时去除页面权限验证
            //if (string.IsNullOrEmpty(Code)) return;

            //LogHelper.WriteCommonLog("SSOLogin", "GetUserInfo", JsonHelper.SerializerObject(userLogin), EnumState.LogLevel.Info);

            //List<MenuInfo> menuList = UserInfoService.GetUserMenu(userLogin.UserId_Common.ToString());
            //if (menuList == null || !menuList.ExistOneSubMenuModel(Code))
            //    filterContext.Result = new ContentResult() { Content = "无访问此页面权限！" };

        }
        private UserInfoEntity GetUserInfo(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session[CommonHelper.SessionUserKey] != null)
                return filterContext.HttpContext.Session[CommonHelper.SessionUserKey] as UserInfoEntity;

            return GetUser(filterContext);
        }
        private UserInfoEntity GetUser(ActionExecutingContext filterContext)
        {
            string key = CommonHelper.Md5(CommonHelper.COOKIE_KEY_USERINFO);
            string data = CookieHelper.GetCookieValue(key);
            if (!string.IsNullOrEmpty(data))
            {
                try
                {
                    data = CommonHelper.DesDecrypt(data, CommonHelper.COOKIE_KEY_ENCRYPT);
                    var userInfo = JsonHelper.Deserialize<UserInfoEntity>(data);
                    filterContext.HttpContext.Session[CommonHelper.SessionUserKey] = userInfo;
                    return userInfo;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return null;
        }

        public virtual ActionResult LoginResult()
        {
            return new RedirectResult("/Account/Login");
        }
    }
}