using System.Linq;
using System.Web.Mvc;
using TiffinSystem.Models;

namespace TiffinSystem.Controllers
{
    public class AuthController : Controller
    {
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(UserDetail userDetail)
        {
            if (ModelState.IsValid)
            {
                using (var context = new DBContext())
                {
                    if (context.UserDetails.Any(x => x.Email == userDetail.Email && x.Password == userDetail.Password))
                    {
                        UserDetail user = context.UserDetails.Single(x => x.Email == userDetail.Email);
                        if (user.UserTypeId == 1)
                        {
                            Session["UserProfile"] = user.Email;
                            return RedirectToAction("Index", "Admin");
                        }
                        else if (user.UserTypeId == 2)
                        {
                            Session["UserProfile"] = user.Email;
                            return RedirectToAction("Index", "Vendor");
                        }
                        else
                        {
                            Session["UserProfile"] = user.Email;
                            return RedirectToAction("Index", "User");
                        }
                    }
                }
            }
            ModelState.AddModelError("", "Invalid username and password");
            return View();
        }
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(UserDetail userDetail)
        {
            if (ModelState.IsValid)
            {
                userDetail.UserTypeId = 3;
                userDetail.Date = System.DateTime.Now;
                using (var context = new DBContext())
                {
                    context.UserDetails.Add(userDetail);
                    context.SaveChanges();
                    return RedirectToAction("SignIn");
                }
            }
            return View();
        }
    }
}