using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class ProductAddController : Controller
    {
        LTIMVCEntities3 db2 = new LTIMVCEntities3();
        [HttpGet]
        // GET: ProductAdd
        public ActionResult InsertNewProduct()
        {
            return View();
        }
        [HttpPost]
        public ActionResult InsertNewProduct(Product pm)
        {
            pm.ProdId = Convert.ToInt32(Request.Form["pid"]);
            pm.ProdName = Request.Form["pname"];
            pm.ProdManufacturer = Request.Form["ddlManufacturer"];
            pm.ProdPrice = Request.Form["pprice"];
            pm.Category = Request.Form["category"];
            ModelState.AddModelError("", pm.ProdId + " " + pm.ProdName + " " + pm.ProdManufacturer + " " +pm.ProdPrice+" "+pm.Category );
            db2.Products.Add(pm);
            int res2 = db2.SaveChanges();
            if (res2 > 0)
            {
                ModelState.AddModelError("", "New Product Inserted");
            }
            return RedirectToAction("GetProducts");
        }
        public ActionResult GetProducts()
        {
            var data = db2.Products.ToList();
            return View(data);
        }
        //Update and Delete Product
        [HttpGet]
        public ActionResult UpdateProduct(int id)
        {
            var data = db2.Products.Where(x => x.ProdId == id).SingleOrDefault();
            return View(data);
        }
        [HttpPost]
        public ActionResult UpdateProduct()
        {
            int id = Convert.ToInt32(Request.Form["pid"]);
            var olddata = db2.Products.Where(x => x.ProdId == id).SingleOrDefault();

            var pname = Request.Form["pname"];
            var pman = Request.Form["ddlmanufacturer"];
            var pprice = Request.Form["pprice"];
            var pcat = Request.Form["category"];

            olddata.ProdName = pname;
            olddata.ProdManufacturer = pman;
            olddata.ProdPrice = pprice;
            olddata.Category = pcat;
            var res = db2.SaveChanges();
            if (res >= 0)
            {
                return RedirectToAction("GetProducts");
            }
            return View();
        }
        [HttpGet]
        public ActionResult DeleteProduct(int id)
        {
            var data = db2.Products.Where(x => x.ProdId == id).SingleOrDefault();
            var delrow = db2.Products.Where(x => x.ProdId == id).SingleOrDefault();
            db2.Products.Remove(delrow);
            var res = db2.SaveChanges();
            if (res > 0)
                return RedirectToAction("GetProducts");
            return View(data);
        }
        [HttpPost]
        public ActionResult DeleteProduct()
        {
            return RedirectToAction("GetProducts");
        }

        [HttpGet]
        public ActionResult InsertOrder()
        {
            ViewData["ord"] = new SelectList(db2.Products.ToList(), "ProdId", "ProdName");
            return View();
        }
        [HttpPost]
        public ActionResult InsertOrder(OrderInfo oinfo)
        {
            oinfo.ProdId = Convert.ToInt32(Request.Form["ddlpid"]);
            //ModelState.AddModelError("", oinfo.pid+" ");
            ViewData["ord"] = new SelectList(db2.Products.ToList(), "ProdId", "ProdName"); //projid,projname from ProjectInfo.cs

            /*oinfo.ProdId = Convert.ToInt32(Request.Form["pidx"]);
            oinfo.Qty = Convert.ToInt32(Request.Form["qty"]);
            oinfo.PaymentMode = Request.Form["pmode"];
            oinfo.Status1 = Request.Form["statuss"];*/
            var data = db2.Products.Where(x => x.ProdId == oinfo.ProdId).SingleOrDefault();
            oinfo.TotalAmount = Convert.ToInt32(oinfo.Qty) * Convert.ToInt32(data.ProdPrice);


            ModelState.AddModelError("", "Product ID: " + oinfo.ProdId + " Quantity:" + oinfo.Qty + " Payment Mode:" + oinfo.PaymentMode + " Status:" + oinfo.Status1 + " Total Amount:" + oinfo.TotalAmount);
            db2.OrderInfoes.Add(oinfo);
            int res = db2.SaveChanges();
            if (res > 0)
            {
                ModelState.AddModelError("", "New Order Inserted");
            }
            
            return View();
        }
        [HttpGet]
        public ActionResult GetOrderDetails()
        {
            var data = (from p in db2.Products
                        join o in db2.OrderInfoes
                        on p.ProdId equals o.ProdId
                        select new CustomOrderDetails
                        {
                            ProdId = o.ProdId,
                            ProdName = p.ProdName,
                            OrderId = o.OrderId,
                            TotalAmount = o.TotalAmount,
                            PaymentMode = o.PaymentMode,
                            Status1 = o.Status1
                        }).ToList();
            return View(data);
        }

        [HttpGet]
        public ActionResult SelectOrderByID()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SelectOrderByID(int? id,string command)
        {
            if(command=="Display")
            {
                id = Convert.ToInt32(Request.Form["oid"]);
                var res = db2.sp_SelectProductbyId(id).SingleOrDefault();
                if(res == null)
                {
                    ModelState.AddModelError("", "Invalid Order Id");
                }
                else
                {
                    ModelState.AddModelError("", res.OrderId + " ," + res.ProdId + " , " + res.Qty+" , "+ res.TotalAmount + " , " + res.PaymentMode + " , " + res.Status1);
                }
                ViewBag.data = res;
                return View();
            }
            return View();
        }

    }
}