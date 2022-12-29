using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminLteMvc.Models;

namespace AdminLteMvc.Controllers
{
    //[Authorize(Roles = "Administradores")]
    public class AttachmentTypesController : Controller
    {
        private FtdContext db = new FtdContext();

        // GET: AttachmentTypes
        public ActionResult Index()
        {
            return View(db.AttachmentType.ToList());
        }

        // GET: AttachmentTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttachmentType attachmentType = db.AttachmentType.Find(id);
            if (attachmentType == null)
            {
                return HttpNotFound();
            }
            return View(attachmentType);
        }

        // GET: AttachmentTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AttachmentTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] AttachmentType attachmentType)
        {
            if (ModelState.IsValid)
            {
                db.AttachmentType.Add(attachmentType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(attachmentType);
        }

        // GET: AttachmentTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttachmentType attachmentType = db.AttachmentType.Find(id);
            if (attachmentType == null)
            {
                return HttpNotFound();
            }
            return View(attachmentType);
        }

        // POST: AttachmentTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] AttachmentType attachmentType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attachmentType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(attachmentType);
        }

        // GET: AttachmentTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttachmentType attachmentType = db.AttachmentType.Find(id);
            if (attachmentType == null)
            {
                return HttpNotFound();
            }
            return View(attachmentType);
        }

        // POST: AttachmentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AttachmentType attachmentType = db.AttachmentType.Find(id);
            db.AttachmentType.Remove(attachmentType);
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
