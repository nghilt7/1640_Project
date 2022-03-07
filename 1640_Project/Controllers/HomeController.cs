using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _1640_Project.Models;

namespace _1640_Project.Controllers
{
    public class HomeController : Controller
    {
        IdeasDbContext db = new IdeasDbContext();
        public ActionResult Index()
        {
            List<Category> categories = db.Categories.ToList();
            return View(categories);
        }
        
        public ActionResult View(int id)
        {
            List<Idea> ideas = db.Ideas.Where(t => t.SubmissionID == id).ToList();   
            return View(ideas);
        }

        public ActionResult Submit(int id)
        {
            List<Submission> submissions = db.Submissions.Where(t => t.CategoryID == id).ToList();
            ViewBag.id = id;
            return View(submissions);
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
    }
}