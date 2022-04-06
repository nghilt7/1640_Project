using _1640_Project.Filters;
using _1640_Project.Models;
using ClosedXML.Excel;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _1640_Project.Controllers
{
    public class DownController : Controller
    {
        IdeasDbContext db = new IdeasDbContext();

        [QamAuthorization]
        public ActionResult Index()
        {
            List<Submission> submissions = db.Submissions.ToList();
            return View(submissions);
        }

        [QamAuthorization]
        public ActionResult Down(int id)
        {
            List<Idea> ideas = db.Ideas.Where(t => t.SubmissionID == id).ToList();
            ViewBag.SubID = id;
            return View(ideas);
        }

        [QamAuthorization]
        [HttpPost]
        public FileResult ExportToExcel(int SubID)
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[10] { new DataColumn("IdeaID"),
                                                     new DataColumn("Title"),
                                                     new DataColumn("Description"),
                                                     new DataColumn("Content"),
                                                     new DataColumn("CreateDate"),
                                                     new DataColumn("ViewCount"),
                                                     new DataColumn("UserID"),
                                                     new DataColumn("CategoryID"),
                                                     new DataColumn("SubmissionID"),
                                                     new DataColumn("FileName")});

            var Idea = from IdeasDbContext in db.Ideas.Where(t => t.SubmissionID == SubID) select IdeasDbContext;

            foreach (var insurance in Idea)
            {
                dt.Rows.Add(insurance.IdeaID, insurance.Title, insurance.Description, insurance.Content,
                    insurance.CreateDate, insurance.ViewCount, insurance.UserID, insurance.CategoryID,
                    insurance.SubmissionID, insurance.FileName);
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

        [HttpPost]
        public ActionResult DownloadZip()
        {
            Response.Clear();
            Response.BufferOutput = false;
            Response.ContentType = "application/zip";
            Response.AddHeader("content-disposition", "attachment; filename= All_Ideas.zip");

            using (ZipFile zipfile = new ZipFile())
            {
                zipfile.AddSelectedFiles("*.*", Server.MapPath("~/UploadedFiles/"), "", false);
                zipfile.Save(Response.OutputStream);
            }
            return RedirectToAction("Index");
        }
    }
}