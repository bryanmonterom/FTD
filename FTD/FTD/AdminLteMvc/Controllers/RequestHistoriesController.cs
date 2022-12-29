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
    [Authorize]
    public class RequestHistoriesController : BaseController
    {
        private FtdContext db = new FtdContext();
        Business business = new Business();
        // GET: RequestHistories
        
        //public ActionResult Index(int? page, int? approvals)
        //{
        //    List<RequestHistory> postHistory = new List<RequestHistory>();
        //    if (User.IsInRole("Administrators"))
        //    {
        //        postHistory = business.GetPostHistoryByEmployee(null);
        //        ViewBag.IsAdmin = true;
        //    }
        //    else
        //    {
        //        approvals = approvals == null ? 0 : approvals;
        //        postHistory = business.GetPostHistoryByEmployee(approvals, (int)AdminLteMvc.Utilities.Status.Pending, "", User.Identity.Name);
        //    }
        //    int pageNumber = page ?? 1;
        //    ViewBag.IdStatus = new SelectList(Utilities.Utilities.GetStatus(), "Value", "Text", 1);
        //    ViewBag.Approvals = new SelectList(Utilities.Utilities.Approvals(), "Value", "Text", 1);
        //    return View(postHistory.OrderBy(a => a.DateApproval).ThenByDescending(a => a.DateApproval).ToPagedList(pageNumber, Utilities.Utilities.pageSize));
        //}
        public ActionResult Index(int? page, int? Approvals, int? IdStatus, string request="", string employee = "")
        {
            List<RequestHistory> postHistory = new List<RequestHistory>();
            Approvals = Approvals ?? 1;
            if (User.IsInRole("Administradores"))
            {
                postHistory = business.GetPostHistoryByEmployee(IdStatus, employee, request);
                ViewBag.IsAdmin = true;
            }
            else
            {
                //postHistory = business.GetPostHistoryByEmployee(Approvals, IdStatus, request, "bmontero");
                postHistory = business.GetPostHistoryByEmployee(Approvals, IdStatus, request, User.Identity.Name);

                //postHistory = business.GetPostHistoryByEmployee(approvals, IdStatus, request, User.Identity.Name);

            }
            ViewBag.IsAdmin = false;
            int pageNumber = page ?? 1;
            ViewBag.IdStatus = new SelectList(Utilities.Utilities.GetStatus(), "Value", "Text");
            ViewBag.Approvals = new SelectList(Utilities.Utilities.Approvals(), "Value", "Text");
            return View(postHistory.OrderBy(a => a.Status.Name).ThenByDescending(a => a.DateApproval).ToPagedList(pageNumber, Utilities.Utilities.pageSize));
        }



        public ActionResult EndFlow(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var request = db.Request.FirstOrDefault(a => a.Id == id);
            var employee = business.GetEmployee(User.Identity.Name);
            var history = new RequestHistory()
            {
             DateApproval   =  DateTime.Now,
             IdEmployee = employee.Id,
             IdStatus = (int)Utilities.Status.Canceled,
             IdRequest = (int)id,
             IdFlowSteps = request.IdFlowSteps
            };

            ViewBag.IdEmployee = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text", employee.Id);
            ViewBag.IdRequest = new SelectList(db.Request, "Id", "Identifier",id);
            ViewBag.IdStatus = new SelectList(Utilities.Utilities.GetStatus(), "Value", "Text", (int)Utilities.Status.Canceled);
            ViewBag.IdRejectReasons = new SelectList(Utilities.Utilities.GetRejectReasons(), "Value", "Text");
         
                ViewBag.IsRejected = false;

            return View("Edit", history);
        }
        [HttpPost]
        public ActionResult EndFlow([Bind(Include = "Id,IdRequest,DateApproval,IdEmployee,Comments,IdStatus,IdFlowSteps,IdRejectReasons")] RequestHistory request)
        {

            var results = business.EndRequest(request, User.Identity.Name);
            if (results.IsSuccess == false)
                Alert(results.Message, NotificationType.error);
            else
                Alert(results.Message, NotificationType.success);


            ViewBag.IsRejected = false;
            ViewBag.IdEmployee = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text", request.IdEmployee);
            ViewBag.IdRequest = new SelectList(db.Request, "Id", "Identifier", request.IdRequest);
            ViewBag.IdStatus = new SelectList(Utilities.Utilities.GetStatus(), "Value", "Text", (int)Utilities.Status.Canceled);
            ViewBag.IdRejectReasons = new SelectList(Utilities.Utilities.GetRejectReasons(), "Value", "Text");
            return View("Edit", request);
        }










        // GET: RequestHistories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestHistory requestHistory = db.RequestHistory.Find(id);
            if (requestHistory == null)
            {
                return HttpNotFound();
            }
            return View(requestHistory);
        }

        // GET: RequestHistories/Create
        public ActionResult Create()
        {
            ViewBag.IdEmployee = new SelectList(db.Employee, "Id", "Identification");
            ViewBag.IdFlowSteps = new SelectList(db.FlowSteps, "Id", "StepName");
            ViewBag.IdRequest = new SelectList(db.Request, "Id", "Identifier");
            ViewBag.IdStatus = new SelectList(db.Status, "Id", "Name");

            return View();
        }

        // POST: RequestHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,IdRequest,DateApproval,IdEmployee,Comments,IdStatus,IdFlowSteps,IdRejectReasons")] RequestHistory requestHistory)
        {
            if (ModelState.IsValid)
            {
                db.RequestHistory.Add(requestHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdEmployee = new SelectList(db.Employee, "Id", "Identification", requestHistory.IdEmployee);
            ViewBag.IdFlowSteps = new SelectList(db.FlowSteps, "Id", "StepName", requestHistory.IdFlowSteps);
            ViewBag.IdRequest = new SelectList(db.Request, "Id", "Identifier", requestHistory.IdRequest);
            ViewBag.IdStatus = new SelectList(Utilities.Utilities.GetStatus(), "Id", "Name", requestHistory.IdStatus);
            ViewBag.IdRejectReasons = new SelectList(Utilities.Utilities.GetRejectReasons(), "Value", "Text", requestHistory.IdRejectReasons);
            ViewBag.IdRejectReasons = new SelectList(Utilities.Utilities.GetStatus(), "Value", "Text", requestHistory.IdRejectReasons);


            return View(requestHistory);
        }

        // GET: RequestHistories/Edit/5
        public ActionResult Edit(int? id, int status)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestHistory requestHistory = db.RequestHistory.Include(a => a.Request).FirstOrDefault(a => a.Id == id);
            //var employee = business.GetEmployee(User.Identity.Name);
            var employee = business.GetEmployee(User.Identity.Name);

            ViewBag.IdEmployee = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text", employee.Id);
            //ViewBag.IdEmployee = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text", 23);
            ViewBag.IdRequest = new SelectList(db.Request, "Id", "Identifier", requestHistory.IdRequest);
            ViewBag.IdStatus = new SelectList(Utilities.Utilities.GetStatus(), "Value", "Text", status);
            ViewBag.IdRejectReasons = new SelectList(Utilities.Utilities.GetRejectReasons(), "Value", "Text", requestHistory.IdRejectReasons);
            if (status == (int)Utilities.Status.Rejected)
            {
                ViewBag.IsRejected = true;
            }
            else
            {
                ViewBag.IsRejected = false;

            }
            if (requestHistory.Request.IsCompleted == true)
            {
                ModelState.AddModelError("", "Este paso ya ha sido aprobado, no puedes editarlo.");
                return View(requestHistory);

            }

            if (requestHistory == null)
            {
                return HttpNotFound();
            }
            return View(requestHistory);
        }

        // POST: RequestHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,IdRequest,DateApproval,IdEmployee,Comments,IdStatus,IdFlowSteps, IdRejectReasons")] RequestHistory requestHistory)
        {
            if (requestHistory.IdStatus == (int)Utilities.Status.Rejected)
            {
                ViewBag.IsRejected = true;
            }
            else
            {
                ViewBag.IsRejected = false;

            }
            if (ModelState.IsValid)
            {
                if (!User.IsInRole("Administradores"))
                {
                    var access = business.AccessOrNot(User.Identity.Name, requestHistory.Id);
                    //var access = business.AccessOrNot("jlopez", requestHistory.Id);

                    if (!access.IsSuccess)
                    {
                        return View("_NoAccess");
                    }
                }

                ViewBag.IdEmployee = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text", requestHistory.IdEmployee);
                ViewBag.IdRequest = new SelectList(db.Request, "Id", "Identifier", requestHistory.IdRequest);
                ViewBag.IdRejectReasons = new SelectList(Utilities.Utilities.GetRejectReasons(), "Value", "Text", requestHistory.IdRejectReasons);
                ViewBag.IdStatus = new SelectList(Utilities.Utilities.GetStatus(), "Value", "Text", requestHistory.IdStatus);
                bool IsFromShopping = User.IsInRole("Secretary");
                if (requestHistory.IdStatus == (int)Utilities.Status.Rejected)
                {
                    ViewBag.IsRejected = true;
                }
                else
                {
                    ViewBag.IsRejected = false;

                }
                if (requestHistory.IdRejectReasons == null && requestHistory.IdStatus == (int)Utilities.Status.Rejected)
                {
                    Alert("Debes seleccionar la razon por la cual estas rechazando este elemento", NotificationType.error);
                    return View(requestHistory);

                }
                var results = business.IsApproved(requestHistory.Id);
                if (results.IsSuccess == false)
                {
                    //var employee = business.GetEmployee("bmontero");
                    var employee = business.GetEmployee(User.Identity.Name);


                    requestHistory.IdEmployee = employee.Id;
                    var a = business.SetApproval(requestHistory);
                    if (a.IsSuccess)
                    {
                        Alert(a.Message, NotificationType.success);
                        Utilities.Utilities.PrepareAuditTrail("Solicitud aprobada | Estado: " + requestHistory.IdStatus + " | Solicitud: " + requestHistory.IdRequest + " | Comentarios: " + requestHistory.Comments, User.Identity.Name,
                            AuditTrailAction.Update);
                    }
                    else
                    {
                        Alert(a.Message.Replace("'", " "), NotificationType.error);
                    }
                }
                else
                {
                    Alert(results.Message.Replace("'", " "), NotificationType.error);

                }
            }

            return View(requestHistory);
        }

        // GET: RequestHistories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestHistory requestHistory = db.RequestHistory.Find(id);
            if (requestHistory == null)
            {
                return HttpNotFound();
            }
            return View(requestHistory);
        }

        // POST: RequestHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RequestHistory requestHistory = db.RequestHistory.Find(id);
            db.RequestHistory.Remove(requestHistory);
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
