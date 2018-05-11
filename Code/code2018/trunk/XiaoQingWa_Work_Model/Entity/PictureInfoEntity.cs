//---------------------------------------------------------------------------------
// <copyright company="" file="PictureInfo.cs" >
//      Author: Peng.Zhu
//      Create Time: 2017/12/20 20:51:21
//      Description: 
// </copyright>
//---------------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;
using System.ComponentModel;
using Dapper;

namespace XiaoQingWa_Work_Model.Entity
{
    /// <summary>
    /// 图片信息实体
    /// </summary>
    [Table("PictureInfo")]
    public partial class PictureInfoEntity
    {
        private int _id = 0;
        private string _oldName = String.Empty;
        private string _filePath = String.Empty;
        private string _fileMD5 = String.Empty;
        private DateTime _createTime = DateTime.Parse("1900-1-1");


        /// <summary>
        /// 主键id
        /// </summary>
        [Key]
        [Description("主键id")]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 图片原名
        /// </summary>
        [Description("图片原名")]
        public string OldName
        {
            get { return _oldName; }
            set { _oldName = value; }
        }

        /// <summary>
        /// 图片路径
        /// </summary>
        [Description("图片路径")]
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        /// <summary>
        /// 文件MD5值
        /// </summary>
        [Description("文件MD5值")]
        public string FileMD5
        {
            get { return _fileMD5; }
            set { _fileMD5 = value; }
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        [Description("添加时间")]
        [IgnoreUpdate]
        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }


    }
}

