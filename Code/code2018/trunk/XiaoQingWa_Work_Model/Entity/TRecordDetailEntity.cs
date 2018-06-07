/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////实体类//////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////

//---------------------------------------------------------------------------------
// <copyright company="" file="tRecordDetail.cs" >
//      Author: Peng.Zhu
//      Create Time: 2018/5/5 20:59:34
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
    [Table("tRecordDetail")]
    public partial class TRecordDetailEntity
    {
        private int _rId = 0;
        private string _billNO = String.Empty;
        private string _tName = String.Empty;
        private string _lineCode = String.Empty;
        private string _lineName = String.Empty;
        private string _stationCode = String.Empty;
        private string _stationName = String.Empty;
        private int _wId = 0;
        private string _wName = String.Empty;
        private string _countType = String.Empty;
        private int _count = 0;
        private string _btnId = String.Empty;
        private DateTime _receiveTime = DateTime.Parse("1900-1-1");
        private int _status = 0;


        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Description("")]
        public int RId
        {
            get { return _rId; }
            set { _rId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string BillNO
        {
            get { return _billNO; }
            set { _billNO = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string TName
        {
            get { return _tName; }
            set { _tName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string LineCode
        {
            get { return _lineCode; }
            set { _lineCode = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string LineName
        {
            get { return _lineName; }
            set { _lineName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string StationCode
        {
            get { return _stationCode; }
            set { _stationCode = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string StationName
        {
            get { return _stationName; }
            set { _stationName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int WId
        {
            get { return _wId; }
            set { _wId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string WName
        {
            get { return _wName; }
            set { _wName = value; }
        }

        /// <summary>
        /// 技术类型，好件/坏件
        /// </summary>
        [Description("技术类型，好件/坏件")]
        public string CountType
        {
            get { return _countType; }
            set { _countType = value; }
        }

        /// <summary>
        /// 数量
        /// </summary>
        [Description("数量")]
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        /// <summary>
        /// 计数器设备编号
        /// </summary>
        [Description("计数器设备编号")]
        public string BtnId
        {
            get { return _btnId; }
            set { _btnId = value; }
        }

        /// <summary>
        /// 计数器发送时间
        /// </summary>
        [Description("计数器发送时间")]
        public DateTime ReceiveTime
        {
            get { return _receiveTime; }
            set { _receiveTime = value; }
        }

        /// <summary>
        /// 本次计数是否有效，默认有效0-有效，1-无效
        /// </summary>
        [Description("本次计数是否有效，默认有效0-有效，1-无效")]
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }


    }

    public class RecordDetailQuery
    {
        public string keyWords { get; set; }
    }
    public class StsticQuery : TRecordDetailEntity
    {
        public DateTime startTime { get; set; }

        public DateTime endTime { get; set; }
    }
}
