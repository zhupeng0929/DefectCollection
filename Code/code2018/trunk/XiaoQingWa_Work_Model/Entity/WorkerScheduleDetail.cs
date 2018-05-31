
using System;
using System.Runtime.Serialization;
using System.ComponentModel;
using Dapper;


namespace XiaoQingWa_Work_Model.Entity
{
    [Table("tWorkerScheduleDetail")]
    public class WorkerScheduleDetail
    {
        [Key]
        public int Id { get; set; }

        public string LineCode { get; set; }

        public int WId { get; set; }

        public int StationIndex { get; set; }

    }
}
