using System;
using System.Linq;
using System.Web.Mvc;
using TiffinMgmtSystem.Models;
using System.Data.Entity;
using PagedList;
using System.Web.Security;

namespace TiffinSystem.Controllers
{
    [Authorize]
    public class VendorController : Controller
    {
        /// <summary>The database</summary>
        private readonly DBContext db = new DBContext();

        /// <summary>Orders the details.</summary>
        public ActionResult OrderDetails(string search, int? page)
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "2")
            {
                var vendorName = Session["UserName"].ToString();
                var orders = db.OrderDetails.Where(u => u.VendorName == vendorName && u.OrderDate == DateTime.Today);
                ViewBag.OrderCount = orders.Count();
                return View(orders.Where(x => x.UserDetail.FirstName.StartsWith(search) || x.UserDetail.LastName.StartsWith(search) || search == null).ToList().ToPagedList(page ?? 1, 5));
            }
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("SignIn", "Auth");
        }

        /// <summary>Reports this instance.</summary>
        [HttpGet]
        public ActionResult Report(string search, int? page)
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "2")
            {
                var vendorName = Session["UserName"].ToString();
                var orders = db.OrderDetails.Include(e => e.UserDetail).Where(e=> e.VendorName == vendorName);
                return View(orders.Where(x => x.UserDetail.FirstName.StartsWith(search) || x.UserDetail.LastName.StartsWith(search) || search == null).ToList().ToPagedList(page ?? 1, 5));
            }
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("SignIn", "Auth");
        }

        /// <summary>Reports the specified start date.</summary>
        /// <param name="StartDate">The start date.</param>
        /// <param name="EndDate">The end date.</param>
        [HttpPost]
        public ActionResult Report(DateTime? StartDate, DateTime? EndDate, int? page)
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "2")
            {
                var orders = db.OrderDetails.AsQueryable();
                var vendorName = Session["UserName"].ToString();

                if (StartDate == null && EndDate == null)
                {
                    return View(orders.Include(e => e.UserDetail).Where(x => x.VendorName == vendorName).ToList().ToPagedList(page ?? 1, 5));
                }
                else
                {
                    orders = orders.Where(e => e.OrderDate >= StartDate && e.OrderDate <= EndDate && e.VendorName == vendorName);
                    return View(orders.ToList().ToPagedList(page ?? 1, 5));
                }
            }
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("SignIn", "Auth");
        }
    }
}