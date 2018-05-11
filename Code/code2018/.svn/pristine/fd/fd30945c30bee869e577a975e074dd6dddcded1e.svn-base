using System.Collections.Generic;
using System.Data;
using XiaoQingWa_Work_Model.Entity;
namespace XiaoQingWa_Work_IDAL
{
    public interface IPictureInfoRepository : IDependency<PictureInfoEntity>
    {
        bool ExistPincture(string pictureMD5);
      
        bool DelPictureInfo(int id);
        bool DelPictureInfoBatch(int[] ids);
       
        /// <summary>
        /// 根据Md5查找图片信息
        /// </summary>
        /// <param name="fileMD5"></param>
        /// <returns></returns>
        PictureInfoEntity GetPictureInfoByFileMD5(string fileMD5);
       
      
    }
}