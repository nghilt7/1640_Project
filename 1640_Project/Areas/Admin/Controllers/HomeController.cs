using _1640_Project.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _1640_Project.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        [AdminAuthorization]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Permission()
        {
            return View();
        }
    }
}