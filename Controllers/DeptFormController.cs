using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class DeptFormController : Controller
    {
        [HttpGet]
        // GET: DeptForm
        public ActionResult InsertNewDepartment()
        {
            return View();
        }
        [HttpPost]
        public ActionResult InsertNewDepartment(Department dept)
        {
            LTIMVCEntities1 db1 = new LTIMVCEntities1();
            dept.DeptId = Convert.ToInt32(Request.Form["txtDeptId"]);
            dept.DeptName = Request.Form["ddldept1"];
            dept.DeptLoc = Request.Form["ddllocation"];
            ModelState.AddModelError("",dept.DeptId + " " + dept.DeptName + " " + dept.DeptLoc + " ");
            db1.Departments.Add(dept);
            int res1 = db1.SaveChanges();
            if (res1 > 0)
            {
                ModelState.AddModelError("", "New Department Inserted");
            }
            return View();
        }
    }
}