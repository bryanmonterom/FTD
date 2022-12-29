using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminLteMvc.BL;

namespace AdminLteMvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        Business business = new Business();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetPendingRequest(string username)
        {
            var qty = business.GetPendingRequests(username);
            return Json(new { quantity = qty }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGroupPendingRequests(string username)
        {
            var qty = business.GetGroupPendingRequests(username);
            return Json(new { quantity = qty }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDashboardViewModel(string username)
        {
            var dashboard = business.GetDashboardViewModel(username);
            return Json(dashboard, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTotal(string username)
        {
            var qty = business.GetTotal(username);
            return Json(new { quantity = qty }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetEmployee()
        {
            var query = business.GetEmployee(User.Identity.Name);
            return Json(new {IdDepartment = query.IdDepartment }, JsonRequestBehavior.AllowGet);
        }
    }
}
