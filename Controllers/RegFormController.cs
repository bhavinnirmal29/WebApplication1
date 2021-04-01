using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class RegFormController : Controller
    {
        LTIMVCEntities db = new LTIMVCEntities();
        // GET: RegForm
        [HttpGet]
        public ActionResult InsertNewEmployee()
        {
            ViewData["proj"] = new SelectList(db.ProjectInfoes.ToList(), "projid", "projname");
            return View();
        }
        [HttpPost]
        public ActionResult InsertNewEmployee(Employee emp)
        {
            int eid = Convert.ToInt32(Request.Form["txtempid"]);
            string name = Request.Form["txtempName"];
            string dept = Request.Form["ddldept"];
            string desg = Request.Form["ddldesg"];
            decimal salary = Convert.ToDecimal(Request.Form["txtSalary"]);
            int pid = Convert.ToInt32(Request.Form["ddlpid"]);
            emp.EmpID = eid;
            emp.EmpName = name;
            emp.Dept = dept;
            emp.Desg = desg;
            emp.Salary = salary;
            emp.projid = pid;
            ModelState.AddModelError(" ", emp.EmpID + " " + emp.EmpName + " " + emp.Dept + " " + emp.Desg + " " + emp.Salary+" "+emp.projid);
            db.Employees.Add(emp);
            int res = db.SaveChanges();
            if(res>0)
            {
                ModelState.AddModelError("", "New Employee Inserted");
            }
            return RedirectToAction("GetEmployees");
        }
        //Retrive all employee details from db when the form is Loading
        public ActionResult GetEmployees()
        {
            var data = db.Employees.ToList();
            return View(data); //Model Binding
        }
        //Retrive Employee details from the db based on condition
        [HttpGet]
        public ActionResult GetEmployeeByDeptSalary()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetEmployeeByDeptSalary(string dept, decimal salary = 10000)
        {
            dept = Request.Form["txtdept"];
            salary = Convert.ToDecimal(Request.Form["txtsalary"]);
            var query = from t in db.Employees
                        where t.Dept == dept && t.Salary >= salary
                        select t;
            //Lambda
            var lambda = db.Employees.Where(x => x.Dept == dept && x.Salary >= salary);
            if (query.Count() == 0)
            {
                ModelState.AddModelError("", "No Data found");
            }
            else
            {
                //This will pass data from controller to view
                Session["data"] = query;
            }
            return View();
        }
        [HttpGet]
        public ActionResult UpdateEmployee(int id)
        {
            var data = db.Employees.Where(x => x.EmpID == id).SingleOrDefault();
            return View(data);
        }
        [HttpPost]
        public ActionResult UpdateEmployee()
        {
            int id = Convert.ToInt32(Request.Form["eid"]);
            var olddata = db.Employees.Where(x => x.EmpID == id).SingleOrDefault();
            
            var newname = Request.Form["name"];
            var newdept = Request.Form["dept"];
            var newdesg = Request.Form["desg"];
            var newsal = Convert.ToDecimal(Request.Form["sal"]);
            var newpid = Convert.ToInt32(Request.Form["pid"]);
            olddata.EmpID = id;
            olddata.EmpName = newname;
            olddata.Dept = newdept;
            olddata.Desg = newdesg;
            olddata.Salary = newsal;
            olddata.projid = newpid;
            var res = db.SaveChanges();
            if(res >= 0)
            {
                return RedirectToAction("GetEmployees");
            }
            return View();
        }
        [HttpGet]
        public ActionResult DeletEmployee(int id)
        {
            var data = db.Employees.Where(x => x.EmpID == id).SingleOrDefault();
            return View(data);
        }
        [HttpPost]
        public ActionResult DeletEmployee()
        {
            int id = Convert.ToInt32(Request.Form["eid"]);
            var delrow = db.Employees.Where(x => x.EmpID == id).SingleOrDefault();
            db.Employees.Remove(delrow);
            var res = db.SaveChanges();
            if (res > 0)
                return RedirectToAction("GetEmployees");
            return RedirectToAction("GetEmployees");
        }
        [HttpGet]
        public ActionResult InsertProject()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertProject(ProjectInfo pinfo)
        {
            if(ModelState.IsValid)
            {
                db.ProjectInfoes.Add(pinfo);
                var res = db.SaveChanges();
                if(res>0)
                {
                    Response.Write("<script>alert('New Project Created');</script>");
                    ModelState.AddModelError("", "New Project Added");
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult GetEmpProject()
        {
            var data = (from e in db.Employees
                        join p in db.ProjectInfoes
                        on e.projid equals p.projid
                        select new CustomEmpProject { EmpId = e.EmpID, Name = e.EmpName, 
                            Dept = e.Dept, ProjName = p.projname, Domain = p.domain }).ToList();
            return View(data);
        }
        [HttpGet]
        public ActionResult SelectAndUpdateProjectSP()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SelectAndUpdateProjectSP(int? id,string command)
        {
            //For Multiple Values
            /*id = Convert.ToInt32(Request.Form["pid"]);
            var res = db.sp_SelectProjectbyId(id).ToList();
            if(res.Count() == 0)
            {
                ModelState.AddModelError("", "Invalid Project ID");
            }
            else
            {
                List<sp_SelectProjectbyId_Result> emps = new List<sp_SelectProjectbyId_Result>();
                foreach (var item in res.ToList())
                {
                    emps.Add(new sp_SelectProjectbyId_Result { projid = item.projid,projname=item.projname,domain = item.domain });
                }
                foreach(var item in emps)
                {
                    ModelState.AddModelError("", item.projid + "  " + item.projname + " " + item.domain);
                }
            }
            ViewBag.data = res;
            return View();*/

            //OR 
            
            if (command == "Select")
            {
                id = Convert.ToInt32(Request.Form["pid"]);
                var result = db.sp_SelectProjectbyId(id).SingleOrDefault();
                if (result == null)
                    ModelState.AddModelError("", "Invalid Id");
                else
                    ModelState.AddModelError("", result.projid + "," + result.projname + "," + result.domain);

                ViewBag.data = result; //To display data from controller to view

                return View();
            }
            else if (command == "Update")
            {
                int projid = Convert.ToInt32(Request.Form["projid"]);
                string newname = Request.Form["pname"];
                string newdomain = Request.Form["pdomain"];
                var res = db.sp_updateProject(projid, newname, newdomain);
                if(res>=0)
                {
                    ModelState.AddModelError("", "Data Updated");
                }
                return View();
            }
            else
            {
                return View();
            }
        }
    }
}