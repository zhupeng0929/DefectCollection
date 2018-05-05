using System.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web;
using System.Configuration;

namespace XiaoQingWa_Work_Utility
{
    public class UploadHelper
    {
        public enum UploadFileType
        {
            /// <summary>
            /// 文件
            /// </summary>
            File = 0,
            /// <summary>
            /// 产品
            /// </summary>
            Product = 1,

            /// <summary>
            /// 新闻
            /// </summary>
            News = 2,

        }
        /// <summary>
        /// 得到上传文件目录
        /// </summary>
        /// <param name="applicationPath">应用程序路径</param>
        /// <param name="server">web请求服务</param>
        /// <param name="isOpen">是否需要对外开放，默认true</param>
        /// <returns>上传文件目录</returns>
        public static string CheckFileUpLoadDirectory(string applicationPath, System.Web.HttpServerUtilityBase server, UploadFileType type = UploadFileType.File)
        {
            string dic = server.MapPath(applicationPath + "/UploadFile/" + GetFileUpLoadPath(type));
            if (!Directory.Exists(dic))
            {
                Directory.CreateDirectory(dic);
            }
            return dic;
        }

        public static string CheckFileUpLoadDirectory(string applicationPath, System.Web.HttpServerUtility server, UploadFileType type = UploadFileType.File)
        {
            string dic = server.MapPath(applicationPath + "/UploadFile/" + GetFileUpLoadPath(type));
            if (!Directory.Exists(dic))
            {
                Directory.CreateDirectory(dic);
            }
            return dic;
        }

        /// <summary>
        /// 存放路径,存放数据库
        /// </summary>
        /// <returns></returns>
        public static string GetFileUpLoadPath(UploadFileType type = UploadFileType.File)
        {
            string dic = type.ToString() + "/";
            dic += DateTime.Today.Year + "/";
            dic += DateTime.Today.Month + "/";
            dic += DateTime.Today.Day + "/";

            return dic;
        }
        /// <summary>
        /// <summary>
        /// 检查目录是否存在
        /// </summary>
        /// <param name="ftpPath">要检查的目录的上一级目录</param>
        /// <param name="dirName">要检查的目录名</param>
        /// <returns>存在返回true，否则false</returns>
        public static bool CheckDirectoryExist(string ftpPath, string dirName, string ftpUserID, string ftpPassword)
        {
            bool result = false;
            //errorMsg = string.Empty;
            try
            {
                //实例化FTP连接
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath));
                //ftp登录
                request.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                //指定FTP操作类型为创建目录
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                //获取FTP服务器的响应
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);

                StringBuilder str = new StringBuilder();
                string line = sr.ReadLine();
                while (line != null)
                {
                    str.Append(line);
                    str.Append("|");
                    line = sr.ReadLine();
                }
                string[] datas = str.ToString().Split('|');

                for (int i = 0; i < datas.Length; i++)
                {
                    if (datas[i].Contains("<DIR>"))
                    {
                        int index = datas[i].IndexOf("<DIR>");
                        string name = datas[i].Substring(index + 5).Trim();
                        if (name == dirName)
                        {
                            result = true;
                            break;
                        }
                    }
                }

