using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using _1640_Project.Filters;
using _1640_Project.Models;

namespace _1640_Project.Controllers
{
    public class IdeasController : Controller
    {
        private IdeasDbContext db = new IdeasDbContext();

        // Get MY IDEAS
        public ActionResult Index(int id)
        {
            var ideas = db.Ideas.Where(t => t.UserID == id).ToList();
            return View(ideas);
        }

        // GET: Ideas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Idea idea = db.Ideas.Find(id);
            if (idea == null)
            {
                return HttpNotFound();
            }
            return View(idea);
        }

        [UserAuthorization]
        public ActionResult Create(int submit)
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.SubmissionID = new SelectList(db.Submissions, "SubmissionID", "SubmissionName");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Name");
            ViewBag.submit = submit;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserAuthorization]
        public ActionResult Create([Bind(Include = "IdeaID,Title,Description,Content,CreateDate,ViewCount,UserID,CategoryID,SubmissionID")] Idea idea)
        {
            if (ModelState.IsValid)
            {
                db.Ideas.Add(idea);
                db.SaveChanges();

                idea.CreateDate = DateTime.Now;

                var user = db.Users.Find(idea.UserID);
                var departmentId = user.DepartmentID;
                var coordinator = db.Users.FirstOrDefault(c => c.RoleID == 4 && c.DepartmentID == departmentId);

                var senderEmail = new MailAddress("minhtien29042001@gmail.com", "Tiennnm@es.vn");
                var receiverEmail = new MailAddress(coordinator.Email, "Receiver");
                var password = "Lovelive9";
                var sub = idea.Title;
                var body = idea.Content;
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                };
                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = sub,
                    Body = body
                })
                {
                    smtp.Send(mess);
                }


                return RedirectToAction("Index", "Home");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", idea.CategoryID);
            ViewBag.SubmissionID = new SelectList(db.Submissions, "SubmissionID", "SubmissionName", idea.SubmissionID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Name", idea.UserID);
            return View(idea);
        }

        [UserAuthorization]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Idea idea = db.Ideas.Find(id);
            if (idea == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", idea.CategoryID);
            ViewBag.SubmissionID = new SelectList(db.Submissions, "SubmissionID", "SubmissionName", idea.SubmissionID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Name", idea.UserID);
            return View(idea);
        }

        [UserAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdeaID,Title,Description,Content,CreateDate,ViewCount,UserID,CategoryID,SubmissionID")] Idea idea)
        {
            if (ModelState.IsValid)
            {
                db.Entry(idea).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", idea.CategoryID);
            ViewBag.SubmissionID = new SelectList(db.Submissions, "SubmissionID", "SubmissionName", idea.SubmissionID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Name", idea.UserID);
            return View(idea);
        }

        [UserAuthorization]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Idea idea = db.Ideas.Find(id);
            if (idea == null)
            {
                return HttpNotFound();
            }
            return View(idea);
        }

        [UserAuthorization]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Idea idea = db.Ideas.Find(id);
            db.Ideas.Remove(idea);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
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
