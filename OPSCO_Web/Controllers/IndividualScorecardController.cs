using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPSCO_Web.Models;
using System.Web.Script.Serialization;
using SelectPdf;
using Microsoft.Office.Interop.Word;
using word = Microsoft.Office.Interop.Word;
using xl = Microsoft.Office.Interop.Excel;
using office = Microsoft.Office.Core;
using System.IO;
namespace OPSCO_Web.Controllers
{
    public class IndividualScorecardController : Controller
    {
        private OSCContext db = new OSCContext();

        // GET: IndividualScorecard
        public ActionResult Index(long? teamId, int? month, int? year)
        {
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanView = db.appFacade.CanView(grp_id, "Individual Scorecard");
                if (!ViewBag.CanView) return HttpNotFound();
            }
            catch (Exception exception)
            {
                //session expired
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "Initialize"
            db.InitializeRepresentatives();
            #endregion "Initialize"
            #region "ViewBagMonths"
            if (month != null) ViewBag.Months = new SelectList(db.months, "Value", "Text", month);
            else ViewBag.Months = db.months;
            #endregion "ViewBagMonths"
            #region "ViewBagYears"
            if (year != null) ViewBag.Years = new SelectList(db.years, "Value", "Text", year);
            else ViewBag.Years = db.years;
            #endregion "ViewBagYears"
            #region "ViewBagTeams"
            switch (role)
            {
                case "Admin":
                    ViewBag.Teams = new SelectList(db.Teams, "TeamId", "TeamName");
                    break;
                case "Manager":
                case "Team Leader":
                case "Department Analyst":
                    List<long> TeamIds = new List<long>();
                    foreach (OSC_Team team in db.Teams)
                    {
                        if (db.IsManaged(team.TeamId, user_name, role))
                            if (!TeamIds.Contains(team.TeamId))
                                TeamIds.Add(team.TeamId);
                    }
                    ViewBag.Teams = new SelectList(db.Teams.Where(x => TeamIds.Contains(x.TeamId) && x.IsActive), "TeamId", "TeamName");
                    break;
                case "Staff":
                    long repTeamId = (long)db.GetRepresentativeByPRD(user_name).TeamId;
                    ViewBag.Teams = new SelectList(db.Teams.Where(t => t.TeamId == repTeamId && t.IsActive), "TeamId", "TeamName");
                    break;

            }
            #endregion "ViewBagTeams"
            #region "ViewBagRepresentatives"
            long defaultTeamId = 0;
            var reps = (from r in db.Representatives select r);
            if (teamId != null)
            {
                long repIdd = db.GetRepresentativeByPRD(user_name).RepId;
                switch (role)
                {
                    case "Admin":
                        reps = reps.Where(r => r.TeamId == teamId);
                        break;
                    case "Manager":
                    case "Team Leader":
                    case "Department Analyst":
                        reps = reps.Where(r => r.TeamId == teamId && r.IsActive);
                        break;
                    case "Staff":
                        reps = reps.Where(r => r.TeamId == teamId && r.RepId == repIdd && r.IsActive);
                        break;
                }
            }
            else
            {
                reps = reps.Where(r => r.TeamId == defaultTeamId);
            }
            ViewBag.Representatives = new SelectList(reps, "RepId", "FullName");
            #endregion "ViewBagRepresentatives"
            #region "Return"
            return View();
            #endregion "Return"
        }

        public ActionResult ScorecardView(long? teamId, long? repId, int? month, int? year)
        {
            OSC_Team oSC_Team = db.Teams.Find((long)teamId);
            if (oSC_Team == null) return HttpNotFound();
            OSC_Representative oSC_Representative = db.Representatives.Find((long)repId);
            if (oSC_Representative == null) return HttpNotFound();
            oSC_Representative.FullName = oSC_Representative.FirstName + " " + oSC_Representative.LastName;
            ViewBag.Title = oSC_Representative.FullName;
            ViewBag.Team = oSC_Team.TeamName;
            ViewBag.Representative = oSC_Representative.FullName;
            ViewBag.Month = db.months.Where(m => m.Value == Convert.ToString((int)month)).First().Text;
            ViewBag.Year = year;
            Session["IS_Team"] = teamId;
            Session["IS_Rep"] = repId;
            Session["IS_Month"] = month;
            Session["IS_Year"] = year;
            return View();
        }

