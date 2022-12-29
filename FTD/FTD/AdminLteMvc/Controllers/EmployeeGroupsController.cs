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

namespace AdminLteMvc.Controllers
{
    //[Authorize(Roles = "Administradores")]
    public class EmployeeGroupsController : BaseController
    {
        private FtdContext db = new FtdContext();
        Business business = new  Business();

        // GET: EmployeeGroups
        public ActionResult Index()
        {
            var employeeGroups = db.EmployeeGroups.Include(e => e.Employee).Include(e => e.Group);
            return View(employeeGroups.ToList());
        }

        // GET: EmployeeGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeGroups employeeGroups = db.EmployeeGroups.Find(id);
            if (employeeGroups == null)
            {
                return HttpNotFound();
            }
            return View(employeeGroups);
        }

        // GET: EmployeeGroups/Create
        public ActionResult Create(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.IdEmployee = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text", id);
            ViewBag.IdGroup = new SelectList(Utilities.Utilities.GetGroups(), "Value", "Text");
            return View();
        }

        // POST: EmployeeGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdEmployee,IdGroup")] EmployeeGroups employeeGroups)
        {
            if (ModelState.IsValid)
            {
                var results = business.Add(employeeGroups);
                if (results.IsSuccess)
                {
                    Alert(results.Message, NotificationType.success);
                    Utilities.Utilities.PrepareAuditTrail("Empleado: " + employeeGroups.IdEmployee + " añadido al grupo: " + employeeGroups.IdGroup, User.Identity.Name,
                        AuditTrailAction.Insert);
                }
                else
                {
                    Alert(results.Message, NotificationType.error);
                }
            }

            ViewBag.IdEmployee = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text", employeeGroups.IdEmployee);
            ViewBag.IdGroup = new SelectList(Utilities.Utilities.GetGroups(), "Value", "Text", employeeGroups.IdGroup);
            return View(employeeGroups);
        }

        // GET: EmployeeGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeGroups employeeGroups = db.EmployeeGroups.Find(id);
            if (employeeGroups == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdEmployee = new SelectList(db.Employee, "Id", "Identification", employeeGroups.IdEmployee);
            ViewBag.IdGroup = new SelectList(db.Group, "Id", "Name", employeeGroups.IdGroup);
            return View(employeeGroups);
        }

        // POST: EmployeeGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdEmployee,IdGroup")] EmployeeGroups employeeGroups)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeGroups).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdEmployee = new SelectList(db.Employee, "Id", "Identification", employeeGroups.IdEmployee);
            ViewBag.IdGroup = new SelectList(db.Group, "Id", "Name", employeeGroups.IdGroup);
            return View(employeeGroups);
        }

        // GET: EmployeeGroups/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    EmployeeGroups employeeGroups = db.EmployeeGroups.Find(id);
        //    if (employeeGroups == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(employeeGroups);
        //}

        public ActionResult GetGroups(int id)
        {
            var a = business.GetEmployeeGroups(id);
            return View("EmployeeAgrouped", a);
        }

        // POST: EmployeeGroups/Delete/5
        [HttpGet, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeeGroups employeeGroups = db.EmployeeGroups.Find(id);
            db.EmployeeGroups.Remove(employeeGroups);
            Utilities.Utilities.PrepareAuditTrail("Empleado: " + employeeGroups.IdEmployee + " removido del grupo: " + employeeGroups.IdGroup, User.Identity.Name,
                AuditTrailAction.Delete);
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
