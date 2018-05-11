/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////实体类//////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////

//---------------------------------------------------------------------------------
// <copyright company="" file="tWorkerLine.cs" >
//      Author: Peng.Zhu
//      Create Time: 2018/5/5 21:13:45
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
    [Table("tWorkerLine")]
    public partial class TWorkerLineEntity
    {
        private int _wId = 0;
        private string _lCode = String.Empty;


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
        /// 流水线编号
        /// </summary>
        [Key]
        [Description("流水线编号")]
        public string LCode
        {
            get { return _lCode; }
            set { _lCode = value; }
        }


    }
}
