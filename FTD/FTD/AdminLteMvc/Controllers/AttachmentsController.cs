using System.Collections.Generic;
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
    //[Authorize]
    public class AttachmentsController : BaseController
    {
        private FtdContext db = new FtdContext();
        private Business business = new Business();
        [HttpPost]
        public ActionResult Upload(int id, IEnumerable<HttpPostedFileBase> files)
        {
            FileUploader fileUploader = new FileUploader() { Id = id };

            //bool IsSecretary = business.IsSecretary(User.Identity.Name);
            bool IsSecretary = !User.IsInRole("Administradores");


            var attachment = business.GetAttachment((int)id);
            if (attachment != null)
            {
                ViewBag.ByteContent = attachment.ByteContent;
            }
            if (files.First() != null)
            {
                foreach (var item in files)
                {
                    var results = business.Add(item, id, IsSecretary);
                    if (results.IsSuccess == false)
                    {
                        Alert(results.Message, NotificationType.error);
                        return View("Uploader", fileUploader);
                     
                    }
                    else
                    {
                        Utilities.Utilities.PrepareAuditTrail("Archivo añadido: " + item.FileName, User.Identity.Name,
                         AuditTrailAction.Insert);
                    }
                    Alert("Subida de archivo exitosa", NotificationType.success);

                }

                return View("Uploader", fileUploader);
            }
            Alert("Debe seleccionar al menos un archivo", NotificationType.error);
            return View("Uploader", fileUploader);
        }

        [HttpGet]
        public ActionResult Upload(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            FileUploader fileUploader = new FileUploader() { Id = (int)id };
            var results = business.GetAttachment((int)id);
            if (results != null)
            {
                ViewBag.ByteContent = true;
                return View("Uploader", fileUploader);
            }
            return View("Uploader", fileUploader);

        }

        public ActionResult Attachments(int idTransfer)
        {
            var attachments = business.GetAttachment(idTransfer);
            if (attachments == null)
            {
                return RedirectToAction("Upload", "Attachments", new { @id = idTransfer });
            }
            return RedirectToAction("Details", "Attachments", new { @id = attachments.Id });
        }

        public ActionResult Download(int id)
        {
            var file = db.Attachments.Find(id);
            if (file != null)
            {
                return File(Utilities.Utilities.Decrypt(file.ByteContent, file.Vector), System.Net.Mime.MediaTypeNames.Application.Octet, file.Name);
            }
            Alert("Algo fallo", NotificationType.error);
            return View("Index");
        }


        // GET: Attachments
        public ActionResult Index(int? page, int? IdRequestd=0, string identifier="")
        {
            var attachments =business.AttachmentsFilter(IdRequestd, identifier);
            int pageNumber = page ?? 1;
            return View(attachments.OrderBy(a=> a.IdRequest).ToPagedList(pageNumber, Utilities.Utilities.pageSize));
        }

        // GET: Attachments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attachments attachments = db.Attachments.Find(id);
            if (attachments == null)
            {
                return HttpNotFound();
            }
            return View(attachments);
        }

        // GET: Attachments/Create
        public ActionResult Create()
        {
            ViewBag.IdAttachmentType = new SelectList(db.AttachmentType, "Id", "Name");
            ViewBag.IdRequest = new SelectList(db.Request, "Id", "Identifier");
            return View();
        }

        // POST: Attachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,IdRequest,ByteContent,ContentType,IdAttachmentType")] Attachments attachments)
        {
            if (ModelState.IsValid)
            {
                db.Attachments.Add(attachments);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdAttachmentType = new SelectList(db.AttachmentType, "Id", "Name", attachments.IdAttachmentType);
            ViewBag.IdRequest = new SelectList(db.Request, "Id", "Identifier", attachments.IdRequest);
            return View(attachments);
        }

        // GET: Attachments/Edit/5
        public ActionResult Edit(int? id, int? IdFile)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            FileUploader fileUploader = new FileUploader() { Id = (int)id };
            ViewBag.IdFile = id;


            var results = business.GetAttachment((int)id);
            if (results != null)
            {
                ViewBag.ByteContent = true;
                return View(fileUploader);
            }
            return View(fileUploader);

        }

        // POST: Attachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, HttpPostedFileBase files, int? IdFile)
        {
            FileUploader fileUploader = new FileUploader() { IdFile = id};
            //bool IsSecretary = business.IsSecretary(User.Identity.Name);
            bool IsSecretary = !User.IsInRole("Administradores");

            ViewBag.IdFile = id;

            var results = business.Update(files, id, IsSecretary);
            if (results.IsSuccess == false)
            {
                Alert(results.Message, NotificationType.error);
                return View( fileUploader);

            }
            else
            {
                Utilities.Utilities.PrepareAuditTrail("Archivo añadido: " + files.FileName, User.Identity.Name,
                    AuditTrailAction.Insert);
            }
            Alert("Subida de archivo exitosa", NotificationType.success);
            return View(fileUploader);


        }

        // GET: Attachments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attachments attachments = db.Attachments.Include(a=> a.Request).Include(a=> a.AttachmentType).FirstOrDefault(A=> A.Id == id);
            if (attachments == null)
            {
                return HttpNotFound();
            }
            return View(attachments);
        }

        // POST: Attachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Attachments attachments = db.Attachments.Find(id);
            db.Attachments.Remove(attachments);
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
