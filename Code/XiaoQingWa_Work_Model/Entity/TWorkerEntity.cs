/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////实体类//////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////

//---------------------------------------------------------------------------------
// <copyright company="" file="tWorker.cs" >
//      Author: Peng.Zhu
//      Create Time: 2018/5/5 20:40:19
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
	[Table("tWorker")]
    public partial class TWorkerEntity
    {
        private int _wId = 0;
        private string _wName = String.Empty;
        private string _wNo = String.Empty;
        private bool _wSex = false;
        private string _wDescript = String.Empty;
        private string _lineCode = String.Empty;
        private int _status = 0;
        private int _orderIndex = 0;
        private DateTime _refreshDate = DateTime.Now;


        /// <summary>
        /// 工人ID
        /// </summary>
        [Key]
        [Description("工人ID")]
        public int WId
        {
            get { return _wId; }
            set { _wId = value; }
        }

        /// <summary>
        /// 工人姓名
        /// </summary>
        [Description("工人姓名")]
        public string WName
        {
            get { return _wName; }
            set { _wName = value; }
        }

        /// <summary>
        /// 工人编号
        /// </summary>
        [Description("工人编号")]
        public string WNo
        {
            get { return _wNo; }
            set { _wNo = value; }
        }

        /// <summary>
        /// 工人性别
        /// </summary>
        [Description("工人性别")]
        public bool WSex
        {
            get { return _wSex; }
            set { _wSex = value; }
        }

        /// <summary>
        /// 备注描述
        /// </summary>
        [Description("备注描述")]
        public string WDescript
        {
            get { return _wDescript; }
            set { _wDescript = value; }
        }

        /// <summary>
        /// 工人在哪条流水线上工作
        /// </summary>
        [Description("工人在哪条流水线上工作")]
        public string LineCode
        {
            get { return _lineCode; }
            set { _lineCode = value; }
        }

        /// <summary>
        /// 员工状态，0-正常上班 1-请假或缺勤
        /// </summary>
        [Description("员工状态，0-正常上班 1-请假或缺勤")]
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>
        /// 自动排序序号，每天有任务工单时，自动-1，如果为0，设为最大值。按照流水线为单位进行循环。
        /// </summary>
        [Description("自动排序序号，每天有任务工单时，自动-1，如果为0，设为最大值。按照流水线为单位进行循环。")]
        public int OrderIndex
        {
            get { return _orderIndex; }
            set { _orderIndex = value; }
        }

        /// <summary>
        /// 排序更新日期
        /// </summary>
        [Description("排序更新日期")]
        public DateTime RefreshDate
        {
            get { return _refreshDate; }
            set { _refreshDate = value; }
        }
    }
}
