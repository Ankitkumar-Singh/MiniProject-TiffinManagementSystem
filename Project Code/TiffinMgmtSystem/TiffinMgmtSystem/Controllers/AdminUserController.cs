using PagedList;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using TiffinMgmtSystem.Models;

namespace TiffinMgmtSystem.Controllers
{
    [Authorize]
    public class AdminUserController : Controller
    {
        /// <summary>The database</summary>
        private DBContext db = new DBContext();

        /// <summary>Indexes the specified search.</summary>
        /// <param name="search">The search.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public ActionResult Index(string search, int? page)
        {
            if (Session["UserRole"].ToString() == "1" && Session["UserEmail"] != null)
            {
                var userDetails = db.UserDetails.Include(u => u.Gender).Include(u => u.UserType);
                var usertype = from s in db.UserDetails select s;
                usertype = usertype.Where(s => s.UserTypeId == 3);
                return View(usertype.Where(x => x.FirstName.StartsWith(search) || x.LastName.StartsWith(search) || x.PhoneNo.StartsWith(search) || x.Email.StartsWith(search) || search == null).ToList().ToPagedList(page ?? 1, 5));
            }
            else
            {
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("SignIn", "Auth");
            }
        }

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
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("SignIn", "Auth");
            }
        }

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
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("SignIn", "Auth");
            }
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
