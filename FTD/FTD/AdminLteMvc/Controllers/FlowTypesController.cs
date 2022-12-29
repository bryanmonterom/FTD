using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminLteMvc.Models;
using AdminLteMvc.Utilities;

namespace AdminLteMvc.Controllers
{
    [Authorize(Roles = "Administradores")]
    public class FlowTypesController :BaseController
    {
        private FtdContext db = new FtdContext();

        // GET: FlowTypes

        public ActionResult Index()
        {
            return View(db.FlowType.OrderBy(a=> a.Name).ToList());
        }

        // GET: FlowTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlowType flowType = db.FlowType.Find(id);
            if (flowType == null)
            {
                return HttpNotFound();
            }
            return View(flowType);
        }

        // GET: FlowTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FlowTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] FlowType flowType)
        {
            if (ModelState.IsValid)
            {
                db.FlowType.Add(flowType);
                db.SaveChanges();
                Utilities.Utilities.PrepareAuditTrail("Flujo creado: " + flowType.Name, User.Identity.Name,
                    AuditTrailAction.Insert);
                return RedirectToAction("Index");
            }

            return View(flowType);
        }

        // GET: FlowTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlowType flowType = db.FlowType.Find(id);
            if (flowType == null)
            {
                return HttpNotFound();
            }
            return View(flowType);
        }

        // POST: FlowTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] FlowType flowType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flowType).State = EntityState.Modified;
                db.SaveChanges();
                Utilities.Utilities.PrepareAuditTrail("Flujo modificado: " + flowType.Name + "Id: "+flowType.Id, User.Identity.Name,
                    AuditTrailAction.Update);
                return RedirectToAction("Index");
            }
            return View(flowType);
        }

        // GET: FlowTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlowType flowType = db.FlowType.Find(id);
            if (flowType == null)
            {
                return HttpNotFound();
            }
            return View(flowType);
        }

        // POST: FlowTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FlowType flowType = db.FlowType.Find(id);
            try
            {
                db.FlowType.Remove(flowType);
                db.SaveChanges();
                Utilities.Utilities.PrepareAuditTrail("Flujo removido: " + flowType.Name + "Id: " + flowType.Id,
                    User.Identity.Name,
                    AuditTrailAction.Delete);
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                Alert(ex.Message, NotificationType.error);
                return View("Delete", flowType);
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
    }
}
