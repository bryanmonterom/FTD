using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminLteMvc.BL;
using AdminLteMvc.Models;
using AdminLteMvc.Utilities;
using PagedList;

namespace AdminLteMvc.Controllers
{

    [Authorize(Roles = "Administradores")]

    public class FlowStepsController : BaseController
    {
        private FtdContext db = new FtdContext();
        Business business = new Business();

        // GET: FlowSteps
        public ActionResult Index(int? page, int? IdFlowType,  string employee = "")
        {
            var flowSteps = business.FlowStepsFilter(employee, IdFlowType);
            int pageNumber = page ?? 1;
            ViewBag.IdFlowType = new SelectList(Utilities.Utilities.GetFlowTypes(), "Value", "Text");
            return View(flowSteps.OrderBy(a => a.IdFlowType).ThenBy(a => a.StepNumber).ToPagedList(pageNumber, Utilities.Utilities.pageSize));
        }

        // GET: FlowSteps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlowSteps flowSteps = db.FlowSteps.Find(id);
            if (flowSteps == null)
            {
                return HttpNotFound();
            }
            return View(flowSteps);
        }

        // GET: FlowSteps/Create
        public ActionResult Create()
        {
            ViewBag.IdAttachmentType = new SelectList(db.AttachmentType, "Id", "Name",1);
            ViewBag.IdEmployee = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text");
            ViewBag.IdFlowType = new SelectList(db.FlowType, "Id", "Name");
            ViewBag.IdGroup = new SelectList(Utilities.Utilities.GetGroups(), "Value", "Text");
            ViewBag.IdGroupToInform = new SelectList(Utilities.Utilities.GetGroups(), "Value", "Text");


            return View();
        }

        // POST: FlowSteps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdEmployee,StepNumber,StepName,IdFlowType,IsAttachmentNeed,IsMatrixNeeded,IsTransferNeed,IsSupervisorNeed,IsBuyersBlocker,IdGroup,IdAttachmentType, IdGroupToInform,SendEmailWhenCompleted,IsRestorePoint, IsBeginPointWhenSupervisor")] FlowSteps flowSteps)
        {
            if (ModelState.IsValid)
            {
                var results = business.Add(flowSteps);
                if (results.IsSuccess == true)
                {
                    Alert(results.Message, NotificationType.success);
                    Utilities.Utilities.PrepareAuditTrail("Step added: " + flowSteps.StepName, User.Identity.Name,
                        AuditTrailAction.Insert);
                }
                else
                {
                    Alert(results.Message, NotificationType.error);
                }
            }

            ViewBag.IdAttachmentType = new SelectList(db.AttachmentType, "Id", "Name", flowSteps.IdAttachmentType);
            ViewBag.IdEmployee = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text", flowSteps.IdEmployee);
            ViewBag.IdFlowType = new SelectList(db.FlowType, "Id", "Name", flowSteps.IdFlowType);
            ViewBag.IdGroup = new SelectList(Utilities.Utilities.GetGroups(), "Value", "Text", flowSteps.IdGroup);
            ViewBag.IdGroupToInform = new SelectList(Utilities.Utilities.GetGroups(), "Value", "Text", flowSteps.IdGroupToInform);

            return View(flowSteps);
        }

        // GET: FlowSteps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlowSteps flowSteps = db.FlowSteps.Find(id);
            if (flowSteps == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdAttachmentType = new SelectList(db.AttachmentType, "Id", "Name", flowSteps.IdAttachmentType);
            ViewBag.IdEmployee = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text", flowSteps.IdEmployee);
            ViewBag.IdFlowType = new SelectList(db.FlowType, "Id", "Name", flowSteps.IdFlowType);
           ViewBag.IdGroup = new SelectList(Utilities.Utilities.GetGroups(), "Value", "Text", flowSteps.IdGroup);
            ViewBag.IdGroupToInform = new SelectList(Utilities.Utilities.GetGroups(), "Value", "Text", flowSteps.IdGroupToInform);

            return View(flowSteps);
        }

        // POST: FlowSteps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdEmployee,StepNumber,StepName,IdFlowType,IsAttachmentNeed,IsMatrixNeeded,IsTransferNeed,IsSupervisorNeed,IsBuyersBlocker,IdGroup,IdAttachmentType, IdGroupToInform,SendEmailWhenCompleted,IsRestorePoint, IsBeginPointWhenSupervisor")] FlowSteps flowSteps)
        {
            if (ModelState.IsValid)
            {

                if (flowSteps.SendEmailWhenCompleted == true && flowSteps.IdGroupToInform == null)
                {
                    Alert("Debes seleccionar un grupo pa enviarle los correos de notificacion", NotificationType.error);
                    return View(flowSteps);

                }
                if (flowSteps.IsAttachmentNeed == true && flowSteps.IdAttachmentType == null)
                {
                    Alert("Debes seleccionar un tipo de adjunto", NotificationType.error);
                    return View(flowSteps);

                }
                var results = business.Update(flowSteps);
                if (results.IsSuccess == true)
                {
                    Alert(results.Message, NotificationType.success);
                    Utilities.Utilities.PrepareAuditTrail("Paso actualizado: " + flowSteps.StepName, User.Identity.Name,
                        AuditTrailAction.Update);
                }
                else
                {
                    Alert(results.Message, NotificationType.error);

                }
            }
            ViewBag.IdAttachmentType = new SelectList(db.AttachmentType, "Id", "Name", flowSteps.IdAttachmentType);
            ViewBag.IdEmployee = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text", flowSteps.IdEmployee);
            ViewBag.IdFlowType = new SelectList(db.FlowType, "Id", "Name", flowSteps.IdFlowType);
            ViewBag.IdGroup = new SelectList(Utilities.Utilities.GetGroups(), "Value", "Text", flowSteps.IdGroup);
            ViewBag.IdGroupToInform = new SelectList(Utilities.Utilities.GetGroups(), "Value", "Text", flowSteps.IdGroupToInform);

            return View(flowSteps);
        }

        // GET: FlowSteps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlowSteps flowSteps = db.FlowSteps.Find(id);
            if (flowSteps == null)
            {
                return HttpNotFound();
            }
            return View(flowSteps);
        }

        // POST: FlowSteps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FlowSteps flowSteps = db.FlowSteps.Find(id);

            try
            {
                db.FlowSteps.Remove(flowSteps);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Alert(ex.Message, NotificationType.error);
                return View("Delete", flowSteps);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //public ActionResult GetFlowTypes(int idFlowType)
        //{
        //    var steps = business.GetFlowSteps(idFlowType);
        //    return Json
        //        (new
        //        {
        //            @idStepOne = business.GetEmployee(steps.OrderBy(a=> a.StepNumber).FirstOrDefault().IdEmployee).IdDepartment, 
        //            @idLastStep = steps.OrderByDescending(a=> a.StepNumber).FirstOrDefault().Id
        //        },JsonRequestBehavior.AllowGet);
        //}
    }
}
