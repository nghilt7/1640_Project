using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _1640_Project.Filters;

namespace _1640_Project.Areas.QAC.Controllers
{
    public class HomeController : Controller
    {
        // GET: QAC/Home
        [QacAuthorization]
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