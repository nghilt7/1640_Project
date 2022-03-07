using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _1640_Project.Models;

namespace _1640_Project.Controllers
{
    public class AccountsController : Controller
    {
        private IdeasDbContext db = new IdeasDbContext();

        // GET: Accounts/Create
        //Hon
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
                Session["CurrentUserRoleID"] = user.RoleID;
                return RedirectToAction("Index", "Home");
            }

            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", user.DepartmentID);
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

        //Hon
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login([Bind(Include = "Name,PasswordHash")] User user)
        {
            // Them Validation
                var usr = db.Users.Where(u => u.Name == user.Name && u.PasswordHash == user.PasswordHash).FirstOrDefault();
                if (usr != null)
                {
                    Session["CurrentUserID"] = usr.UserID;
                    Session["CurrentUserName"] = usr.Name;
                    Session["CurrentUserRoleID"] = usr.RoleID;
                    if (usr.RoleID == 1)
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    else if (usr.RoleID == 2)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (usr.RoleID == 3)
                    {
                        return RedirectToAction("Index", "Home", new { area = "QAM" });
                    } else
                    {
                    return RedirectToAction("Index", "Home", new { area = "QAC" });
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Name or Password is wrong.");

                }
                return View();
        }

        //Hon
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
                return RedirectToAction("Index", "Home");
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", user.DepartmentID);
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }
    }
}
