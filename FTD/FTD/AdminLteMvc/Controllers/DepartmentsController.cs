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
    public class DepartmentsController : BaseController
    {
        private FtdContext db = new FtdContext();
        Business business = new Business();

        // GET: Departments
        public ActionResult Index()
        {
            return View(db.Department.OrderBy(a=> a.Name).ToList());
        }

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Department.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Departments/Create
        public ActionResult Create()
        {
            ViewBag.IdDirector = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name, Identifier, IdDirector")] Department department)
        {
            ViewBag.IdDirector = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text", department.IdDirector);
            if (ModelState.IsValid)
            {
                if (department.IdDirector == null)
                {
                    Alert("Debes seleccionar un director para este departamento",NotificationType.error);
                    
                }
                else
                {
                    if (business.ValidateIfDirector((int)department.IdDirector))
                    {
                    Alert("Ya este empleado es director de un departamento",NotificationType.error);

                    }
                    else
                    {
                        db.Department.Add(department);
                        db.SaveChanges();

                        Utilities.Utilities.PrepareAuditTrail("Departamento añadido: " + department.Name, User.Identity.Name,
                            AuditTrailAction.Insert);
                        return RedirectToAction("Index");
                    }
                }

           
            }
            return View(department);
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Department.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdDirector = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text", department.IdDirector);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name, Identifier, IdDirector")]
            Department department)
        {

            ViewBag.IdDirector =
                new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text", department.IdDirector);
            if (ModelState.IsValid)
            {
                if (department.IdDirector == null)
                {
                    Alert("Debes seleccionar un director para este departamento", NotificationType.error);

                }
                else
                {
                    if (business.ValidateIfDirector((int) department.IdDirector, department.Id))
                    {
                        Alert("Ya este empleado es director de un departamento", NotificationType.error);

                    }
                    else
                    {
                        db.Entry(department).State = EntityState.Modified;
                        db.SaveChanges();

                        Utilities.Utilities.PrepareAuditTrail("Departamento modificado: " + department.Name + "ID: "+department.Id, User.Identity.Name,
                            AuditTrailAction.Update);
                        return RedirectToAction("Index");
                    }


                }

            }
            return View(department);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Department.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Department department = db.Department.Find(id);
            db.Department.Remove(department);
            db.SaveChanges();

            Utilities.Utilities.PrepareAuditTrail("Departamento removido: " + department.Name + "ID: " + department.Id, User.Identity.Name,
                AuditTrailAction.Delete);
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
