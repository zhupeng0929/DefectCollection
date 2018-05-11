/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////实体类//////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////

//---------------------------------------------------------------------------------
// <copyright company="" file="tLine.cs" >
//      Author: Peng.Zhu
//      Create Time: 2018/5/5 20:56:43
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
    [Table("tLine")]
    public partial class TLineEntity
    {
        private string _lCode = String.Empty;
        private string _lName = String.Empty;
        private string _lFullName = String.Empty;
        private string _lType = String.Empty;
        private int _stationCount = 0;


        /// <summary>
        /// 流水线编码
        /// </summary>
       
        [Description("流水线编码")]
        public string LCode
        {
            get { return _lCode; }
            set { _lCode = value; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称")]
        public string LName
        {
            get { return _lName; }
            set { _lName = value; }
        }

        /// <summary>
        /// 全称
        /// </summary>
        [Description("全称")]
        public string LFullName
        {
            get { return _lFullName; }
            set { _lFullName = value; }
        }

        /// <summary>
        /// 类型
        /// </summary>
        [Description("类型")]
        public string LType
        {
            get { return _lType; }
            set { _lType = value; }
        }

        /// <summary>
        /// 工位数量，station表的增删时，必须更新这个字段
        /// </summary>
        [Description("工位数量，station表的增删时，必须更新这个字段")]
        public int StationCount
        {
            get { return _stationCount; }
            set { _stationCount = value; }
        }
        [Description("自增id")]
        [Key]
        public int LId
        {
            get;
            set;
        }


    }

    public class LineQuery
    {
        public string keyWords { get; set; }
    }
}