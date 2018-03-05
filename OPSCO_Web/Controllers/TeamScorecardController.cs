using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPSCO_Web.Models;
using OPSCO_Web.BL;

namespace OPSCO_Web.Controllers
{
    public class TeamScorecardController : Controller
    {
        #region "InitializeObjects"
        private OSCContext db = new OSCContext();
        private AppFacade af = new AppFacade();
        #endregion "InitializeObjects"

        #region "Index"
        // GET: TeamScorecard
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
                ViewBag.CanView = af.CanView(grp_id, "Team Scorecard");
                if (!ViewBag.CanView) return HttpNotFound();
            }
            catch (Exception exception)
            {
                //session expired
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
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
            #region "Return"
            return View();
            #endregion "Return"
        }
        #endregion "Index"

        #region "MainScorecard"
        public ActionResult ScorecardView(long? teamId, int? month, int? year)
        {
            OSC_Team oSC_Team = db.Teams.Find((long)teamId);
            if (oSC_Team == null) return HttpNotFound();
            ViewBag.Title = oSC_Team.TeamName;
            ViewBag.Team = oSC_Team.TeamName;
            ViewBag.Month = db.months.Where(m => m.Value == Convert.ToString((int)month)).First().Text;
            ViewBag.Year = year;
            Session["TS_Team"] = teamId;
            Session["TS_Month"] = month;
            Session["TS_Year"] = year;
            return View();
        }

        public PartialViewResult ScorecardBase()
        {
            return PartialView();
        }

