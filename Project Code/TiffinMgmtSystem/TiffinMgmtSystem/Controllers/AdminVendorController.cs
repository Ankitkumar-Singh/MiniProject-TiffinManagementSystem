using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TiffinMgmtSystem.Models;
using PagedList;
using System.Web.Security;

namespace TiffinMgmtSystem.Controllers
{
    [Authorize]
    public class AdminVendorController : Controller
    {
        #region "Database"
        /// <summary>The database</summary>
        private DBContext db = new DBContext();
        #endregion

        #region "View list of vendors"
        /// <summary>Indexes this instance.</summary>
        /// <returns></returns>
        public ActionResult Index(string search, int? page)
        {
            if (Session["UserRole"].ToString() == "1" && Session["UserEmail"] != null)
            {
                var userDetails = db.UserDetails.Include(u => u.Gender).Include(u => u.UserType);
                var usertype = from s in db.UserDetails select s;
                usertype = usertype.Where(s => s.UserTypeId == 2);
                return View(usertype.Where(x => x.FirstName.StartsWith(search) || x.LastName.StartsWith(search) || x.PhoneNo.StartsWith(search) || x.Email.StartsWith(search) || search == null).ToList().ToPagedList(page ?? 1, 5));
            }
            else
            {
                Response.Redirect("<script>alert('Session logged out. Sign in again');</script>");
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("SignIn", "Auth");
            }
        }
        #endregion

        #region "View vendor details"
        /// <summary>Detailses the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (Session["UserRole"].ToString() == "1" && Session["UserEmail"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UserDetail userDetail = db.UserDetails.Find(id);
                if (userDetail == null)
                {
                    return HttpNotFound();
                }
                return View(userDetail);
            }
            else
            {
                Response.Redirect("<script>alert('Session logged out. Sign in again');</script>");
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("SignIn", "Auth");
            }
        }
        #endregion

        #region "Add a new vendor"
        /// <summary>Creates this instance.</summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            if (Session["UserRole"].ToString() == "1" && Session["UserEmail"] != null)
            {
                ViewBag.GenderId = new SelectList(db.Genders, "GenderId", "Name");
                return View();
            }
            else
            {
                Response.Redirect("<script>alert('Session logged out. Sign in again');</script>");
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("SignIn", "Auth");
            }
        }

        /// <summary>Creates the specified user detail.</summary>
        /// <param name="userDetail">The user detail.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,FirstName,LastName,GenderId,PhoneNo,Email,Password,UserTypeId,Date")] UserDetail userDetail)
        {
            if (Session["UserRole"].ToString() == "1" && Session["UserEmail"] != null)
            {
                if (ModelState.IsValid)
                {
                    userDetail.UserTypeId = 2;
                    userDetail.Date = DateTime.Now;
                    db.UserDetails.Add(userDetail);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.GenderId = new SelectList(db.Genders, "GenderId", "Name", userDetail.GenderId);
                return View(userDetail);
            }
            else
            {
                Response.Redirect("<script>alert('Session logged out. Sign in again');</script>");
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("SignIn", "Auth");
            }
        }
        #endregion

        #region "Edit vendor details"
        /// <summary>Edits the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (Session["UserRole"].ToString() == "1" && Session["UserEmail"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UserDetail userDetail = db.UserDetails.Find(id);
                if (userDetail == null)
                {
                    return HttpNotFound();
                }
                ViewBag.GenderId = new SelectList(db.Genders, "GenderId", "Name", userDetail.GenderId);
                ViewBag.UserTypeId = new SelectList(db.UserTypes, "TypeId", "Type", userDetail.UserTypeId);
                return View(userDetail);
            }
            else
            {
                Response.Redirect("<script>alert('Session logged out. Sign in again');</script>");
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("SignIn", "Auth");
            }
        }

        /// <summary>Edits the specified user detail.</summary>
        /// <param name="userDetail">The user detail.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,FirstName,LastName,GenderId,PhoneNo,Email,Password,UserTypeId,Date")] UserDetail userDetail)
        {
            if (Session["UserRole"].ToString() == "1" && Session["UserEmail"] != null)
            {
                if (ModelState.IsValid)
                {
                    userDetail.UserTypeId = 2;
                    userDetail.Date = DateTime.Now;
                    db.Entry(userDetail).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.GenderId = new SelectList(db.Genders, "GenderId", "Name", userDetail.GenderId);
                ViewBag.UserTypeId = new SelectList(db.UserTypes, "TypeId", "Type", userDetail.UserTypeId);
                return View(userDetail);
            }
            else
            {
                Response.Redirect("<script>alert('Session logged out. Sign in again');</script>");
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("SignIn", "Auth");
            }
        }
        #endregion

        #region "Delete vendor"
        /// <summary>Deletes the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            if (Session["UserRole"].ToString() == "1" && Session["UserEmail"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UserDetail userDetail = db.UserDetails.Find(id);
                if (userDetail == null)
                {
                    return HttpNotFound();
                }
                return View(userDetail);
            }
            else
            {
                Response.Redirect("<script>alert('Session logged out. Sign in again');</script>");
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("SignIn", "Auth");
            }
        }

        /// <summary>Deletes the confirmed.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserRole"].ToString() == "1" && Session["UserEmail"] != null)
            {
                UserDetail userDetail = db.UserDetails.Find(id);
                db.UserDetails.Remove(userDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                Response.Redirect("<script>alert('Session logged out. Sign in again');</script>");
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("SignIn", "Auth");
            }
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
