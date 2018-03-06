﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPSCO_Web.Models;
using OPSCO_Web.BL;
using System.Web.Script.Serialization;
using word = Microsoft.Office.Interop.Word;
using xl = Microsoft.Office.Interop.Excel;
using System.IO;
using ClosedXML.Excel;

namespace OPSCO_Web.Controllers
{
    public class IndividualScorecardController : Controller
    {
        #region "InitializeObjects"
        private OSCContext db = new OSCContext();
        private AppFacade af = new AppFacade();
        #endregion "InitializeObjects"

        #region "Index"
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
                ViewBag.CanView = af.CanView(grp_id, "Individual Scorecard");
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
            af.InitializeRepresentatives(db);
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
                        if (af.IsManaged(team.TeamId, user_name, role))
                            if (!TeamIds.Contains(team.TeamId))
                                TeamIds.Add(team.TeamId);
                    }
                    ViewBag.Teams = new SelectList(db.Teams.Where(x => TeamIds.Contains(x.TeamId) && x.IsActive), "TeamId", "TeamName");
                    break;
                case "Staff":
                    long repTeamId = (long)af.GetRepresentativeByPRD(user_name).TeamId;
                    ViewBag.Teams = new SelectList(db.Teams.Where(t => t.TeamId == repTeamId && t.IsActive), "TeamId", "TeamName");
                    break;

            }
            #endregion "ViewBagTeams"
            #region "ViewBagRepresentatives"
            long defaultTeamId = 0;
            var reps = (from r in db.Representatives select r);
            if (teamId != null)
            {
                long repIdd = af.GetRepresentativeByPRD(user_name).RepId;
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
        #endregion "Index"

        #region "MainScorecard"
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

        public PartialViewResult ScorecardBase()
        {
            return PartialView();
        }
        #endregion "MainScorecard"

        #region "PDF"
        public ActionResult GetWord()
        {
            string logon_user = (string)Session["logon_user"];
            long teamId, repId;
            int month, year;
            teamId = (long)Session["IS_Team"];
            repId = (long)Session["IS_Rep"];
            month = (int)Session["IS_Month"];
            year = (int)Session["IS_Year"];
            OSC_Team oSC_Team = db.Teams.Find(teamId);
            OSC_Representative oSC_Representative = db.Representatives.Find(repId);
            
            word.Application app = new word.Application();
            string templateFileName = Server.MapPath("~/Templates/ScorecardTemplate.docx");
            word.Document doc = app.Documents.Open(templateFileName);
            string exportPath = Server.MapPath("~/Export");
            string fileName = exportPath + "/IndividualScorecard_" + oSC_Representative.LastName + oSC_Representative.FirstName + "_" + month.ToString() + year.ToString() + "_" + logon_user + ".docx";
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
            list = af.GetIndividualScorecardFull(teamId, repId, month, year);
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
            IndividualScorecard ind = af.GetIndividualScorecard(teamId, repId, month, year);
            if (doc.Bookmarks.Exists("Highlights"))
            {
                doc.Bookmarks["Highlights"].Range.Text = ind.Highlights;
            }
            #endregion "Highlights"

            #region "ProdChart"

            #endregion "ProdChart"

            #region "NPTChart"

            #endregion "NPTChart"

            string pdfFileName = exportPath + "/IndividualScorecard_" + oSC_Representative.LastName + oSC_Representative.FirstName + "_" + month.ToString() + year.ToString() + "_" + logon_user + ".pdf";
            doc.SaveAs2(pdfFileName, word.WdSaveFormat.wdFormatPDF);
            doc.Close();
            app.Quit();
            var mimeType = "application/pdf";
            var pdf = System.IO.File.ReadAllBytes(pdfFileName);
            FileResult fileResult = new FileContentResult(pdf, mimeType);            
            fileResult.FileDownloadName = "IndividualScorecard_" + oSC_Representative.LastName + oSC_Representative.FirstName + "_" + month.ToString() + year.ToString() + "_" + logon_user + ".pdf";
            System.IO.File.Delete(fileName);
            return fileResult;
        }

        public FileResult GetPDF()
        {
            string logon_user = (string)Session["logon_user"];
            long teamId, repId;
            int month, year;
            teamId = (long)Session["IS_Team"];
            repId = (long)Session["IS_Rep"];
            month = (int)Session["IS_Month"];
            year = (int)Session["IS_Year"];
            OSC_Team oSC_Team = db.Teams.Find(teamId);
            OSC_Representative oSC_Representative = db.Representatives.Find(repId);

            string templateFileName = Server.MapPath("~/Templates/Template.xlsx");
            XLWorkbook workbook = new XLWorkbook(templateFileName);
            string exportPath = Server.MapPath("~/Export");
            string excelFileName = "IndividualScorecard_" + oSC_Representative.LastName + oSC_Representative.FirstName + "_" + month.ToString() + year.ToString() + "_" + logon_user + ".xlsx";
            string pdfFileName = "IndividualScorecard_" + oSC_Representative.LastName + oSC_Representative.FirstName + "_" + month.ToString() + year.ToString() + "_" + logon_user + ".pdf";
            string tempFileName = exportPath + "/" + excelFileName;
            string outputFileName = exportPath + "/" + pdfFileName;
            if (System.IO.File.Exists(tempFileName)) System.IO.File.Delete(tempFileName);
            workbook.SaveAs(tempFileName);
            #region "Cover"
            IXLWorksheet worksheet = workbook.Worksheet("Scorecard");
            worksheet.Cell("D2").Value = oSC_Team.TeamName + " Monthly Scorecard";
            worksheet.Range("D2:K3").Row(1).Merge();
            #endregion "Cover"
            #region "Table"
            List<IndividualScorecard> list = new List<IndividualScorecard>();
            list = af.GetIndividualScorecardFull(teamId, repId, month, year);
            int x = 2;
            int y = 29;
            IXLCell cell = worksheet.Cell(y, x);
            cell.SetValue("Attendance");
            x += 1;
            cell = worksheet.Cell(29, x);
            cell.SetValue("Overtime");
            x += 1;
            cell = worksheet.Cell(29, x);
            cell.SetValue("NPT Hours");
            x += 1;
            cell = worksheet.Cell(29, x);
            cell.SetValue("Processing Time");
            x += 1;
            cell = worksheet.Cell(29, x);
            cell.SetValue("Total Transactions");
            x += 1;
            cell = worksheet.Cell(29, x);
            cell.SetValue("Rate of Production");
            x += 1;
            cell = worksheet.Cell(29, x);
            cell.SetValue("Processing Quality");
            x += 1;
            cell = worksheet.Cell(29, x);
            cell.SetValue("Total Utilization");
            x += 1;
            cell = worksheet.Cell(29, x);
            cell.SetValue("Efficiency");
            
            for (int i = 1; i <= list.Count; i++)
            {
                y += 1;
                x = 2;
                cell = worksheet.Cell(y, x);
                cell.SetValue(list[i - 1].individualActivities.Attendance_Days.ToString());
                x += 1;
                cell = worksheet.Cell(y, x);
                cell.SetValue(list[i - 1].individualActivities.Overtime_Hours.ToString());
                x += 1;
                cell = worksheet.Cell(y, x);
                cell.SetValue(list[i - 1].individualNonProcessing.NPTHours.ToString());
                x += 1;
                cell = worksheet.Cell(y, x);
                cell.SetValue(list[i - 1].individualBIProd.ProcessingHours.ToString());
                x += 1;
                cell = worksheet.Cell(y, x);
                cell.SetValue(list[i - 1].individualBIProd.Count.ToString());
                x += 1;
                cell = worksheet.Cell(y, x);
                cell.SetValue(list[i - 1].ProductivityRating.ToString() + "%");
                x += 1;
                cell = worksheet.Cell(y, x);
                cell.SetValue(list[i - 1].individualBIQual.QualityRating.ToString() + "%");
                x += 1;
                cell = worksheet.Cell(y, x);
                cell.SetValue(list[i - 1].TotalUtilization.ToString() + "%");
                x += 1;
                cell = worksheet.Cell(y, x);
                cell.SetValue(list[i - 1].Efficiency.ToString() + "%");
            }
            do
            {
                x += 1;
                worksheet.Column(x).Clear();
            }
            while (x <= 23);
            #endregion "Table"

            workbook.SaveAs(tempFileName);
            xl.Application app = new xl.Application();
            xl.Workbook wb = app.Workbooks.Open(tempFileName);
            wb.ExportAsFixedFormat(xl.XlFixedFormatType.xlTypePDF, outputFileName);
            //using (MemoryStream stream = new MemoryStream())
            //{
            //                workbook.SaveAs(stream);
            //return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelFileName);
            //}
            byte[] fileBytes = System.IO.File.ReadAllBytes(outputFileName);
            string fileName = pdfFileName;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        #endregion "PDF"

        #region "Cover"
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
        #endregion "Cover"

        #region "Table"
        public PartialViewResult ScorecardMainTable()
        {
            long teamId, repId;
            int month, year;
            teamId = (long)Session["IS_Team"];
            repId = (long)Session["IS_Rep"];
            month = (int)Session["IS_Month"];
            year = (int)Session["IS_Year"];
            List<IndividualScorecard> list = af.GetIndividualScorecardFull(teamId, repId, month, year);
            return PartialView(list);
        }
        #endregion "Table"

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

            var worktypes = af.GetIndividualWorkTypes(teamId, repId, month, year);
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

            var npts = af.GetIndividualNPT(teamId, repId, month, year);
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
            ViewBag.Highlights = af.GetIndividualScorecard(teamId, repId, month, year).Highlights;
            return PartialView();
        }
        #endregion "Highlights"
    }
}