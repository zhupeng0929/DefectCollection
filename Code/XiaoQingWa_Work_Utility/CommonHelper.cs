using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace XiaoQingWa_Work_Utility
{
    /// <summary>
    /// CommonHelper
    /// </summary>
    public class CommonHelper
    {
        /// <summary>
        /// Session Key
        /// </summary>
        public const string SessionUserKey = "SessionUserKey";
        public const string CACHE_KEY_USERMENU = "XiaoQingWaUserMenu_Cache";
        public const string CACHE_KEY_USERINFO = "XiaoQingWaUserInfo_Cache";
        public const string COOKIE_KEY_USERINFO = "XiaoQingWaUserInfo_Cookie";
        public const string COOKIE_KEY_ENCRYPT = "d7&Ff%12";
        public const string TOKEN_KEY_NotLogin = "RL%hkBd^";

        /// <summary>
        /// 获取目录地址
        /// </summary>
        /// <returns>目录地址</returns>
        public static string RootCatalog
        {
            get { return "/" + GetAppSetting("RootCatalog"); }
        }

        /// <summary>
        /// 获取指定AppSettingsString
        /// </summary>
        /// <param name="key">健</param>
        /// <returns>返回值</returns>
        public static string GetAppSetting(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            if (!string.IsNullOrEmpty(value))
            {
                return value.Trim();
            }

            return string.Empty;
        }

        /// <summary>
        /// MD5函数
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string Md5(string str)
        {
            byte[] b = Encoding.Default.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            var ret = string.Empty;
            for (var i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }

        /// <summary>
        /// 获取文件MD5值
        /// </summary>
        /// <param name="fileName">文件绝对路径</param>
        /// <returns>MD5值</returns>
        public static string GetMD5HashFromFile(Stream InputStream)
        {
            try
            {
                //FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(InputStream);
                //file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        /// <summary>
        /// 获取本地IP
        /// </summary>
        /// <returns></returns>
        public static string GetIp()
        {
            string ip = "192.168.0.1";
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            if (ipHost != null)
            {
                IPAddress ipAddr = ipHost.AddressList[0];
                ip = ipAddr.ToString();
            }

            return ip;
        }

        /// <summary>
        /// 转换为int类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue">返回的默认值</param>
        /// <returns></returns>
        public static int ConvertToInt(object obj, int defaultValue)
        {
            int result = defaultValue;
            if (obj != null && obj != DBNull.Value)
            {
                if (!int.TryParse(obj.ToString().Trim(), NumberStyles.Number, null, out result))
                {
                    result = defaultValue;
                }
            }
            return result;
        }

        /// <summary>
        /// int类型转化成datetime类型
        /// </summary>
        /// <param name="date">date值为19000101到99999999范围之间</param>
        /// <returns>事例：2014-01-01</returns>
        public static DateTime ConvertToDate(int date)
        {
            if (date < 19000101 || date > 99999999)
            {
                return DateTime.Parse("1900-01-01");
            }
            var year = date / 10000;
            var month = date % 10000 / 100;
            var day = date % 100;
            return DateTime.Parse(year + "-" + month + "-" + day);
        }

        /// <summary>
        /// 转换为decimal类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue">返回的默认值</param>
        /// <returns></returns>
        public static decimal ConvertToDecimal(object obj, decimal defaultValue)
        {
            decimal result = defaultValue;
            if (obj != null && obj != DBNull.Value)
            {
                if (!decimal.TryParse(obj.ToString().Trim(), out result))
                {
                    result = defaultValue;
                }
            }
            return result;
        }

        /// <summary>
        /// 转换为DateTime
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue">返回的默认值</param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(object obj, DateTime defaultValue)
        {
            DateTime result = defaultValue;
            if (obj != null)
            {
                if (!DateTime.TryParse(obj.ToString().Trim(), out result))
                {
                    result = defaultValue;
                }
            }
            return result;
        }
        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="codeCount">生成字符串长度</param>
        /// <returns></returns>
        public static string CreateRandomCode(int codeCount)
        {
            // 函数功能:产生数字和字符混合的随机字符串
            //abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ
            string allChar = "0123456789";
            char[] allCharArray = allChar.ToCharArray();
            string randomCode = "";
            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                int r = rand.Next(allChar.Length - 1);
                randomCode += allCharArray.GetValue(r);
            }
            return randomCode;
        }

        /// <summary>
        /// 得到照片上传文件目录
        /// </summary>
        /// <param name="applicationPath">应用程序路径</param>
        /// <param name="server">web请求服务</param>
        /// <param name="uploadPath">要上传的目录 </param>
        /// <returns>返回值 照片上传文件目录</returns>
        public static string CheckPicUpLoadDirectory(string applicationPath, System.Web.HttpServerUtilityBase server, string uploadPath)
        {
            string dic = server.MapPath(applicationPath + uploadPath);
            return CreateFilePath(dic);
        }

        /// <summary>
        /// 返回图片的存放路径,存放数据库
        /// </summary>
        /// <returns></returns>
        public static string GetPicUpLoad()
        {
            string dic = "/UploadFile/UploadPic/Original/";
            dic += DateTime.Today.Year + "/";
            dic += DateTime.Today.Month + "/";
            dic += DateTime.Today.Day + "/";

            return dic;
        }

        /// <summary>
        /// 创建文件目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string CreateFilePath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        /// <summary>
        /// 是否是手机号
        /// </summary>
        /// <param name="strMobile">待测试手机号字符串</param>
        /// <returns>是手机格式就返回true;否则返回false</returns>
        public static bool IsMobile(string strMobile)
        {
            if (strMobile == null)
                return false;
            return Regex.IsMatch(strMobile.Trim(), @"^((\(\d{2,3}\))|(\d{3}\-))?(13\d{9})|(15[0,1,2,3,5,6,7,8,9]\d{8})|(18[0,1,2,3,5,6,7,8,9]\d{8})|(147\d{8})$");
        }

        /// <summary>
        /// 是否是电话号码
        /// </summary>
        /// <param name="strPhone"></param>
        /// <returns>是电话格式就返回true;否则返回false</returns>
        public static bool IsPhone(string strPhone)
        {
            if (strPhone == null)
                return false;
            return Regex.IsMatch(strPhone.Trim(), @"^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$");
        }

        /// <summary>
        /// 是否是传真号
        /// </summary>
        /// <param name="fax">传真号码</param>
        /// <returns></returns>
        public static bool IsFax(string fax)
        {
            if (string.IsNullOrEmpty(fax))
                return false;
            return Regex.IsMatch(fax.Trim(), @"^(\d{3,4}-)?\d{7,8}$");
        }

        /// <summary>
        /// 根据EntityList呈现下拉框
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bindingVal">绑定的val字段</param>
        /// <param name="bindingText">绑定的text字段</param>
        /// <param name="tList"></param>
        /// <param name="defalutVal">默认值</param>
        /// <param name="addChoose"> </param>
        /// <returns></returns>
        public static List<SelectListItem> SelectListEntity<T>(string bindingVal, string bindingText, List<T> tList, object defalutVal = null,
                                                               bool addChoose = true) where T : new()
        {
            var selectListItem = new List<SelectListItem>();
            if (addChoose)
            {
                var listItem = new SelectListItem { Text = "--请选择--", Value = "-1" };
                selectListItem.Add(listItem);
            }
            if (tList == null || tList.Count == 0)
            {
                return selectListItem;
            }

            Type t = typeof(T);
            if (t.GetProperty(bindingVal) != null || t.GetProperty(bindingText) != null)
            {
                selectListItem.AddRange(tList.Select(item => new SelectListItem
                {
                    Text = item.GetType().GetProperty(bindingText).GetValue(item, null).ToString(),
                    Value = item.GetType().GetProperty(bindingVal).GetValue(item, null).ToString(),
                    Selected = (defalutVal == null ? false : defalutVal.ToString().Equals(item.GetType().GetProperty(bindingVal).GetValue(item, null).ToString()))
                }));
            }

            return selectListItem;
        }

        /// <summary>
        /// 对地址有首字母的字符串进行截取
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public string SubAddress(object o)
        {
            string address = string.Empty;
            address = o.ToString();
            string str = string.Empty;
            if (!string.IsNullOrEmpty(address))
            {
                str = address.Substring(0, 1);
                if (Regex.IsMatch(str, @"[a-zA-Z]"))
                {
                    address = address.Substring(1);
                }
            }
            return address;
        }

        /// <summary>
        /// 获取最终字符串（排除DBNull，null，string.Empty 或空值后的真实值） 
        /// </summary>
        /// <param name="objString"></param>
        /// <returns></returns>
        public static string FinalString(object objString)
        {
            if (!IsDBNullOrNullOrEmptyString(objString))
                return objString.ToString();
            else
                return string.Empty;
        }

        /// <summary>
        /// DataRow的value或从数据库中取出的Object型数据验证,验证取出的object是否是DBNull,空或null]
        /// 如果是DBNull,null或空字符串则返回true
        /// </summary>
        /// <param name="objSource">待验证的object</param>
        /// <returns>
        /// 如果是DBNull,null或空字符串则返回true
        /// </returns>
        public static bool IsDBNullOrNullOrEmptyString(object objSource)
        {
            if ((objSource == DBNull.Value) || (objSource == null))
                return true;
            string strSource = objSource.ToString();
            if (strSource.Trim() == string.Empty)
                return true;
            return false;
        }

        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string DesDecrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string DesEncrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            des.Mode = CipherMode.CBC;

            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:x2}", b);
            }
            return ret.ToString();
        }
        /// <summary>
        /// 获取时间差
        /// </summary>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public static TimeSpan ExecDateDiff(DateTime dateBegin, DateTime dateEnd)
        {
            TimeSpan ts1 = new TimeSpan(dateBegin.Ticks);
            TimeSpan ts2 = new TimeSpan(dateEnd.Ticks);
            TimeSpan ts3 = ts1.Subtract(ts2).Duration();
            //你想转的格式
            return ts3;
            //string.Format("{0}天{1}小时{2}分钟{3}秒", ts3.Days, ts3.Hours, ts3.Minutes, ts3.Seconds);
        }

        //周岁计算
        public static int ExecZhouAge(DateTime birthday, DateTime compareDate)
        {
            int age = -1;
            if (birthday > new DateTime(1900, 1, 1) && compareDate > new DateTime(1900, 1, 1))
            {
                DateTime dtbirth = new DateTime(1900, 1, 1).AddMonths(birthday.Month).AddDays(birthday.Day);
                DateTime dtcompareDate = new DateTime(1900, 1, 1).AddMonths(compareDate.Month).AddDays(compareDate.Day);
                age = compareDate.Year - birthday.Year;
                if (dtcompareDate <= dtbirth)
                {
                    age = age - 1;
                }
            }
            return age;
        }


        #region 分页转化
        public static string SqlMutiplyPage(int pagesize, int pageIndex, string sql, string sqlsort)
        {
            StringBuilder strInfo = new StringBuilder();
            strInfo.AppendFormat(@" select top {0} * from (
            select ROW_NUMBER() over({1}) rownum,* from (  ", pagesize, sqlsort);
            strInfo.Append(sql);
            strInfo.AppendFormat(" ) t ) m where m.rownum> {0}*{1} order by m.rownum  ", pagesize, pageIndex - 1);
            return strInfo.ToString();
        }
        #endregion

    }
}
