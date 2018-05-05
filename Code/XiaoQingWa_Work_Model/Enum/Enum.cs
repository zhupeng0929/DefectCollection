using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaoQingWa_Work_Model.Enum
{
    public enum UserStatu
    {
        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        Enable = 0,

        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Disable = 1
    }
    public enum Sex
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        N = 0,
        /// <summary>
        /// 男
        /// </summary>
        [Description("男")]
        M = 1,

        /// <summary>
        /// 女
        /// </summary>
        [Description("女")]
        W = 2,

        /// <summary>
        /// 保密
        /// </summary>
        [Description("保密")]
        S = 3,
    }

   
}
