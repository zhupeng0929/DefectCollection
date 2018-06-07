using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaoQingWa_Work_Model.Entity
{
    public class WorkerStatic : TWorkerEntity
    {
        public int TaskCount { get; set; }

        public string TaskCode { get; set; }
        public string TaskName { get; set; }

        public int GoodCount { get; set; }

        public int BadCount { get; set; }
        public DateTime StartDateTime { get; set; }
    }

    public class LineStatic : TLineEntity
    {
        public int Qty { get; set; }
        public int GoodCount { get; set; }

        public int BadCount { get; set; }



        public int DiffCount { get; set; }
    }
}
