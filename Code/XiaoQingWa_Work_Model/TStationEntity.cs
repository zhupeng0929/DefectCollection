/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////实体类//////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////

//---------------------------------------------------------------------------------
// <copyright company="" file="tStation.cs" >
//      Author: Peng.Zhu
//      Create Time: 2018/5/5 21:01:56
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
    [Table("tStation")]
    public partial class TStationEntity
    {
        private string _stationCode = String.Empty;
        private string _stationName = String.Empty;
        private string _stationFullName = String.Empty;
        private string _lineCode = String.Empty;
        private string _lineName = String.Empty;
        private int _stationIndex = 0;
        private string _jianKongPoint = String.Empty;
        private string _goodBtnId = String.Empty;
        private string _badBtnId = String.Empty;
        private DateTime _goodLastTime = DateTime.Parse("1900-1-1");
        private DateTime _badLastTime = DateTime.Parse("1900-1-1");
        private int _status = 0;


        /// <summary>
        /// 工位编码
        /// </summary>
        [Key]
        [Description("工位编码")]
        public string StationCode
        {
            get { return _stationCode; }
            set { _stationCode = value; }
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
        /// 工位全称
        /// </summary>
        [Description("工位全称")]
        public string StationFullName
        {
            get { return _stationFullName; }
            set { _stationFullName = value; }
        }

        /// <summary>
        /// 流水线编码
        /// </summary>
        [Description("流水线编码")]
        public string LineCode
        {
            get { return _lineCode; }
            set { _lineCode = value; }
        }

        /// <summary>
        /// 流水线名称
        /// </summary>
        [Description("流水线名称")]
        public string LineName
        {
            get { return _lineName; }
            set { _lineName = value; }
        }

        /// <summary>
        /// 工位顺序索引
        /// </summary>
        [Description("工位顺序索引")]
        public int StationIndex
        {
            get { return _stationIndex; }
            set { _stationIndex = value; }
        }

        /// <summary>
        /// 监控点位
        /// </summary>
        [Description("监控点位")]
        public string JianKongPoint
        {
            get { return _jianKongPoint; }
            set { _jianKongPoint = value; }
        }

        /// <summary>
        /// 好件按钮硬件序列号
        /// </summary>
        [Description("好件按钮硬件序列号")]
        public string GoodBtnId
        {
            get { return _goodBtnId; }
            set { _goodBtnId = value; }
        }

        /// <summary>
        /// 坏件按钮硬件序列号
        /// </summary>
        [Description("坏件按钮硬件序列号")]
        public string BadBtnId
        {
            get { return _badBtnId; }
            set { _badBtnId = value; }
        }

        /// <summary>
        /// 好件最近接收信号时间
        /// </summary>
        [Description("好件最近接收信号时间")]
        public DateTime GoodLastTime
        {
            get { return _goodLastTime; }
            set { _goodLastTime = value; }
        }

        /// <summary>
        /// 坏件最近接收信号时间
        /// </summary>
        [Description("坏件最近接收信号时间")]
        public DateTime BadLastTime
        {
            get { return _badLastTime; }
            set { _badLastTime = value; }
        }

        /// <summary>
        /// 工位状态，0-正常，1-故障暂停使用
        /// </summary>
        [Description("工位状态，0-正常，1-故障暂停使用")]
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }


    }
}
