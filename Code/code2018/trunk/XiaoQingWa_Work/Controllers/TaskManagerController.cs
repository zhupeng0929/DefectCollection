using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XiaoQingWa_Work_Model.Entity;

namespace XiaoQingWa_Work.Controllers
{
    public class TaskManagerController : BaseController
    {
        // GET: TaskManager
        public ActionResult TaskList()
        {
            return View();
        }
       
        public ActionResult TaskListResult(LineQuery lineQuery)
        {
            var taskList = tTaskRepository.GetTTaskList(lineQuery);
            return View(taskList);
        }
    }
}