        public PartialViewResult ScorecardCover()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            OSC_Team oSC_Team = db.Teams.Find(teamId);
            ViewBag.Team = oSC_Team.TeamName;
            ViewBag.Month = db.months.Where(m => m.Value == Convert.ToString(month)).First().Text;
            ViewBag.Year = year;
            return PartialView();
        }
        #endregion "MainScorecard"

        #region "IndividualSummary"
        public PartialViewResult IndividualSummary()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            ViewBag.Month = db.months.Where(m => m.Value == Convert.ToString(month)).First().Text;
            ViewBag.Year = year;
            return PartialView();
        }

        public PartialViewResult IndividualSummaryTableSection()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            List<IndividualSummary> list = af.GetIndividualSummary(teamId, month, year);
            return PartialView(list);
        }

        public PartialViewResult IndividualSummaryChart()
        {
            return PartialView();
        }

        public JsonResult JsonIndividualProductivity()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            var s = af.GetIndividualProductivity(teamId, month, year);
            return Json(s, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsonIndividualQuality()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            var s = af.GetIndividualProductivity(teamId, month, year);
            return Json(s, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsonIndividualEfficiency()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            var s = af.GetIndividualEfficiency(teamId, month, year);
            return Json(s, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsonIndividualUtilization()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            var s = af.GetIndividualUtilization(teamId, month, year);
            return Json(s, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult IndividualSummaryComment()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            ViewBag.Id = db.TeamScorecards.Where(t => t.TeamId == teamId && t.Month == month && t.Year == year).FirstOrDefault().TeamScorecardId;
            ViewBag.Comment = db.TeamScorecards.Where(t => t.TeamId == teamId && t.Month == month && t.Year == year).FirstOrDefault().IndividualSummaryComments;
            return PartialView();
        }

        public JsonResult SaveIndividualSummaryComment(long? id, string comment)
        {
            object s = new { type = "failed", message = "Saving failed!" };
            if (id == null) return Json(s, JsonRequestBehavior.AllowGet);
            try
            {
                OSC_TeamScorecard_Current ts = db.TeamScorecards.Find(id);
                if (ModelState.IsValid)
                { 
                    ts.IndividualSummaryComments = comment;
                    db.Entry(ts).State = EntityState.Modified;
                    db.SaveChanges();
                }
                s = new { type = "success", message = "Successfully saved!" };
            }
            catch (Exception ex)
            {
                s = new { type = "failed", message = "Saving failed!\nError occured:\n" + ex.Message.ToString() };
            }
            return Json(s, JsonRequestBehavior.AllowGet);
        }
        #endregion "IndividualSummary"

        #region "TeamSummary"
        public PartialViewResult TeamSummary()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            ViewBag.Month = db.months.Where(m => m.Value == Convert.ToString(month)).First().Text;
            ViewBag.Year = year;
            return PartialView();
        }

        public PartialViewResult TeamSummaryTable()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            List<TeamScorecard> list = af.GetTeamSummaries(teamId, month, year);
            return PartialView(list);
        }

        public PartialViewResult TeamSummaryChart()
        {
            return PartialView();
        }

        public JsonResult JsonTeamProductivity()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            var s = af.GetTeamProductivity(teamId, month, year);
            return Json(s, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsonTeamQuality()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            var s = af.GetTeamQuality(teamId, month, year);
            return Json(s, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsonTeamEfficiency()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            var s = af.GetTeamEfficiency(teamId, month, year);
            return Json(s, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsonTeamUtilization()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            var s = af.GetTeamUtilizaton(teamId, month, year);
            return Json(s, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsonTeamNPT()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            var s = af.GetTeamNPT(teamId, month, year);
            return Json(s, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsonTeamTimers()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            var s = af.GetTeamTimers(teamId, month, year);
            return Json(s, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult TeamSummaryComment()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            ViewBag.Id = db.TeamScorecards.Where(t => t.TeamId == teamId && t.Month == month && t.Year == year).FirstOrDefault().TeamScorecardId;
            ViewBag.Comment = db.TeamScorecards.Where(t => t.TeamId == teamId && t.Month == month && t.Year == year).FirstOrDefault().TeamSummaryComments;
            return PartialView();
        }

        public JsonResult SaveTeamSummaryComment(long? id, string comment)
        {
            object s = new { type = "failed", message = "Saving failed!" };
            if (id == null) return Json(s, JsonRequestBehavior.AllowGet);
            try
            {
                OSC_TeamScorecard_Current ts = db.TeamScorecards.Find(id);
                if (ModelState.IsValid)
                {
                    ts.TeamSummaryComments = comment;
                    db.Entry(ts).State = EntityState.Modified;
                    db.SaveChanges();
                }
                s = new { type = "success", message = "Successfully saved!" };
            }
            catch (Exception ex)
            {
                s = new { type = "failed", message = "Saving failed!\nError occured:\n" + ex.Message.ToString() };
            }
            return Json(s, JsonRequestBehavior.AllowGet);
        }
        #endregion "TeamSummary"

        #region "WorktypeSummary"
        public PartialViewResult WorktypeSummary()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            ViewBag.Month = db.months.Where(m => m.Value == Convert.ToString(month)).First().Text;
            ViewBag.Year = year;
            return PartialView();
        }

        public PartialViewResult WorktypeSummaryComment()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            ViewBag.Id = db.TeamScorecards.Where(t => t.TeamId == teamId && t.Month == month && t.Year == year).FirstOrDefault().TeamScorecardId;
            ViewBag.Comment = db.TeamScorecards.Where(t => t.TeamId == teamId && t.Month == month && t.Year == year).FirstOrDefault().WorktypeSummaryComments;
            return PartialView();
        }

        public JsonResult SaveWorktypeSummaryComment(long? id, string comment)
        {
            object s = new { type = "failed", message = "Saving failed!" };
            if (id == null) return Json(s, JsonRequestBehavior.AllowGet);
            try
            {
                OSC_TeamScorecard_Current ts = db.TeamScorecards.Find(id);
                if (ModelState.IsValid)
                {
                    ts.WorktypeSummaryComments = comment;
                    db.Entry(ts).State = EntityState.Modified;
                    db.SaveChanges();
                }
                s = new { type = "success", message = "Successfully saved!" };
            }
            catch (Exception ex)
            {
                s = new { type = "failed", message = "Saving failed!\nError occured:\n" + ex.Message.ToString() };
            }
            return Json(s, JsonRequestBehavior.AllowGet);
        }
        #endregion "WorktypeSummary"

        #region "StatusSummary"
        public PartialViewResult StatusSummary()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            ViewBag.Month = db.months.Where(m => m.Value == Convert.ToString(month)).First().Text;
            ViewBag.Year = year;
            return PartialView();
        }

        public PartialViewResult StatusSummaryComment()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            ViewBag.Id = db.TeamScorecards.Where(t => t.TeamId == teamId && t.Month == month && t.Year == year).FirstOrDefault().TeamScorecardId;
            ViewBag.Comment = db.TeamScorecards.Where(t => t.TeamId == teamId && t.Month == month && t.Year == year).FirstOrDefault().StatusSummaryComments;
            return PartialView();
        }

        public JsonResult SaveStatusSummaryComment(long? id, string comment)
        {
            object s = new { type = "failed", message = "Saving failed!" };
            if (id == null) return Json(s, JsonRequestBehavior.AllowGet);
            try
            {
                OSC_TeamScorecard_Current ts = db.TeamScorecards.Find(id);
                if (ModelState.IsValid)
                {
                    ts.StatusSummaryComments = comment;
                    db.Entry(ts).State = EntityState.Modified;
                    db.SaveChanges();
                }
                s = new { type = "success", message = "Successfully saved!" };
            }
            catch (Exception ex)
            {
                s = new { type = "failed", message = "Saving failed!\nError occured:\n" + ex.Message.ToString() };
            }
            return Json(s, JsonRequestBehavior.AllowGet);
        }
        #endregion "StatusSummary"

        #region "ManagerSignoff"
        public PartialViewResult ManagerSignoff()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            ViewBag.Month = db.months.Where(m => m.Value == Convert.ToString(month)).First().Text;
            ViewBag.Year = year;
            OSC_TeamScorecard_Current obj = db.TeamScorecards.Where(t => t.TeamId == teamId && t.Month == month && t.Year == year).FirstOrDefault();
            ViewBag.Id = obj.TeamScorecardId;

            if (obj.IsSignedOff)
            {
                ViewBag.ManagerSignOff = obj.ManagerSignOff;
                ViewBag.SignOffDate = obj.SignOffDate;
                ViewBag.SignOffBy = obj.SignOffBy;
                ViewBag.IsSignedOff = obj.IsSignedOff;
            }
            else
            {
                ViewBag.ManagerSignOff = "";
                ViewBag.SignOffDate = DateTime.Now;
                ViewBag.SignOffBy = (string)Session["logon_user"];
                ViewBag.IsSignedOff = obj.IsSignedOff;
            }
            return PartialView();
        }

        public JsonResult SignOff(long? id, string comment)
        {
            object s = new { type = "failed", message = "Saving failed!" };
            if (id == null) return Json(s, JsonRequestBehavior.AllowGet);
            try
            {
                OSC_TeamScorecard_Current ts = db.TeamScorecards.Find(id);
                if (ModelState.IsValid)
                {
                    ts.ManagerSignOff = comment;
                    ts.SignOffDate = DateTime.Now;
                    ts.SignOffBy = (string)Session["logon_user"];
                    ts.IsSignedOff = true;
                    db.Entry(ts).State = EntityState.Modified;                 
                    db.SaveChanges();
                }
                s = new { type = "success", message = "Successfully signed-off!" };
            }
            catch (Exception ex)
            {
                s = new { type = "failed", message = "Saving failed!\nError occured:\n" + ex.Message.ToString() };
            }
            return Json(s, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CancelSignOff(long? id)
        {
            object s = new { type = "failed", message = "Saving failed!" };
            if (id == null) return Json(s, JsonRequestBehavior.AllowGet);
            try
            {
                OSC_TeamScorecard_Current ts = db.TeamScorecards.Find(id);
                if (ModelState.IsValid)
                {
                    ts.ManagerSignOff = "";
                    ts.SignOffDate = null;
                    ts.SignOffBy = null;
                    ts.IsSignedOff = false;
                    db.Entry(ts).State = EntityState.Modified;
                    db.SaveChanges();
                }
                s = new { type = "success", message = "Sign-off cancelled!" };
            }
            catch (Exception ex)
            {
                s = new { type = "failed", message = "Saving failed!\nError occured:\n" + ex.Message.ToString() };
            }
            return Json(s, JsonRequestBehavior.AllowGet);
        }
        #endregion "ManagerSignoff"

        #region "Appendix"
        public PartialViewResult Appendix()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            ViewBag.Month = db.months.Where(m => m.Value == Convert.ToString(month)).First().Text;
            ViewBag.Year = year;
            OSC_TeamScorecard_Current obj = db.TeamScorecards.Where(t => t.TeamId == teamId && t.Month == month && t.Year == year).FirstOrDefault();
            ViewBag.Id = obj.TeamScorecardId;

            return PartialView();
        }
        #endregion "Appendix"
    }
}