        public ActionResult ScorecardViewHidden(long? teamId, long? repId, int? month, int? year)
        {
            OSC_Team oSC_Team = db.Teams.Find((long)teamId);
            if (oSC_Team == null) return HttpNotFound();
            OSC_Representative oSC_Representative = db.Representatives.Find((long)repId);
            if (oSC_Representative == null) return HttpNotFound();
            oSC_Representative.FullName = oSC_Representative.FirstName + " " + oSC_Representative.LastName;
            ViewBag.Title = oSC_Representative.FullName;
            ViewBag.Team = oSC_Team.TeamName;
            ViewBag.Representative = oSC_Representative.FullName;
            ViewBag.Month = db.months.Where(m => m.Value == Convert.ToString((int)month)).First().Text;
            ViewBag.Year = year;
            Session["IS_Team"] = teamId;
            Session["IS_Rep"] = repId;
            Session["IS_Month"] = month;
            Session["IS_Year"] = year;
            return View();
        }

        public ActionResult GetPdf()
        {
            long repId;
            int month, year;
            repId = (long)Session["IS_Rep"];
            month = (int)Session["IS_Month"];
            year = (int)Session["IS_Year"];
            OSC_Representative oSC_Representative = db.Representatives.Find(repId);

            HtmlToPdf converter = new HtmlToPdf();
            var doc = converter.ConvertUrl("http://localhost:61845/IndividualScorecard/ScorecardView" + 
                "?teamId=" + Session["IS_Team"].ToString() +
                "&repId=" + Session["IS_Rep"].ToString() + 
                "&month=" + Session["IS_Month"].ToString() + 
                "&year=" + Session["IS_Year"].ToString());

            byte[] pdf = doc.Save();
            doc.Close();

            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = "IndividualScorecard_" + oSC_Representative.LastName + oSC_Representative.FirstName + "_" + month.ToString() + "_" + year.ToString() + ".pdf";
            return fileResult;
        }