                sr.Close();
                sr.Dispose();
                response.Close();
            }
            catch (Exception ex)
            {
                //errorMsg = ex.Message;
            }
            return result;
        }


        public static void MakeDir(string ftpPath, string dirName, string password, string useName)
        {
            //errorMsg = string.Empty;;
            //实例化FTP连接
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath + dirName));
            //ftp登录
            request.Credentials = new NetworkCredential(useName, password);

            //指定FTP操作类型为创建目录
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                //获取FTP服务器的响应
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
            }
            catch (Exception)
            {
                request.Abort();
            }
            request.Abort();
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public static string SaveFileMethod(HttpPostedFileBase file)
        {
           
            string newFileName = Guid.NewGuid().ToString().Replace("-", "") + System.IO.Path.GetExtension(file.FileName);
            string returnPath = System.Web.HttpContext.Current.Server.MapPath("~/uploadfile");
            string relativePath = "/doc/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/";
            if (!Directory.Exists(returnPath + relativePath))
            {
                Directory.CreateDirectory(returnPath + relativePath);
            }
            if (File.Exists(returnPath + relativePath + newFileName))
            {
                File.Delete(returnPath + relativePath + newFileName);
            }
            try
            {
                //string fileServiceAddress = ConfigurationManager.AppSettings["fileServiceAddress"];
                file.SaveAs((returnPath + relativePath + newFileName));
                return "/uploadfile" + relativePath + newFileName;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }






        /// <summary>
        /// 上传文件-用于护照扫描仪器
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public static string SaveFileMethodByBytes(string fileName, byte[] bytes)
        {
            string newFileName = Guid.NewGuid().ToString().Replace("-", "") + System.IO.Path.GetExtension(fileName);
            string returnPath = System.Web.HttpContext.Current.Server.MapPath("~/uploadfile/cruises");
            string relativePath = "/doc/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/";
            if (!Directory.Exists(returnPath + relativePath))
            {
                Directory.CreateDirectory(returnPath + relativePath);
            }
            if (File.Exists(returnPath + relativePath + newFileName))
            {
                File.Delete(returnPath + relativePath + newFileName);
            }

            FileStream fileStream = null;
            try
            {
               
                fileStream = new FileStream(returnPath + relativePath + newFileName, FileMode.Create);
                fileStream.Write(bytes, 0, bytes.Length - 1);
                Uri tempUri = System.Web.HttpContext.Current.Request.Url;
               
                string fileServiceAddress = ConfigurationManager.AppSettings["fileServiceAddress"];

                return fileServiceAddress + relativePath + newFileName;
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }

        /// <summary>
        /// 图片加水印(确认单图片)
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="watermarkPath"></param>
        //private static void MakeWatermarkNew(Image originalImage, string watermarkPath)
        //{
        //    try
        //    {
        //        watermarkPath = Tool.ConfigString("ComfirmPictureUrl"); //章的图片
        //        if (string.IsNullOrEmpty(watermarkPath))
        //        {
        //            watermarkPath = "http://img.17u.cn/ly/cn/img/admin/cruises/comfirm.png";
        //        }
        //        WebRequest webreq = WebRequest.Create(watermarkPath);
        //        //红色部分为文件URL地址，这里是一张图片
        //        WebResponse webres = webreq.GetResponse();
        //        Stream stream = webres.GetResponseStream();
        //        System.Drawing.Image copyImage;
        //        copyImage = System.Drawing.Image.FromStream(stream);

        //        // System.Drawing.Image copyImage = System.Drawing.Image.FromFile(watermarkPath);
        //        Graphics g = Graphics.FromImage(originalImage);
        //        g.DrawImage(copyImage,
        //            new Rectangle(3 * (originalImage.Width - copyImage.Width) / 4, (originalImage.Height - copyImage.Height) / 9, copyImage.Width, copyImage.Height)
        //            , 0
        //            , 0
        //            , copyImage.Width
        //            , copyImage.Height
        //            , GraphicsUnit.Pixel);
        //        g.Dispose();

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        /// <summary>
        /// 上传目录按时间分类(判断与创建目录) 
        /// </summary>
        /// <param name="tempUpAddress"></param>
        /// <param name="ftpUser"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static string CheckAndCreateDirectory(string tempUpAddress, string ftpUser, string pass)
        {
            string tempAddress = string.Empty;
            try
            {
                //目录按顺序存到数组中
                string[] tempDic = { "cn", "img", "admin", "Cruises", "doc", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString() };
                for (int i = 0; i < tempDic.Length; i++)
                {
                    if (!CheckDirectoryExist(tempUpAddress + tempAddress, tempDic[i], ftpUser, pass))
                    {
                        MakeDir(tempUpAddress + tempAddress, tempDic[i] + "/", pass, ftpUser);
                    }
                    tempAddress += tempDic[i] + "/";
                }
                tempAddress = "/" + tempAddress;
            }
            catch (Exception)
            {
                return string.Empty;
            }
            return tempAddress;
        }

        //#region  将html转化为图片
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="BrowserWidth"></param>
        ///// <param name="BrowserHeight"></param>
        ///// <param name="ThumbnailWidth"></param>
        ///// <param name="ThumbnailHeight"></param>
        //public MemoryStream GetChangeImgeByHtml(string url, int BrowserWidth, int BrowserHeight, int ThumbnailWidth, int ThumbnailHeight)
        //{
        //    Bitmap m_Bitmap = ImageHelper.GetWebSiteThumbnail(url, BrowserWidth, BrowserHeight, ThumbnailWidth, ThumbnailHeight);
        //    MemoryStream ms = new MemoryStream();
        //    m_Bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);//JPG、GIF、PNG等均可  
        //    return ms;
        //}
        //#endregion

        //#region 根据图片流上传图片
        ///// <summary>
        ///// 根据图片流上传图片
        ///// </summary>
        ///// <param name="url">路径</param>
        ///// <param name="applicationPath"></param>
        ///// <param name="error"></param>
        ///// <returns></returns>
        //public static string UploadFTPByImageStream(string url, int falg, out string error, out string dfsAddress)
        //{
        //    error = string.Empty;
        //    string newFileName = Guid.NewGuid().ToString().Replace("-", "") + "." + System.Drawing.Imaging.ImageFormat.Jpeg.ToString();

        //    string userPath = string.Empty;
        //    string ftpUser = Tool.ConfigString("ftpName");
        //    string address = Tool.ConfigString("ftpAddress");
        //    string pass = Tool.ConfigString("ftppass");
        //    dfsAddress = string.Empty;
        //    try
        //    {

        //        #region 上传目录按时间分类(判断与创建目录)
        //        string tempUpAddress = "ftp://" + address + "/";
        //        string tempAddress = CheckAndCreateDirectory(tempUpAddress, ftpUser, pass);
        //        #endregion

        //        //获取图片
        //        Bitmap m_Bitmap = ImageHelper.GetWebSiteThumbnail(url, 600, 600, 600, 600);
        //        //截去图片 高度-145

        //        Graphics g = Graphics.FromImage(m_Bitmap);
        //        g.DrawImage(m_Bitmap,
        //            new Rectangle(0, 0, m_Bitmap.Width, m_Bitmap.Height)
        //            , 0
        //            , 145
        //            , m_Bitmap.Width
        //            , m_Bitmap.Height - 145
        //            , GraphicsUnit.Pixel);
        //        g.Dispose();

        //        //加水印
        //        if (falg == 0)
        //        {
        //            MakeWatermarkNew(m_Bitmap, string.Empty);
        //        }
        //        MemoryStream stream = new MemoryStream();
        //        m_Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);


        //        FtpHelper ftp = new FtpHelper(address, tempAddress, ftpUser, pass, 21);
        //        if (ftp.UpLoadFile(newFileName, stream, out dfsAddress))
        //        {
        //            userPath = "http://img.17u.cn/ly" + tempAddress + newFileName;
        //        }
        //        //删除图片
        //        stream.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        error = ex.Message;
        //        return string.Empty;
        //    }
        //    return userPath;
        //}

        //#endregion


        //#region 上传截图

        //public static string UploadFTPByImageUrl(string Url, out string ErrorMsg, out string dfsAddress)
        //{
        //    ErrorMsg = string.Empty;
        //    string newFileName = Guid.NewGuid().ToString().Replace("-", "") + "." + System.Drawing.Imaging.ImageFormat.Png.ToString();




        //    string userPath = string.Empty;
        //    string ftpUser = Tool.ConfigString("ftpName");
        //    string address = Tool.ConfigString("ftpAddress");
        //    string pass = Tool.ConfigString("ftppass");
        //    dfsAddress = string.Empty;
        //    try
        //    {
        //        #region 上传目录按时间分类(判断与创建目录)
        //        string tempUpAddress = "ftp://" + address + "/";
        //        string tempAddress = CheckAndCreateDirectory(tempUpAddress, ftpUser, pass);
        //        #endregion

        //        //获取图片
        //        Bitmap m_Bitmap = WebSiteThumbnail.GetWebSiteThumbnail(Url, 850, 850, 850, 850); //ImageHelper.GetWebSiteThumbnail(Url, 1024, 768, 1024, 768);
        //        //截去图片 高度-120

        //        //Graphics g = Graphics.FromImage(m_Bitmap);
        //        //g.DrawImage(m_Bitmap,
        //        //    new Rectangle(0, 0, m_Bitmap.Width, m_Bitmap.Height)
        //        //    , 0
        //        //    , 120
        //        //    , m_Bitmap.Width
        //        //    , m_Bitmap.Height
        //        //    , GraphicsUnit.Pixel);
        //        //g.Dispose();

        //        MemoryStream stream = new MemoryStream();
        //        m_Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        //        FtpHelper ftp = new FtpHelper(address, tempAddress, ftpUser, pass, 21);
        //        if (ftp.UpLoadFile(newFileName, stream, out dfsAddress))
        //        {
        //            userPath = "http://img.17u.cn/ly" + tempAddress + newFileName;
        //        }
        //        //删除图片
        //        stream.Close();

        //        MemoryStream streamDfs = new MemoryStream();
        //        m_Bitmap.Save(streamDfs, System.Drawing.Imaging.ImageFormat.Png);
        //        var byts = new byte[streamDfs.Length];
        //        streamDfs.Write(byts, 0, byts.Length);
        //        streamDfs.Close();

        //        dfsAddress = CommonHelper.SaveFileToDfs(newFileName, byts);

        //        if (string.IsNullOrWhiteSpace(dfsAddress))
        //        {
        //            LogHelper.WriteLog(string.Format("{0}上传失败-分布式文件系统 UploadFTPByImageUrl。", newFileName), EnumState.LogServiceType.DFSFile);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMsg = string.Format("{0}|{1}|{2}", ex.Message, ex.Source, ex.StackTrace);
        //        return string.Empty;
        //    }
        //    return userPath;
        //}
        //#endregion


        //#region 将Word,pdf转化成图片上传

        ///// <summary>上传出团通知书图片</summary>
        ///// <param name="ftp">FtpHelper实例</param>
        ///// <param name="fileName">文件名</param>
        ///// <param name="fileStream">文件流</param>
        //public static List<string> UploadImg(HttpPostedFileBase file, out string res, out List<string> dfsAddressList)
        //{
        //    List<string> listImageUrl = new List<string>();
        //    res = string.Empty;
        //    string newFileName = Guid.NewGuid().ToString().Replace("-", "") + System.IO.Path.GetExtension(file.FileName);
        //    string fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower().Replace(".", "");
        //    string userPath = string.Empty;
        //    string ftpUser = Tool.ConfigString("ftpName");
        //    string address = Tool.ConfigString("ftpAddress");
        //    string pass = Tool.ConfigString("ftppass");

        //    #region 上传目录按时间分类(判断与创建目录)
        //    string tempUpAddress = "ftp://" + address + "/";
        //    string tempAddress = CheckAndCreateDirectory(tempUpAddress, ftpUser, pass);
        //    #endregion

        //    dfsAddressList = new List<string>();
        //    try
        //    {
        //        FtpHelper ftp = new FtpHelper(address, tempAddress, ftpUser, pass, 21);
        //        Stream fs = file.InputStream;
        //        if (fileExtension.Contains("doc") || fileExtension.Contains("docx"))
        //        {
        //            listImageUrl = UploadImgForWord(ftp, newFileName, fs, "http://img.17u.cn/ly" + tempAddress, out res, out dfsAddressList);
        //        }
        //        else if (fileExtension.Contains("pdf"))
        //        {
        //            listImageUrl = UploadImgForPdf(ftp, newFileName, fs, "http://img.17u.cn/ly" + tempAddress, out res, out dfsAddressList);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res = ex.Message;
        //    }
        //    return listImageUrl;
        //}

        ///// <summary>上传出团通知书图片(word转图片)</summary>
        ///// <param name="ftp">FtpHelper实例</param>
        ///// <param name="fileName">文件名</param>
        ///// <param name="fileStream">文件流</param>
        //private static List<string> UploadImgForWord(FtpHelper ftp, string fileName, Stream fileStream, string tempAddress, out string message, out List<string> dfsAddressList)
        //{

        //    MemoryStream ms = null;
        //    List<string> listImage = new List<string>();
        //    message = string.Empty;
        //    dfsAddressList = new List<string>();
        //    try
        //    {
        //        Aspose.Words.Document doc = new Aspose.Words.Document(fileStream);
        //        Aspose.Words.Saving.ImageSaveOptions iso = new Aspose.Words.Saving.ImageSaveOptions(Aspose.Words.SaveFormat.Png);
        //        iso.Resolution = 1000;
        //        iso.Scale = 0.1f;
        //        iso.JpegQuality = 100;
        //        iso.PrettyFormat = true;
        //        iso.UseAntiAliasing = true;
        //        string tempFileName = Path.GetFileNameWithoutExtension(fileName);
        //        for (int i = 0; i < doc.PageCount; i++)
        //        {
        //            string dfsAddress;

        //            ms = new MemoryStream();
        //            iso.PageIndex = i;
        //            doc.Save(ms, iso);
        //            ftp.UpLoadFile(string.Format("{0}_{1}.png", tempFileName, i + 1), ms, out dfsAddress);
        //            listImage.Add(string.Format("{0}_{1}.png", tempAddress + tempFileName, i + 1));
        //            ms.Close();

        //            dfsAddressList.Add(dfsAddress);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        message = ex.Message;
        //    }
        //    finally
        //    {
        //        ms.Close();
        //    }
        //    return listImage;
        //}

        ///// <summary>上传出团通知书图片(pdf转图片)</summary>
        ///// <param name="ftp">FtpHelper实例</param>
        ///// <param name="fileName">文件名</param>
        ///// <param name="fileStream">文件流</param>
        //private static List<string> UploadImgForPdf(FtpHelper ftp, string fileName, Stream fileStream, string tempAddress, out string message, out List<string> dfsAddressList)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    List<string> listImage = new List<string>();
        //    message = string.Empty;
        //    dfsAddressList = new List<string>();
        //    try
        //    {
        //        Aspose.Pdf.Document doc = new Aspose.Pdf.Document(fileStream);
        //        PngDevice pngDevice = new PngDevice(945, 1336, new Resolution(1000));
        //        string tempFileName = Path.GetFileNameWithoutExtension(fileName);
        //        for (int i = 1; i <= doc.Pages.Count; i++)
        //        {
        //            string dfsAddress;

        //            ms = new MemoryStream();
        //            pngDevice.Process(doc.Pages[i], ms);
        //            ftp.UpLoadFile(string.Format("{0}_{1}.png", tempFileName, i), ms, out dfsAddress);
        //            listImage.Add(string.Format("{0}_{1}.png", tempAddress + tempFileName, i));
        //            ms.Close();

        //            dfsAddressList.Add(dfsAddress);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        message = ex.Message;
        //    }
        //    finally
        //    {
        //        ms.Close();
        //    }
        //    return listImage;
        //}

        //#endregion

        //#region 将Word,pdf转化成图片批量上传

        ///// <summary>上传出团通知书图片</summary>
        ///// <param name="ftp">FtpHelper实例</param>
        ///// <param name="fileName">文件名</param>
        ///// <param name="fileStream">文件流</param>
        //public static List<string> UploadImgMany(string tempAddressName, string fileName, int type, out string res, out List<string> dfsAddressList)
        //{
        //    List<string> listImageUrl = new List<string>();
        //    res = string.Empty;
        //    //string newFileName = Guid.NewGuid().ToString().Replace("-", "") + System.IO.Path.GetExtension(file.FileName);
        //    //string fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower().Replace(".", "");
        //    string userPath = string.Empty;
        //    string ftpUser = Tool.ConfigString("ftpName");
        //    string address = Tool.ConfigString("ftpAddress");
        //    string pass = Tool.ConfigString("ftppass");
        //    Dictionary<string, List<string>> dicReturnImage = new Dictionary<string, List<string>>();
        //    #region 上传目录按时间分类(判断与创建目录)
        //    string tempUpAddress = "ftp://" + address + "/";
        //    DateTime dtNow = DateTime.Now;
        //    string tempAddress = "cn/img/admin/Cruises/doc/" + dtNow.Year + "/" + dtNow.Month + "/" + dtNow.Day + "/"; //CheckAndCreateDirectory(tempUpAddress, ftpUser, pass);
        //    #endregion

        //    dfsAddressList = new List<string>();
        //    try
        //    {
        //        FtpHelper ftp = new FtpHelper(address, tempAddress, ftpUser, pass, 21);

        //        // List<string> listImage = new List<string>();
        //        MemoryStream stream = GetStreamChangeFile(tempAddressName, fileName, out res);

        //        if (type == 1)
        //        {
        //            listImageUrl = UploadImgForWord(ftp, fileName, stream, "http://img.17u.cn/ly" + tempAddress, out res, out dfsAddressList);
        //        }
        //        else if (type == 2)
        //        {
        //            listImageUrl = UploadImgForPdf(ftp, fileName, stream, "http://img.17u.cn/ly" + tempAddress, out res, out dfsAddressList);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        res += ex.Message;
        //    }
        //    return listImageUrl;
        //}
        //#endregion

        //#region  将文件转化为流
        ///// <summary>
        ///// 将文件转化为流
        ///// </summary>
        ///// <param name="tempAddress">文件在服务器路径</param>
        ///// <param name="fileName">文件名</param>
        ///// <param name="res"></param>
        ///// <returns></returns>
        //public static MemoryStream GetStreamChangeFile(string tempAddress, string fileName, out string res)
        //{
        //    string ftpUser = Tool.ConfigString("ftpName");
        //    string address = Tool.ConfigString("ftpAddress");
        //    string pass = Tool.ConfigString("ftppass");
        //    res = string.Empty;
        //    MemoryStream stream = null;
        //    try
        //    {
        //        FtpHelper ftp = new FtpHelper(address, tempAddress, ftpUser, pass, 21);
        //        stream = ftp.DownLoadFile(fileName);
        //    }
        //    catch (Exception ex)
        //    {
        //        res += ex.Message;
        //    }
        //    return stream;
        //}

        //#endregion

    }
}
