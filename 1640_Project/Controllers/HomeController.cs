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
        
        public ActionResult View(int Sub, int PageNo = 1, string search = "", string SortColumn = "")
        {
            // Search
            ViewBag.search = search;
            List<Idea> ideas = db.Ideas.Where(t => t.SubmissionID == Sub && t.Title.Contains(search)).ToList();
            ViewBag.SubID = Sub;

            // Sorting
            ViewBag.SortColumn = SortColumn;

            if (SortColumn == "TopView")
            {
                ideas = ideas.OrderByDescending(i => i.ViewCount).ToList();
            } 
            else if (SortColumn == "TopLike")
            {
                ideas = ideas.OrderByDescending(i => i.VotesCount).ToList();
            } 
            else if (SortColumn == "Newest")
            {
                ideas = ideas.OrderByDescending(i => i.CreateDate).ToList();
            }

                //Pagination
                int NoOfRecordsPerPage = 5;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ideas.Count) / Convert.ToDouble(NoOfRecordsPerPage)));
            int NoOfRecordsToSkip = (PageNo - 1) * NoOfRecordsPerPage;
            ViewBag.PageNo = PageNo;
            ViewBag.NoOfPages = NoOfPages;
            ideas = ideas.Skip(NoOfRecordsToSkip).Take(NoOfRecordsPerPage).ToList();

            // Check Like
            for (int i = 0; i < ideas.Count; i++)
            {
                var id = ideas[i].IdeaID;
                var user = Convert.ToInt32(Session["CurrentUserID"]);
                Vote vt = db.Votes.Where(v => v.IdeaID == id && v.UserID == user).FirstOrDefault();
                if (vt != null)
                {
                    ideas[i].CurrentUserVoteType = vt.VoteValue;
                }
                else
                {
                    ideas[i].CurrentUserVoteType = 0;
                }
            }

            //Check Close Deadline
            Submission Submit = db.Submissions.Where(s => s.SubmissionID == Sub).FirstOrDefault();
            int result = DateTime.Compare(Submit.CloseDate, DateTime.Now);
            if (result < 0)
            {
                ViewBag.Deadline = 0;
            }
            else
            {
                ViewBag.DeadLine = 1;
            }

            db.SaveChanges();

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

        public ActionResult Permission()
        {
            return View();
        }
    }
}