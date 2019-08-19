using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using TiffinMgmtSystem.Models;

namespace TiffinMgmtSystem.Controllers
{
    public class AuthController : Controller
    {
        #region "Signin authentication"
        /// <summary>Signs the in.</summary>
        /// <returns></returns
        public ActionResult SignIn()
        {
            return View();
        }

        /// <summary>Signs the in.</summary>
        /// <param name="userDetail">The user detail.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SignIn(UserDetail userDetail)
        {
            using (var context = new DBContext())
            {
                var email = userDetail.Email;
                var passsword = userDetail.Password;

                if (context.UserDetails.Any(x => x.Email.Equals(userDetail.Email, StringComparison.Ordinal) && x.Password.Equals(userDetail.Password, StringComparison.Ordinal)))
                {
                    UserDetail user = context.UserDetails.Single(x => x.Email == userDetail.Email);

                    Session["UserEmail"] = user.Email;
                    Session["UserRole"] = user.UserTypeId;
                    FormsAuthentication.SetAuthCookie(user.Email, false);

                    if (user.UserTypeId == 1)
                    {
                        return RedirectToAction("Index", "AdminVendor");
                    }
                    else if (user.UserTypeId == 2)
                    {
                        Session["UserName"] = user.FirstName;
                        return RedirectToAction("OrderDetails", "Vendor");
                    }
                    else if (user.UserTypeId == 3)
                    {
                        Session["UserId"] = user.UserId;
                        return RedirectToAction("Create", "User");
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            ModelState.AddModelError("", "Invalid email and password");
            return View();
        }
        #endregion

        #region "Signup authentication"
        /// <summary>Signs up.</summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        /// <summary>Signs up.</summary>
        /// <param name="userDetail">The user detail.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(UserDetail userDetail)
        {
            if (ModelState.IsValid)
            {
                userDetail.UserTypeId = 3;
                userDetail.Date = DateTime.Now;
                using (var context = new DBContext())
                {
                    context.UserDetails.Add(userDetail);
                    context.SaveChanges();
                    return RedirectToAction("SignIn");
                }
            }
            return View();
        }
        #endregion

        #region "Sign out"
        /// <summary>Represents an event that is raised when the sign-out operation is complete.</summary>
        /// <returns></returns>
        public ActionResult SignOut()
        {
            Session.Clear();
            return RedirectToAction("SignIn");
        }
        #endregion

        #region "Error"
        /// <summary>Errors this instance.</summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            return View();
        }
        #endregion
    }
}