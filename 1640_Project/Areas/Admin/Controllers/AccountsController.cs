using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _1640_Project.Models;

namespace _1640_Project.Areas.Admin.Controllers
{
    public class AccountsController : Controller
    {
        private IdeasDbContext db = new IdeasDbContext();

        // GET: Accounts
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Department).Include(u => u.Role);
            return View(users.ToList());
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Accounts/Create
        public ActionResult Register()
        {
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName");
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "UserID,Name,Email,PasswordHash,PhoneNumber,DepartmentID,RoleID")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                Session["CurrentUserID"] = user.UserID;
                Session["CurrentUserName"] = user.Name;
                Session["CurrentUserEmail"] = user.Email;
                Session["CurrentUserPassword"] = user.PasswordHash;
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", user.DepartmentID);
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login([Bind(Include = "Email,PasswordHash")] User user)
        {
            // Them Validation
            var usr = db.Users.Where(u => u.Name == user.Name && u.PasswordHash == user.PasswordHash).FirstOrDefault();
            if (usr != null)
            {
                if (usr.RoleID == 1)
                {
                    Session["CurrentUserID"] = usr.UserID;
                    Session["CurrentUserName"] = usr.Name;
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else
                {
                    Session["CurrentUserID"] = usr.UserID.ToString();
                    Session["CurrentUserEmail"] = usr.Email.ToString();
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Name or Password is wrong.");

            }
            return View();
        }




        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChangeProfile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", user.DepartmentID);
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeProfile([Bind(Include = "UserID,Name,Email,PasswordHash,PhoneNumber,DepartmentID,RoleID")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", user.DepartmentID);
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
