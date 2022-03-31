using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _1640_Project.Models;
using _1640_Project.Filters;

namespace _1640_Project.Controllers
{
    public class HomeController : Controller
    {
        IdeasDbContext db = new IdeasDbContext();
        public ActionResult Index()
        {
            List<Submission> submissions = db.Submissions.ToList();
            return View(submissions);
        }
        
        public ActionResult View(int id)
        {
            List<Idea> ideas = db.Ideas.Where(t => t.SubmissionID == id).ToList();
            ViewBag.SubID = id;
            return View(ideas);
        }

        public ActionResult Category(int id)
        {
            List<Category> categories = db.Categories.ToList();
            ViewBag.SubmitID = id;
            return View(categories);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Agree()
        {
            return View();
        }
    }
}