/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////实体类//////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////

//---------------------------------------------------------------------------------
// <copyright company="" file="tTask.cs" >
//      Author: Peng.Zhu
//      Create Time: 2018/5/5 21:03:14
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
    /// 实体
    /// </summary>
    [Table("tTask")]
    public partial class TTaskEntity
    {
        private string _billNO = String.Empty;
        private string _tName = String.Empty;
        private DateTime _startDateTime = DateTime.Parse("1900-1-1");
        private DateTime _endDateTime = DateTime.Parse("1900-1-1");
        private string _lineCode = String.Empty;
        private string _itemCode = String.Empty;
        private string _itemName = String.Empty;
        private int _qty = 0;
        private int _goodCount = 0;
        private int _badCount = 0;
        private int _diffCount = 0;


        /// <summary>
        /// 任务单号
        /// </summary>
        [Key]
        [Required]
        [IgnoreUpdate]
        [Description("任务单号")]
        public string BillNO
        {
            get { return _billNO; }
            set { _billNO = value; }
        }

        /// <summary>
        /// 任务单名称
        /// </summary>
        [Description("任务单名称")]
        public string TName
        {
            get { return _tName; }
            set { _tName = value; }
        }

        /// <summary>
        /// 任务单开始时间
        /// </summary>
        [Description("任务单开始时间")]
        public DateTime StartDateTime
        {
            get { return _startDateTime; }
            set { _startDateTime = value; }
        }

        /// <summary>
        /// 任务单结束时间
        /// </summary>
        [Description("任务单结束时间")]
        public DateTime EndDateTime
        {
            get { return _endDateTime; }
            set { _endDateTime = value; }
        }

        /// <summary>
        /// 流水线号
        /// </summary>
        [Description("流水线号")]
        public string LineCode
        {
            get { return _lineCode; }
            set { _lineCode = value; }
        }

        /// <summary>
        /// 物料代码
        /// </summary>
        [Description("物料代码")]
        public string ItemCode
        {
            get { return _itemCode; }
            set { _itemCode = value; }
        }

        /// <summary>
        /// 物料名称
        /// </summary>
        [Description("物料名称")]
        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; }
        }

        /// <summary>
        /// 物料数量
        /// </summary>
        [Description("物料数量")]
        public int Qty
        {
            get { return _qty; }
            set { _qty = value; }
        }

        /// <summary>
        /// 好件统计
        /// </summary>
        [Description("好件统计")]
        public int GoodCount
        {
            get { return _goodCount; }
            set { _goodCount = value; }
        }

        /// <summary>
        /// 坏件统计
        /// </summary>
        [Description("坏件统计")]
        public int BadCount
        {
            get { return _badCount; }
            set { _badCount = value; }
        }

        /// <summary>
        /// 汇总差额
        /// </summary>
        [Description("汇总差额")]
        public int DiffCount
        {
            get { return _diffCount; }
            set { _diffCount = value; }
        }

        /// <summary>
        /// 操作类型0-新增，1-修改，2-删除
        /// </summary>
        [NotMapped]

        public int OperateType { get; set; }

        /// <summary>
        /// 0-未启动，1-启动
        /// </summary>
        public int Status { get; set; }

        public bool IsOrder { get; set; } = false;
    }
}