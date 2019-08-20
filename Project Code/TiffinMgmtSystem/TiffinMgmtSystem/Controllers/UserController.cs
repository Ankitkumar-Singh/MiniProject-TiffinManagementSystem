using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TiffinMgmtSystem.Models;
using PagedList;
using System.Web.Security;

namespace TiffinSystem.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        #region "Database"
        private DBContext db = new DBContext();
        #endregion

        #region "Users order details"
        /// <summary>Detailses the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Details(int? id, int? page)
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "3" && Convert.ToInt32(Session["UserId"]) == id)
            {
                var orderDetails = db.OrderDetails.Include(o => o.Extra).Include(o => o.TiffinDetail).Include(o => o.UserDetail);
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                List<OrderDetail> orderDetail = db.OrderDetails.Where(u => u.UserId == id).OrderByDescending(u => u.OrderDate).ToList();
                return View(orderDetail.ToPagedList(page ?? 1, 5));
            }
            Response.Write("<script>alert('Session logged out. Sign in again');</script>");
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("SignIn", "Auth");
        }

        /// <summary>Detailses the specified from.</summary>
        /// <param name="From">From.</param>
        /// <param name="To">To.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Details(DateTime? From, DateTime? To, int? page)
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "3")
            {
                var orders = db.OrderDetails.AsQueryable();
                var id = Convert.ToInt32(Session["UserId"]);
                if (From == null && To == null)
                {
                    return View(orders.Include(e => e.UserDetail).Where(x => x.UserId == id).OrderByDescending(u => u.OrderDate).ToList().ToPagedList(page ?? 1, 5));
                }
                else
                {
                    orders = orders.Where(e => e.OrderDate >= From && e.OrderDate <= To && e.UserId == id);
                    return View(orders.ToList().ToPagedList(page ?? 1, 5));
                }
            }
            Response.Write("<script>alert('Session logged out. Sign in again');</script>");
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("SignIn", "Auth");
        }
        #endregion

        #region "Place order"
        /// <summary>Creates this instance.</summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "3")
            {
                var orderDetails = db.OrderDetails.Include(o => o.Extra).Include(o => o.TiffinDetail).Include(o => o.UserDetail);
                ViewBag.VendorName = new SelectList(db.UserDetails.Where(u => u.UserTypeId == 2), "FirstName", "FirstName");
                ViewBag.ExtraId = new SelectList(db.Extras, "ExtraId", "ExtraName");
                ViewBag.TiffinTypeId = new SelectList(db.TiffinDetails, "Id", "Type");
                return View();
            }
            Response.Write("<script>alert('Session logged out. Sign in again');</script>");
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("SignIn", "Auth");
        }

        /// <summary>Creates the specified order detail.</summary>
        /// <param name="orderDetail">The order detail.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,TiffinTypeId,VendorName,ExtraId,Total,Count")] OrderDetail orderDetail)
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "3")
            {
                var orderDetails = db.OrderDetails.Include(o => o.Extra).Include(o => o.TiffinDetail).Include(o => o.UserDetail);
                if (ModelState.IsValid)
                {
                    TiffinDetail tiffin = db.TiffinDetails.Single(t => t.Id == orderDetail.TiffinTypeId);
                    orderDetail.OrderDate = DateTime.Now;
                    orderDetail.UserId = Convert.ToInt32(Session["UserId"]);
                    orderDetail.Total = (tiffin.Cost + (Convert.ToInt16(orderDetail.Count * 5)));
                    db.OrderDetails.Add(orderDetail);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = Convert.ToInt32(Session["UserId"]) });
                }
                ViewBag.VendorName = new SelectList(db.UserDetails.Where(u => u.UserTypeId == 2), "FirstName", "FirstName", orderDetail.VendorName);
                ViewBag.ExtraId = new SelectList(db.Extras, "ExtraId", "ExtraName", orderDetail.ExtraId);
                ViewBag.TiffinTypeId = new SelectList(db.TiffinDetails, "Id", "Type", orderDetail.TiffinTypeId);
                return View(orderDetail);
            }
            Response.Write("<script>alert('Session logged out. Sign in again');</script>");
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("SignIn", "Auth");
        }
        #endregion

        #region "Dispose"
        /// <summary>Releases unmanaged resources and optionally releases managed resources.</summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
