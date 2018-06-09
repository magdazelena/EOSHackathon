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
    [Authorize]
    public class CertificatesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Certificates
        public ActionResult Index()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            ViewBag.EosAccount = user.EOSAccountName;


            return View(db.Certificates
                    .Where(x => x.EOSAuthorAccount == user.EOSAccountName || x.EOSOwnerAccount == user.EOSAccountName)
                    .ToList()
                );
        }

        // GET: Certificates/Details/5
        public ActionResult Details(int? id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Certificate certificate = db.Certificates
                .Where(x => (x.EOSAuthorAccount == user.EOSAccountName || x.EOSOwnerAccount == user.EOSAccountName) && id == x.Id).SingleOrDefault();

            if (certificate == null)
            {
                return HttpNotFound();
            }
            return View(certificate);
        }

        // GET: Certificates/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            ViewBag.Author = user.Name;
            ViewBag.eosauthor = user.EOSAccountName;

            return View();
        }

        // POST: Certificates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(CertificateViewModel certificate)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                Certificate cert = new Certificate();
                cert.Content = certificate.Content;
                cert.EOSOwnerAccount = certificate.EOSOwnerAccount;
                cert.Name = certificate.Name;
                cert.PlaceOfIssue = certificate.PlaceOfIssue;
                cert.Author = user.Name;
                cert.EOSAuthorAccount = user.EOSAccountName;

                db.Certificates.Add(cert);
                db.SaveChanges();
                return Json(new { certificateId = cert.Id });
            }

            return View(certificate);
        }
        
        // GET: Certificates/Delete/5
        public ActionResult Delete(int? id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Certificate certificate = db.Certificates
                .Where(x => (x.EOSAuthorAccount == user.EOSAccountName || x.EOSOwnerAccount == user.EOSAccountName) && x.Id == id)
                .SingleOrDefault();

            if (certificate == null)
            {
                return HttpNotFound();
            }

            ViewBag.CertificateId = id;

            return View(certificate);
        }

        // POST: Certificates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            Certificate certificate = db.Certificates
                .Where(x => (x.EOSAuthorAccount == user.EOSAccountName || x.EOSOwnerAccount == user.EOSAccountName) && x.Id == id)
                .SingleOrDefault();
            if (certificate == null)
            {
                return HttpNotFound();
            }
            db.Certificates.Remove(certificate);
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
