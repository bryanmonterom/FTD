using AdminLteMvc.BL;
using AdminLteMvc.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminLteMvc.Models;
using AdminLteMvc.Utilities;
using Microsoft.Owin.Security.Provider;

namespace AdminLteMvc.Controllers
{
    [Authorize]
    public class ReportsController : BaseController
    {

        // GET: Reports
        Business business = new Business();

        public ActionResult Requests()
        {
            var request = new RequestReportViewModel()
            {
                PostId = 0
            };
            ViewBag.IdStatus = new SelectList(Utilities.Utilities.GetStatus(), "Value", "Text");
            ViewBag.IdFlowType = new SelectList(Utilities.Utilities.GetFlowTypes(), "Value", "Text");
            ViewBag.IdDepartment = new SelectList(Utilities.Utilities.GetDepartments(), "Value", "Text");


            return View(request);
        }

        public ActionResult Generate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var results = business.RequestQueryBuilder(id, User.Identity.Name);
            //var results = business.RequestQueryBuilder(id, "cmartinez");
            if (results.IsSuccess == true)
            {
                DevExpress.XtraReports.UI.XtraReport rpt = new rptRequest();
                var filter = business.RequestQueryBuilder(id, User.Identity.Name).Message;
                //var filter = business.RequestQueryBuilder(id, "cmartinez").Message;

                rpt.FilterString = filter;
                return View("Print", rpt);
            }

            var request = new RequestReportViewModel()
            {
                PostId = (int)id
            };
            ViewBag.IdStatus = new SelectList(Utilities.Utilities.GetStatus(), "Value", "Text");
            ViewBag.IdFlowType = new SelectList(Utilities.Utilities.GetFlowTypes(), "Value", "Text");
            ViewBag.IdDepartment = new SelectList(Utilities.Utilities.GetDepartments(), "Value", "Text");
            Alert(results.Message.Replace("'", " "), NotificationType.error);
            return View("Requests", request);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Filter(DateTime? FromDate, DateTime? ToDate, int? IdStatus=0, int? IdFlowType=0, int? idDepartment=0)
        {
            if (!User.IsInRole("Administradores"))
            {
                idDepartment = business.GetEmployee(User.Identity.Name).IdDepartment;
            }
            var results = business.RequestQueryBuilder(FromDate, ToDate, IdFlowType,IdStatus, idDepartment);
                DevExpress.XtraReports.UI.XtraReport rpt = new rptRequestHistoryv2();
                var filter = results;
                rpt.FilterString = filter;
                return View("Print", rpt);
        }

        [HttpPost]
        [Authorize(Roles = "Administradores")]
        public ActionResult AuditTrail(DateTime? ToDate, DateTime? FromDate, string PerformedBy = "", string IdAction = "")
        {
            var results = business.AuditTrailQueryBuilder(ToDate,FromDate,PerformedBy, IdAction);
            DevExpress.XtraReports.UI.XtraReport rpt = new rptAuditTrail();
            var filter = results;
            rpt.FilterString = filter.Message;
            ViewBag.IdAction = new SelectList(Utilities.Utilities.GetActions(), "Value", "Text", IdAction);

            return View("Print", rpt);
        }
        [Authorize(Roles = "Administradores")]
        public ActionResult AuditTrail()
        {
            ViewBag.IdAction = new SelectList(Utilities.Utilities.GetActions(), "Value", "Text");

            return View();
        }


    }
}