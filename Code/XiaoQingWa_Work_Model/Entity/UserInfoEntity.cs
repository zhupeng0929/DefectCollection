using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaoQingWa_Work_Model.Entity
{
    /// <summary>
	/// 实体
	/// </summary>
	[Table("UserInfo")]
    public partial class UserInfoEntity
    {
        private int _userId = 0;
        private string _passWord = String.Empty;
        private string _userName = String.Empty;
        private byte _userSex = 0;
        private string _userPhone = String.Empty;
        private string _userMail = String.Empty;
        private string _userAddress = String.Empty;
        private DateTime _createDate = DateTime.Parse("1900-1-1");
        private byte _userState = 0;
        private byte _isDelete = 0;


        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Description("id")]
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        /// <summary>
        /// 密码
        /// </summary>
        [Description("密码")]
        public string PassWord
        {
            get { return _passWord; }
            set { _passWord = value; }
        }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [Description("用户姓名")]
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        /// <summary>
        /// 用户性别
        /// </summary>
        [Description("用户性别")]
        public byte UserSex
        {
            get { return _userSex; }
            set { _userSex = value; }
        }

        /// <summary>
        /// 电话
        /// </summary>
        [Description("电话")]
        public string UserPhone
        {
            get { return _userPhone; }
            set { _userPhone = value; }
        }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Description("邮箱")]
        public string UserMail
        {
            get { return _userMail; }
            set { _userMail = value; }
        }

        /// <summary>
        /// 地址
        /// </summary>
        [Description("地址")]
        public string UserAddress
        {
            get { return _userAddress; }
            set { _userAddress = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态")]
        public byte UserState
        {
            get { return _userState; }
            set { _userState = value; }
        }

        /// <summary>
        /// 是否删除
        /// </summary>
        [Description("是否删除")]
        public byte IsDelete
        {
            get { return _isDelete; }
            set { _isDelete = value; }
        }


    }


    public class UserQuery
    {
        public DateTime? datemin { get; set; }
        public DateTime? datemax { get; set; }
        public string keyWords { get; set; }
    }
}
