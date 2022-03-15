using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using _1640_Project.Models;



namespace _1640_Project.Areas.QAM.Controllers
{
    public class DownController : Controller
    {
        // GET: QAM/Down
        IdeasDbContext db = new IdeasDbContext();
        public ActionResult Index()
        {
            List<Submission> submissions = db.Submissions.ToList();
            return View(submissions);
        }

        public ActionResult Down(int id)
        {
            List<Idea> ideas = db.Ideas.Where(t => t.SubmissionID == id).ToList();
            ViewBag.SubID = id;
            return View(ideas);
        }

        [HttpPost]
        public FileResult ExportToExcel(int SubID)
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[9] { new DataColumn("IdeaID"),
                                                     new DataColumn("Title"),
                                                     new DataColumn("Description"),
                                                     new DataColumn("Content"),
                                                     new DataColumn("CreateDate"),
                                                     new DataColumn("ViewCount"),
                                                     new DataColumn("UserID"),
                                                     new DataColumn("CategoryID"),
                                                     new DataColumn("SubmissionID")});

            var Idea = from IdeasDbContext in db.Ideas.Where(t => t.SubmissionID == SubID) select IdeasDbContext;

            foreach (var insurance in Idea)
            {
                dt.Rows.Add(insurance.IdeaID, insurance.Title, insurance.Description, insurance.Content,
                    insurance.CreateDate, insurance.ViewCount, insurance.UserID, insurance.CategoryID,
                    insurance.SubmissionID);
            }

            using (XLWorkbook wb = new XLWorkbook()) //Install ClosedXml from Nuget for XLWorkbook  
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream()) //using System.IO;  
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExcelFile.xlsx");
                }
            }
        }

    }
}
