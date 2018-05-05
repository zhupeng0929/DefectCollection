/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////实体类//////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////

//---------------------------------------------------------------------------------
// <copyright company="" file="tCounter.cs" >
//      Author: Peng.Zhu
//      Create Time: 2018/5/5 20:52:26
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
    [Table("tCounter")]
    public partial class TCounterEntity
    {
        private int _cId = 0;
        private string _countNo = String.Empty;
        private int _status = 0;


        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Description("")]
        public int CId
        {
            get { return _cId; }
            set { _cId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string CountNo
        {
            get { return _countNo; }
            set { _countNo = value; }
        }

        /// <summary>
        /// 0-未启用，1-启用
        /// </summary>
        [Description("0-未启用，1-启用")]
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }


    }
}
