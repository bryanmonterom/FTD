using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
    public class EmployeesController : BaseController
    {
        private FtdContext db = new FtdContext();
        Business business= new Business();

        // GET: Employees
         public ActionResult Index(int? page, string name = "", string card = "")
        {
            int pageNumber = page ?? 1;
            var employees = business.EmployeeFilter(card, name);
            return View(employees.OrderBy(a => a.Name).ToPagedList(pageNumber, Utilities.Utilities.pageSize));
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.IdSupervisor = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text");
            ViewBag.IdDepartment = new SelectList(Utilities.Utilities.GetDepartments(), "Value", "Text");

            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Identification,Name,Email,IdSupervisor,IdDepartment, IsSupervisor")] Employee employee, HttpPostedFileBase files)
        {
            ViewBag.IdSupervisor = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text", employee.IdSupervisor);
            ViewBag.IdDepartment = new SelectList(Utilities.Utilities.GetDepartments(), "Value", "Text", employee.IdDepartment);

            if (ModelState.IsValid)
            {
                byte[] bytes;
                var results = business.Find(employee, false);
                if (files== null)
                {
                    Alert("El archivo de firmas no puede estar en blanco", NotificationType.error);
                    return View(employee);
                }

                if (files.ContentType.ToLower() != "image/jpeg" && files.ContentType.ToLower() != "image/png")
                {
                    Alert("Solo se aceptan archivos en formato .png o .jpg", NotificationType.error);
                    return View(employee);


                }

                using (BinaryReader br = new BinaryReader(files.InputStream))
                {
                   bytes = br.ReadBytes(files.ContentLength);
                   employee.FileName = Path.GetFileName(files.FileName);
                   employee.ContentType = files.ContentType;
                   employee.ByteContent = bytes;
                }
                if (results.IsSuccess == true)
                {
                    db.Employee.Add(employee);
                    db.SaveChanges();
                    Utilities.Utilities.PrepareAuditTrail("Empleado añadido: " + employee.Identification, User.Identity.Name,
                        AuditTrailAction.Insert);
                    Alert("Transaccion completada con exito", NotificationType.success);
                }
                else
                {
                    Alert(results.Message, NotificationType.error);

                }
            }
            if (employee.ByteContent != null)
            {
                ViewBag.ImageData = PrepareImage(employee);
            }


            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdSupervisor = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text", employee.IdSupervisor);
            ViewBag.IdDepartment = new SelectList(Utilities.Utilities.GetDepartments(), "Value", "Text", employee.IdDepartment);
            if (employee.ByteContent != null)
            {
                ViewBag.ImageData = PrepareImage(employee);
            }
            return View(employee);
        }

        private string PrepareImage(Employee employee)
        {
            string imageBase64Data = Convert.ToBase64String(employee.ByteContent);
            string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
            return imageDataURL;
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Identification,Name,Email,IdSupervisor,IdDepartment, IsSupervisor, ByteContent, ContentType")] Employee employee, HttpPostedFileBase files)
        {
            ViewBag.IdSupervisor = new SelectList(Utilities.Utilities.GetEmployees(), "Value", "Text", employee.IdSupervisor);
            ViewBag.IdDepartment = new SelectList(Utilities.Utilities.GetDepartments(), "Value", "Text", employee.IdDepartment);

            if (ModelState.IsValid)
            {
                var results = business.Find(employee, true);
                if (files != null) 
                {
                    if (files.ContentType.ToLower() != "image/jpeg" && files.ContentType.ToLower() != "image/png")
                    {
                        Alert("Solo se aceptan archivos en formato .png o .jpg", NotificationType.error);
                        return View(employee);


                    }

                    using (BinaryReader br = new BinaryReader(files.InputStream))
                    {
                        byte[] bytes = br.ReadBytes(files.ContentLength);
                        employee.FileName = Path.GetFileName(files.FileName);
                        employee.ContentType = files.ContentType;
                        employee.ByteContent = bytes;
                    }
                }

                if (results.IsSuccess == true)
                {

                    db.Entry(employee).State = EntityState.Modified;
                    db.SaveChanges();
                    Utilities.Utilities.PrepareAuditTrail("Empleado actualizado: " + employee.Identification, User.Identity.Name,
                        AuditTrailAction.Update);
                    Alert("Transaccion completada con exito", NotificationType.success);

                }
                else
                {
                    Alert(results.Message, NotificationType.error);
                }
            }

            if (employee.ByteContent !=null)
                ViewBag.ImageData = PrepareImage(employee);

            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
                Employee employee = db.Employee.Find(id);

                try
                {

                    db.Employee.Remove(employee);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    Alert(exception.Message.Replace("'", " "), NotificationType.error);
                    return View("Delete", employee);

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
