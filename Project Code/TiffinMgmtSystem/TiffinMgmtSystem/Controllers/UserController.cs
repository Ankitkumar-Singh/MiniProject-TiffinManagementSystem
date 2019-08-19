using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TiffinMgmtSystem.Models;

namespace TiffinSystem.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private DBContext db = new DBContext();

        // GET: OrderDetails/Details/5
        /// <summary>Detailses the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Details(int ? id)
        {
            var orderDetails = db.OrderDetails.Include(o => o.Extra).Include(o => o.TiffinDetail).Include(o => o.UserDetail);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<OrderDetail> orderDetail = db.OrderDetails.Where(u => u.UserId == id).OrderBy(u => u.OrderDate).ToList();
            return View(orderDetail);
        }

        /// <summary>Detailses the specified from.</summary>
        /// <param name="From">From.</param>
        /// <param name="To">To.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Details(DateTime? From, DateTime? To)
        {
            var orders = db.OrderDetails.AsQueryable();

            if (From == null && To == null)
            {
                return View(orders.Include(e => e.UserDetail).ToList());
            }
            else
            {
                orders = orders.Where(e => e.OrderDate >= From && e.OrderDate <= To);

                return View(orders.ToList());
            }
        }
        // GET: OrderDetails/Create
        /// <summary>Creates this instance.</summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var orderDetails = db.OrderDetails.Include(o => o.Extra).Include(o => o.TiffinDetail).Include(o => o.UserDetail);
            ViewBag.VendorName = new SelectList(db.UserDetails.Where(u => u.UserTypeId == 2),"FirstName","FirstName");
            ViewBag.ExtraId = new SelectList(db.Extras, "ExtraId", "ExtraName");
            ViewBag.TiffinTypeId = new SelectList(db.TiffinDetails, "Id", "Type");
            return View();
        }

        /// <summary>Creates the specified order detail.</summary>
        /// <param name="orderDetail">The order detail.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,TiffinTypeId,VendorName,ExtraId,Total,Count")] OrderDetail orderDetail)
        {
            var orderDetails = db.OrderDetails.Include(o => o.Extra).Include(o => o.TiffinDetail).Include(o => o.UserDetail);
            if (ModelState.IsValid)
            {
                var userEmail = Session["UserEmail"].ToString();
                UserDetail user = db.UserDetails.Single(x => x.Email == userEmail);
                TiffinDetail tiffin = db.TiffinDetails.Single(t => t.Id == orderDetail.TiffinTypeId);
                orderDetail.OrderDate = DateTime.Now;
                orderDetail.UserId = user.UserId;
                orderDetail.Total = (tiffin.Cost + (Convert.ToInt16(orderDetail.Count * 5)));
                db.OrderDetails.Add(orderDetail);
                db.SaveChanges();
                return RedirectToAction("Details",new { id=user.UserId });
            }
            ViewBag.VendorName = new SelectList(db.UserDetails.Where(u => u.UserTypeId == 2), "FirstName", "FirstName", orderDetail.VendorName);
            //ViewBag.VendorName = new SelectList(db.UserDetails, "UserId", "FirstName", orderDetail.VendorName);
            ViewBag.ExtraId = new SelectList(db.Extras, "ExtraId", "ExtraName", orderDetail.ExtraId);
            ViewBag.TiffinTypeId = new SelectList(db.TiffinDetails, "Id", "Type", orderDetail.TiffinTypeId);
            return View(orderDetail);
        }


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
    }
}
