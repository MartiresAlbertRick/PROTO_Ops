using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPSCO_Web.Models;
using OPSCO_Web.BL;

namespace OPSCO_Web.Controllers
{
    public class TeamScorecardController : Controller
    {
        private OSCContext db = new OSCContext();
        private AppFacade af = new AppFacade();

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
            return View();
        }

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

        public PartialViewResult IndividualSummary()
        {
            long teamId;
            int month, year;
            teamId = (long)Session["TS_Team"];
            month = (int)Session["TS_Month"];
            year = (int)Session["TS_Year"];
            List<TeamViewRepList> list = af.GetRepList(teamId, month, year);
            return PartialView(list);
        }
    }
}