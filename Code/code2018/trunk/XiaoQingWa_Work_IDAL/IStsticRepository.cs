using System.Data;
using System.Collections.Generic;
using XiaoQingWa_Work_Model.Entity;

namespace XiaoQingWa_Work_IDAL
{
    public interface IStsticRepository
    {
        /// <summary>
        /// 根据任务统计
        /// </summary>
        /// <param name="taskEntity"></param>
        /// <returns></returns>
        List<TTaskEntity> StaticByTask(StsticQuery taskEntity);
        /// <summary>
        /// 根据工人统计
        /// </summary>
        /// <param name="taskEntity"></param>
        /// <returns></returns>
        List<WorkerStatic> StaticByWorker(StsticQuery taskEntity);
        /// <summary>
        /// 根据流水线统计
        /// </summary>
        /// <param name="taskEntity"></param>
        /// <returns></returns>
        List<LineStatic> StaticByLine(StsticQuery taskEntity);
    }
}
