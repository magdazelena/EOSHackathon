﻿using System;
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
using System.Net.Mail;

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

        [Authorize]
        [HttpPost]
        public ActionResult ProcessVerification(string CertificateHash, int CertificateId, int RequestId)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            Request r = db.Requests.Include("Certificate").Where(x => x.Id == RequestId && x.Certificate.EOSOwnerAccount == user.EOSAccountName).SingleOrDefault();
            if(r == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            r.Status = "Verified";
            db.SaveChanges();

            MailMessage mail = new MailMessage("eoshackathon@irespo.com", r.Email);
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "serwer1761616.home.pl";
            client.Credentials = new NetworkCredential("eoshackathon@irespo.com", "26a;!jl@b4qB");
            mail.Subject = "Your request has been verified.";
            mail.Body = "<p>Your request has been verified.</p>"
                  + "<p>CertificateId: "
                  + r.CertificateId
                  + "</p>"
                  + "<p>Name: "
                  + r.Certificate.Name
                  + "</p>"
                  + "<p>Author: "
                  + r.Certificate.Author
                  + "</p>"
                  + "<p>Place of issue: "
                  + r.Certificate.PlaceOfIssue
                  + "</p>"
                  + "<p>EOS Owner Account: "
                  + r.Certificate.EOSOwnerAccount
                  + "</p>"
                   + "<p>Certificate Hash: "
                  + CertificateHash
                  + "</p>"
                  ;
            mail.IsBodyHtml = true;
            client.Send(mail);

            return Json(new { success = true});
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

        [Authorize]
        [HttpPost]
        public ActionResult Verify()
        {
            return RedirectToAction("Index");
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

        // GET: Requests/Reject/5
        [Authorize]
        public ActionResult Reject(int? id)
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
        [HttpPost, ActionName("Reject")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult RejectConfirmed(int id)
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

            request.Status = "Rejected";
            //db.Requests.Remove(request);
            db.SaveChanges();

            MailMessage mail = new MailMessage("eoshackathon@irespo.com", request.Email);
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "serwer1761616.home.pl";
            client.Credentials = new NetworkCredential("eoshackathon@irespo.com", "26a;!jl@b4qB");
            mail.Subject = "Your request has been rejected.";
            mail.Body = "<p>Your request has been rejected.</p>"
                  + "<p>You requested accesing certificate with id " 
                  + request.CertificateId
                  + "</p>";
            mail.IsBodyHtml = true;
            client.Send(mail);

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