        public ActionResult GetWord()
        {
            long teamId, repId;
            int month, year;
            teamId = (long)Session["IS_Team"];
            repId = (long)Session["IS_Rep"];
            month = (int)Session["IS_Month"];
            year = (int)Session["IS_Year"];
            OSC_Team oSC_Team = db.Teams.Find(teamId);
            OSC_Representative oSC_Representative = db.Representatives.Find(repId);
            
            Application app = new word.Application();
            string templateFileName = Server.MapPath("~/ImportFile/ScorecardTemplate.docx");
            Document doc = app.Documents.Open(templateFileName);
            string exportPath = Server.MapPath("~/Export");
            string fileName = exportPath + "/IndividualScorecard_" + oSC_Representative.LastName + oSC_Representative.FirstName + "_" + month.ToString() + "_" + year.ToString() + ".docx";
            if (System.IO.File.Exists(fileName)) System.IO.File.Delete(fileName);
            doc.SaveAs(fileName);
            doc.Activate();

            #region "CoverValues"
            if (doc.Bookmarks.Exists("TeamName"))
            {
                doc.Bookmarks["TeamName"].Range.Text = oSC_Team.TeamName;
            }
            if (doc.Bookmarks.Exists("RepName"))
            {
                doc.Bookmarks["RepName"].Range.Text = oSC_Representative.FirstName + " " + oSC_Representative.LastName;
            }
            if (doc.Bookmarks.Exists("Month"))
            {
                doc.Bookmarks["Month"].Range.Text = db.months.Where(m => m.Value == Convert.ToString((int)month)).First().Text;
            }
            if (doc.Bookmarks.Exists("Year"))
            {
                doc.Bookmarks["Year"].Range.Text = year.ToString();
            }
            #endregion "CoverValues"

            #region "Table"
            List<IndividualScorecard> list = new List<IndividualScorecard>();
            list = db.GetIndividualScorecardFull(teamId, repId, month, year);
            int noOfRows = list.Count();
            int noOfCols = 10;
            word.Table table = doc.Tables[1];
            for (int i = 1; i <= noOfCols - 1; i++)
            {
                table.Columns.Add(table.Columns[1]);
            }
            table.Rows[1].Cells[1].Range.Text = "Month";
            table.Rows[1].Cells[2].Range.Text = "Attendance";
            table.Rows[1].Cells[3].Range.Text = "Overtime";
            table.Rows[1].Cells[4].Range.Text = "NPT Hours";
            table.Rows[1].Cells[5].Range.Text = "Processing Time";
            table.Rows[1].Cells[6].Range.Text = "Total Transactions";
            table.Rows[1].Cells[7].Range.Text = "Rate of Production";
            table.Rows[1].Cells[8].Range.Text = "Processing Quality";
            table.Rows[1].Cells[9].Range.Text = "Total Utilization";
            table.Rows[1].Cells[10].Range.Text = "Efficiency";

            for (int i = 1; i <= list.Count - 1; i++)
            {
                table.Rows.Add(table.Rows[2]);
            }
            for (int i = 1; i <= list.Count; i++)
            {
                table.Rows[i + 1].Cells[1].Range.Text = list[i - 1].MonthName;
                table.Rows[i + 1].Cells[2].Range.Text = list[i - 1].individualActivities.Attendance_Days.ToString();
                table.Rows[i + 1].Cells[3].Range.Text = list[i - 1].individualActivities.Overtime_Hours.ToString();
                table.Rows[i + 1].Cells[4].Range.Text = list[i - 1].individualNonProcessing.NPTHours.ToString();
                table.Rows[i + 1].Cells[5].Range.Text = list[i - 1].individualBIProd.ProcessingHours.ToString();
                table.Rows[i + 1].Cells[6].Range.Text = list[i - 1].individualBIProd.Count.ToString();
                table.Rows[i + 1].Cells[7].Range.Text = list[i - 1].ProductivityRating.ToString() + "%";
                table.Rows[i + 1].Cells[8].Range.Text = list[i - 1].individualBIQual.QualityRating.ToString() + "%";
                table.Rows[i + 1].Cells[9].Range.Text = list[i - 1].TotalUtilization.ToString() + "%";
                table.Rows[i + 1].Cells[10].Range.Text = list[i - 1].Efficiency.ToString() + "%";
            }
            #endregion "Table"

            #region "Highlights"
            IndividualScorecard ind = db.GetIndividualScorecard(teamId, repId, month, year);
            if (doc.Bookmarks.Exists("Highlights"))
            {
                doc.Bookmarks["Highlights"].Range.Text = ind.Highlights;
            }
            #endregion "Highlights"

            xl.Application _excelApp = new xl.Application();
            xl.Workbook workbook = _excelApp.Workbooks.Add();
            string excelFileName = Server.MapPath("~/Export/IndividualScorecard_" + oSC_Representative.LastName + oSC_Representative.FirstName + "_" + month.ToString() + "_" + year.ToString() + ".xlsx");
            workbook.SaveAs(excelFileName);
            workbook.Close();
            _excelApp.Quit();
            

            string pdfFileName = exportPath + "/IndividualScorecard_" + oSC_Representative.LastName + oSC_Representative.FirstName + "_" + month.ToString() + "_" + year.ToString() + ".pdf";
            doc.SaveAs2(pdfFileName, word.WdSaveFormat.wdFormatPDF);
            doc.Close();
            app.Quit();
            var mimeType = "application/pdf";
            var pdf = System.IO.File.ReadAllBytes(pdfFileName);
            FileResult fileResult = new FileContentResult(pdf, mimeType);            
            fileResult.FileDownloadName = "IndividualScorecard_" + oSC_Representative.LastName + oSC_Representative.FirstName + "_" + month.ToString() + "_" + year.ToString() + ".pdf";
            System.IO.File.Delete(fileName);
            System.IO.File.Delete(excelFileName);
            return fileResult;
        }

