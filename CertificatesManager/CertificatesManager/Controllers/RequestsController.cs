using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CertificatesManager.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace CertificatesManager.Controllers
{
    public class RequestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Requests
        [Authorize]
        public ActionResult Index()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            return View(db.Requests.Include("Certificate").Where(x => x.Certificate.EOSOwnerAccount == user.EOSAccountName).ToList());
        }

        // GET: Requests/Details/5
        [Authorize]
        public ActionResult Verify(int? id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db
                .Requests
                .Include("Certificate")
                .Where(x => x.Certificate.EOSOwnerAccount == user.EOSAccountName && x.Id == id)
                .SingleOrDefault();

            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(RequestViewModel request)
        {
            if (ModelState.IsValid)
            {
                Request r = new Request();
                r.CertificateId = request.CertificateId;
                r.Email = request.Email;
                r.EOSRequestorName = request.EOSRequestorName;
                r.Status = "NEW";
                db.Requests.Add(r);
                db.SaveChanges();
                return Json(new { requestId = r.Id });
            }

            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        // GET: Requests/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Request request = db
               .Requests
               .Include("Certificate")
               .Where(x => x.Certificate.EOSOwnerAccount == user.EOSAccountName && x.Id == id)
               .SingleOrDefault();

            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            Request request = db
              .Requests
              .Include("Certificate")
              .Where(x => x.Certificate.EOSOwnerAccount == user.EOSAccountName && x.Id == id)
              .SingleOrDefault();

            if (request == null)
            {
                return HttpNotFound();
            }
            db.Requests.Remove(request);
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
