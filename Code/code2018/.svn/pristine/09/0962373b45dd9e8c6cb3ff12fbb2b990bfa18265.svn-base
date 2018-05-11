/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////实体类//////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////

//---------------------------------------------------------------------------------
// <copyright company="" file="tTaskWorker.cs" >
//      Author: Peng.Zhu
//      Create Time: 2018/5/5 21:10:03
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
    [Table("tTaskWorker")]
    public partial class TTaskWorkerEntity
    {
        private string _billNO = String.Empty;
        private string _lineCode = String.Empty;
        private string _stationCode = String.Empty;
        private int _wId = 0;
        private int _iD = 0;


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
        public string LineCode
        {
            get { return _lineCode; }
            set { _lineCode = value; }
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
        public int WId
        {
            get { return _wId; }
            set { _wId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Description("")]
        public int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }


    }
}