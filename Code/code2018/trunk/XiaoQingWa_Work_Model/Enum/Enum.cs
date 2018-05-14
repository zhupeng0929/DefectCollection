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
        Disable    = 0,

        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Enable = 1
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
    public enum WorkerSex
    {
        /// <summary>
        /// 男
        /// </summary>
        [Description("男")]
        M = 0,

        /// <summary>
        /// 女
        /// </summary>
        [Description("女")]
        W = 1,
    }
   public enum StationStatu
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Enable = 0,

        /// <summary>
        /// 故障暂停使用
        /// </summary>
        [Description("故障暂停使用")]
        Disable  = 1
    }

    public enum CounterStatu
    {
        /// <summary>
        /// 未启用
        /// </summary>
        [Description("未启用")]
        Disable = 0,

        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Enable = 1
    }

    public enum WorkerStatu
    {
        /// <summary>
        /// 正常上班
        /// </summary>
        [Description("正常上班")]
        Common = 0,

        /// <summary>
        /// 请假或缺勤
        /// </summary>
        [Description("请假或缺勤")]
        Leave = 1
    }

    public enum RecordStatu
    {
        /// <summary>
        /// 有效
        /// </summary>
        [Description("有效")]
        Valid = 0,

        /// <summary>
        /// 无效
        /// </summary>
        [Description("无效")]
        UnValid = 1
    }
}
