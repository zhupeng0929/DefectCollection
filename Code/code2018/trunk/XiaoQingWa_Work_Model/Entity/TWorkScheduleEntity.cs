/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////实体类//////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////

//---------------------------------------------------------------------------------
// <copyright company="" file="tWorkSchedule.cs" >
//      Author: Peng.Zhu
//      Create Time: 2018/5/5 21:17:27
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
    [Table("tWorkSchedule")]
    public partial class TWorkScheduleEntity
    {
        private int _id = 0;
        private string _billNO = String.Empty;
        private int _wId = 0;
        private string _wName = String.Empty;
        private string _stationCode = String.Empty;
        private int _stationIndex = 0;
        private string _stationName = String.Empty;
        private string _goodBtnId = String.Empty;
        private string _badBtnId = String.Empty;
        private string _lineCode = String.Empty;
        private DateTime _date = DateTime.Parse("1900-1-1");
        private int _goodCount = 0;
        private int _badCount = 0;
        private int _status = 0;


        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Description("")]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 任务单
        /// </summary>
        [Description("任务单")]
        public string BillNO
        {
            get { return _billNO; }
            set { _billNO = value; }
        }

        /// <summary>
        /// 工人id
        /// </summary>
        
        [Description("工人id")]
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
        /// 工位code
        /// </summary>
        [Description("工位code")]
        public string StationCode
        {
            get { return _stationCode; }
            set { _stationCode = value; }
        }

        /// <summary>
        /// 工位索引排序
        /// </summary>
        [Description("工位索引排序")]
        public int StationIndex
        {
            get { return _stationIndex; }
            set { _stationIndex = value; }
        }

        /// <summary>
        /// 工位名称
        /// </summary>
        [Description("工位名称")]
        public string StationName
        {
            get { return _stationName; }
            set { _stationName = value; }
        }

        /// <summary>
        /// 汇报好件的按键序列号
        /// </summary>
        [Description("汇报好件的按键序列号")]
        public string GoodBtnId
        {
            get { return _goodBtnId; }
            set { _goodBtnId = value; }
        }

        /// <summary>
        /// 汇报坏件的按键序列号
        /// </summary>
        [Description("汇报坏件的按键序列号")]
        public string BadBtnId
        {
            get { return _badBtnId; }
            set { _badBtnId = value; }
        }

        /// <summary>
        /// 流水线code
        /// </summary>
        [Description("流水线code")]
        public string LineCode
        {
            get { return _lineCode; }
            set { _lineCode = value; }
        }

        /// <summary>
        /// 日期
        /// </summary>
        [Description("日期")]
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
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
        /// 工作任务安排单，0-未开始，1-启动，2-结束
        /// </summary>
        [Description("工作任务安排单，0-未开始，1-启动，2-结束")]
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }


    }
}