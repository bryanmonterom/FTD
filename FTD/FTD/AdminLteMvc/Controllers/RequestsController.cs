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
using Microsoft.Ajax.Utilities;
using PagedList;

namespace AdminLteMvc.Controllers
{
    [Authorize]
    public class RequestController : BaseController
    {
        private FtdContext db = new FtdContext();
        Business business = new Business() ;

        // GET: Request
        public ActionResult Index(int? idStatus, int? page,string identifier="", string requestor="")
        {
            int pageNumber = page ?? 1;
            var request = new List<Request>();
            if (User.IsInRole("Administradores"))
                request = business.RequestFilter(idStatus, identifier, requestor);
            else
                request = business.RequestFilter(idStatus, identifier, requestor, User.Identity.Name);
            ViewBag.IdStatus = new SelectList(Utilities.Utilities.GetStatus(), "Value", "Text");
            return View(request.OrderBy(a => a.Id).ToPagedList(pageNumber, Utilities.Utilities.pageSize));

        }



        // GET: Request/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Request.Include(a=> a.FlowSteps).Include(a=> a.From).Include(a=> a.To).Include(a=> a.Status).Include(a=> a.FlowType).Include(a=> a.Requestor).FirstOrDefault(a=> a.Id == id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // GET: Request/Create
        public ActionResult Create()
        {
            
            ViewBag.IdFlowType = new SelectList(Utilities.Utilities.GetFlowTypes(), "Value", "Text");
            ViewBag.IdRequestor = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text", business.GetEmployee(User.Identity.Name).Id);

            //ViewBag.IdRequestor = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text", business.GetEmployee(User.Identity.Name).Id);
            //ViewBag.IdRequestor = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text", 1);
            //ViewBag.IdTo = new SelectList(Utilities.Utilities.GetDepartments(), "Value", "Text");
            //ViewBag.IdFrom = new SelectList(Utilities.Utilities.GetDepartments(), "Value", "Text");

            return View();
        }

        // POST: Request/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdTo,IdFrom,Identifier,IdRequestType,IdRequestor,Date,LastModified,Subject,Message,IdStatus,IdFlowType,IdFlowSteps,IsCompleted")] Request request)
        {
            ViewBag.IdFlowType = new SelectList(db.FlowType, "Id", "Name", request.IdFlowType);
            ViewBag.IdRequestor = new SelectList(db.Employee, "Id", "Identification", request.IdRequestor);
            //ViewBag.IdTo = new SelectList(Utilities.Utilities.GetDepartments(), "Value", "Text", request.IdTo);
            //ViewBag.IdFrom = new SelectList(Utilities.Utilities.GetDepartments(), "Value", "Text", request.IdFrom);

            if (ModelState.IsValid)
            {
                if (request.Message.IsNullOrWhiteSpace())
                {
                    Alert("El campo mensaje no puede estar vacio.",
                        NotificationType.error);
                    return View(request);
                }
                if (request.Subject.IsNullOrWhiteSpace())
                {
                    Alert("El campo asunto no puede estar vacio.",
                        NotificationType.error);
                    return View(request);
                    
                }
                var results =  business.Add(request);
                if (results.IsSuccess == true)
                {
                    return RedirectToAction("Edit", new {@id = results.Parameter});
                }

                Alert(results.Message, NotificationType.error);
            }

            ViewBag.IdFlowType = new SelectList(db.FlowType, "Id", "Name", request.IdFlowType);
            ViewBag.IdRequestor = new SelectList(db.Employee, "Id", "Identification", request.IdRequestor);
            //ViewBag.IdTo = new SelectList(Utilities.Utilities.GetDepartments(), "Value", "Text", request.IdTo);
            //ViewBag.IdFrom = new SelectList(Utilities.Utilities.GetDepartments(), "Value", "Text", request.IdFrom);

            return View(request);
        }

        // GET: Request/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Request.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            //bool IsSecretary = business.IsSecretary(User.Identity.Name);
            bool IsSecretary = !User.IsInRole("Administradores");

