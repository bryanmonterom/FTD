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
using PagedList;


namespace AdminLteMvc.Controllers
{
    //[Authorize(Roles ="Administradores")]
    public class AuditTrailsController : Controller
    {
        private FtdContext db = new FtdContext();
        Business business = new Business();

        // GET: AuditTrails
        public ActionResult Index(int? page, string performedby = "", string IdAction = "")
        {
            int pageNumber = page ?? 1;
            var auditrail = business.AuditTrailsFilter(performedby, IdAction);
            ViewBag.IdAction = new SelectList(Utilities.Utilities.GetActions(), "Value", "Text");
            return View(auditrail.OrderBy(a=> a.Date).ToList().ToPagedList(pageNumber, Utilities.Utilities.pageSize));
        }

        // GET: AuditTrails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditTrail auditTrail = db.AuditTrail.Find(id);
            if (auditTrail == null)
            {
                return HttpNotFound();
            }
            return View(auditTrail);
        }

        // GET: AuditTrails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuditTrails/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PerformedBy,Date,Action,ActionMessage")] AuditTrail auditTrail)
        {
            if (ModelState.IsValid)
            {
                db.AuditTrail.Add(auditTrail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(auditTrail);
        }

        // GET: AuditTrails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditTrail auditTrail = db.AuditTrail.Find(id);
            if (auditTrail == null)
            {
                return HttpNotFound();
            }
            return View(auditTrail);
        }

        // POST: AuditTrails/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PerformedBy,Date,Action,ActionMessage")] AuditTrail auditTrail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(auditTrail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(auditTrail);
        }

        // GET: AuditTrails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditTrail auditTrail = db.AuditTrail.Find(id);
            if (auditTrail == null)
            {
                return HttpNotFound();
            }
            return View(auditTrail);
        }

        // POST: AuditTrails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AuditTrail auditTrail = db.AuditTrail.Find(id);
            db.AuditTrail.Remove(auditTrail);
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
