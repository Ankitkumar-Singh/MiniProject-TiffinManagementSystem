using System.Web.Mvc;
using TiffinSystem.Models;
using System.Data.Entity;
using System.Linq;
using System;

namespace TiffinSystem.Controllers
{
    public class VendorController : Controller
    {
        private readonly DBContext db = new DBContext();

        public ActionResult OrderDetails()
        {
            var orders = db.OrderDetails.Include(e => e.UserDetail).Where(e => DateTime.Compare(e.OrderDate, DateTime.Today.Date) == 0);

            return View(orders.ToList());
        }
    }
}