        public PartialViewResult ScorecardBase()
        {
            return PartialView();
        }

        public PartialViewResult ScorecardCover()
        {
            long teamId, repId;
            int month, year;
            teamId = (long)Session["IS_Team"];
            repId = (long)Session["IS_Rep"];
            month = (int)Session["IS_Month"];
            year = (int)Session["IS_Year"];
            OSC_Team oSC_Team = db.Teams.Find(teamId);
            OSC_Representative oSC_Representative = db.Representatives.Find(repId);
            oSC_Representative.FullName = oSC_Representative.FirstName + " " + oSC_Representative.LastName;
            ViewBag.Team = oSC_Team.TeamName;
            ViewBag.Representative = oSC_Representative.FullName;
            ViewBag.Month = db.months.Where(m => m.Value == Convert.ToString(month)).First().Text;
            ViewBag.Year = year;
            return PartialView();
        }

        public PartialViewResult ScorecardMainTable()
        {
            long teamId, repId;
            int month, year;
            teamId = (long)Session["IS_Team"];
            repId = (long)Session["IS_Rep"];
            month = (int)Session["IS_Month"];
            year = (int)Session["IS_Year"];
            List<IndividualScorecard> list = db.GetIndividualScorecardFull(teamId, repId, month, year);
            return PartialView(list);
        }

        #region "UploadImages"
        public ActionResult UploadProductivity(string imageData)
        {
            long teamId, repId;
            int month, year;
            teamId = (long)Session["IS_Team"];
            repId = (long)Session["IS_Rep"];
            month = (int)Session["IS_Month"];
            year = (int)Session["IS_Year"];
            OSC_Team oSC_Team = db.Teams.Find(teamId);
            OSC_Representative oSC_Representative = db.Representatives.Find(repId);
            string exportPath = Server.MapPath("~/Export");
            string fileNameWitPath = exportPath + "/IndividualScorecard_" + oSC_Representative.LastName + oSC_Representative.FirstName + "_" + month.ToString() + "_" + year.ToString() + "_prodchart.png";
            using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    byte[] data = Convert.FromBase64String(imageData);//convert from base64
                    bw.Write(data);
                    bw.Close();
                }
            }
            return null;
        }
        #endregion "UploadImages"

        #region "ProductivityChart"
        public PartialViewResult ScorecardProductivityChart()
        {
            return PartialView();
        }

        public JsonResult JsonProductivityChart()
        {
            long teamId, repId;
            int month, year;
            teamId = (long)Session["IS_Team"];
            repId = (long)Session["IS_Rep"];
            month = (int)Session["IS_Month"];
            year = (int)Session["IS_Year"];

            var worktypes = db.GetIndividualWorkTypes(teamId, repId, month, year);
            return Json(worktypes, JsonRequestBehavior.AllowGet);
        }
        #endregion "ProductivityChart"

        #region "NptChart"
        public PartialViewResult ScorecardNptChart()
        {
            return PartialView();
        }

        public JsonResult JsonNptChart()
        {
            long teamId, repId;
            int month, year;
            teamId = (long)Session["IS_Team"];
            repId = (long)Session["IS_Rep"];
            month = (int)Session["IS_Month"];
            year = (int)Session["IS_Year"];

            var npts = db.GetIndividualNPT(teamId, repId, month, year);
            return Json(npts, JsonRequestBehavior.AllowGet);
        }
        #endregion "NptChart"

        #region "Highlights"
        public PartialViewResult ScorecardHighlights()
        {
            long teamId, repId;
            int month, year;
            teamId = (long)Session["IS_Team"];
            repId = (long)Session["IS_Rep"];
            month = (int)Session["IS_Month"];
            year = (int)Session["IS_Year"];
            ViewBag.Highlights = db.GetIndividualScorecard(teamId, repId, month, year).Highlights;
            return PartialView();
        }
        #endregion "Highlights"
    }
}