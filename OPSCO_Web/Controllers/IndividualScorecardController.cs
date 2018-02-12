using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPSCO_Web.Models;
using System.Web.Script.Serialization;
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
            return PartialView();
        }

        public PartialViewResult ScorecardProductivityChart()
        {
            return PartialView();
        }

        public PartialViewResult ScorecardHighlights()
        {
            return PartialView();
        }


        //public ActionResult ExportPDF()
        //{

        //}
    }
}