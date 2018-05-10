using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XiaoQingWa_Work_Model.Entity;
using XiaoQingWa_Work_Utility;
namespace XiaoQingWa_Work.Controllers
{
    
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SaveUploadFile()
        {
            ReturnJsonMessage returnJsonMessage = new ReturnJsonMessage();
            HttpFileCollectionBase imgUpFile = Request.Files;
            if (imgUpFile == null || imgUpFile.Count == 0)
            {
                return Content("上传文件不能为空", "text/html;charset=UTF-8");
            }
            try
            {

                var pictureMD5 = CommonHelper.GetMD5HashFromFile(imgUpFile[0].InputStream);
                var pictureInfo = pictureInfoRepository.GetPictureInfoByFileMD5(pictureMD5);
                if (pictureInfo != null && pictureInfo.Id > 0)//存在
                {

                    returnJsonMessage.Text = "true";
                    returnJsonMessage.Value = pictureInfo.Id.ToString();
                }
                else//不存在
                {
                    string filePath = UploadHelper.SaveFileMethod(imgUpFile[0]);
                    var newPictureInfo = new PictureInfoEntity();
                    newPictureInfo.CreateTime = DateTime.Now;
                    newPictureInfo.FileMD5 = pictureMD5;
                    newPictureInfo.FilePath = filePath;
                    newPictureInfo.OldName = imgUpFile[0].FileName;
                    var id = pictureInfoRepository.AddReturnInt(newPictureInfo);//新增图片
                    returnJsonMessage.Text = "true";
                    returnJsonMessage.Value = id.ToString();
                }
            }
            catch (Exception ex)
            {
                returnJsonMessage.Text = "false";
                returnJsonMessage.Value = ex.Message;
            }
            return Content(JsonHelper.Serializer(returnJsonMessage));//返回图片id
        }
    }
}