            var completition = business.IsCompleted((int)id);
            if (request.IdStatus == (int)AdminLteMvc.Utilities.Status.Blocked && IsSecretary)
            {
                ModelState.AddModelError("", "Esta solicitud se encuentra bloqueada para la edicion.");
            }
            if (request.IdStatus == (int)AdminLteMvc.Utilities.Status.OnlyAttachments && IsSecretary)
            {
                ModelState.AddModelError("", "Esta solicitud se encuentra bloqueada, solo puedes agregar adjuntos.");
            }
            if (completition.IsSuccess == true)
            {
                ModelState.AddModelError("", "Esta solicitud esta marcada como completada, no puedes editarla.");
            }
            ViewBag.IdFlowSteps = new SelectList(db.FlowSteps.ToList(), "Id", "StepName", request.IdFlowSteps);
            ViewBag.IdFlowType = new SelectList(db.FlowType.ToList(), "Id", "Name", request.IdFlowType);
            ViewBag.IdRequestor = new SelectList(db.Employee.ToList(), "Id", "Identification", request.IdRequestor);
            ViewBag.IdStatus = new SelectList(db.Status.ToList(), "Id", "Name", request.IdStatus);
            ViewBag.IdTo = new SelectList(db.Department.ToList(), "Id", "Name", request.IdTo);
            ViewBag.IdFrom = new SelectList(Utilities.Utilities.GetDepartments(), "Value", "Text", request.IdFrom);

            return View(request);
        }

        public ActionResult GetFlowHistory(int id)
        {
            var a = business.GetRequestHistoryForFlow(id);
            return View("_Flow", a);
        }

        // POST: Request/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdTo,IdFrom,Identifier,IdRequestType,IdRequestor,Date,LastModified,Subject,Message,IdStatus,IdFlowType,IdFlowSteps,IsCompleted")] Request request)
        {
            ViewBag.IdFlowSteps = new SelectList(db.FlowSteps, "Id", "StepName", request.IdFlowSteps);
            ViewBag.IdFlowType = new SelectList(db.FlowType, "Id", "Name", request.IdFlowType);
            ViewBag.IdRequestor = new SelectList(db.Employee, "Id", "Identification", request.IdRequestor);
            ViewBag.IdStatus = new SelectList(db.Status, "Id", "Name", request.IdStatus);
            ViewBag.IdTo = new SelectList(db.Department, "Id", "Name", request.IdTo);
            ViewBag.IdFrom = new SelectList(Utilities.Utilities.GetDepartments(), "Value", "Text", request.IdFrom);

            if (ModelState.IsValid)
            {
                //bool IsSecretary = business.IsSecretary(User.Identity.Name);
                if (request.Message.IsNullOrWhiteSpace())
                {
                    Alert("El campo mensaje no puede estar vacio.",
                        NotificationType.error);
                    return View(request);
                }
                if (request.Subject.IsNullOrWhiteSpace())
                {
                    Alert("El campo asunto no puede estar vacio.",
                        NotificationType.error);
                    return View(request);

                }
                bool IsSecretary = !User.IsInRole("Administradores");

                if (request.IdStatus == (int)AdminLteMvc.Utilities.Status.Blocked && IsSecretary)
                {
                    ModelState.AddModelError("", "Esta solicitud se encuentra bloqueada para la edicion.");
                    Alert("Esta solicitud se encuentra bloqueada para la edicion por secretarias.",
                        NotificationType.error);
                    return View(request);

                }
                if (request.IdStatus == (int)AdminLteMvc.Utilities.Status.OnlyAttachments && IsSecretary)
                {
                    ModelState.AddModelError("", "Esta solicitud se encuentra bloqueada, solo puedes agregar adjuntos.");
                    Alert("Esta solicitud se encuentra bloqueada, solo puedes agregar adjuntos.",
                        NotificationType.error);
                    return View(request);

                }
                if (request.IsCompleted == true)
                {
                    ModelState.AddModelError("", "Esta solicitud esta marcada como completada, no puedes editarla.");
                    Alert("Esta solicitud esta marcada como completada, no puedes editarla.",
                        NotificationType.error);
                    return View(request);
                }
                try
                {
                    db.Entry(request).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Alert(e.Message, NotificationType.error);
                }


                return RedirectToAction("Index");
            }
        
            return View(request);
        }

        // GET: Request/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Request.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Request/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Request request = db.Request.Find(id);
            db.Request.Remove(request);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
