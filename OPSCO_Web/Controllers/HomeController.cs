using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPSCO_Web.Models;

namespace OPSCO_Web.Controllers
{
    public class HomeController : Controller
    {
        private OSCContext db = new OSCContext();

        public ActionResult Index()
        {
            string logon_user = Request.ServerVariables["LOGON_USER"].ToString();
            logon_user = logon_user.Remove(0, logon_user.IndexOf('\\') + 1);
            logon_user = "martiab";
            Session["logon_user"] = logon_user;
            Session["user_id"] = "";
            var s = db.appFacade.GetUserInfo(logon_user);
            if (s != null)
            {
                string user_id = db.appFacade.GetUserInfo(logon_user).user_id;
                string grp_id = db.appFacade.GetUserAccess(user_id).grp_id;
                string full_name = db.appFacade.GetUserInfo(logon_user).user_first_name + " " + db.appFacade.GetUserInfo(logon_user).user_last_name;
                string role = "";
                Session["user_full_name"] = full_name;
                Session["user_id"] = user_id;
                Session["grp_id"] = grp_id;

                if (db.appFacade.IsAdmin(grp_id))
                {
                    role = "Admin";
                    Session["role"] = role;
                }
                else
                {
                    if (db.appFacade.IsManager(grp_id)) { Session["role"] = "Manager"; }
                    if (db.appFacade.IsTeamLeader(grp_id)) { Session["role"] = "Team Leader"; }
                    if (db.appFacade.IsDepAnalyst(grp_id)) { Session["role"] = "Department Analyst"; }
                    if (db.appFacade.IsStaff(grp_id)) { Session["role"] = "Staff"; }
                    Session["Maintenance"] = db.appFacade.CanView(grp_id, "Maintenance");
                    Session["Department"] = db.appFacade.CanView(grp_id, "Department");
                    Session["Team"] = db.appFacade.CanView(grp_id, "Team");
                    Session["Representative"] = db.appFacade.CanView(grp_id, "Representative");
                    Session["Manager"] = db.appFacade.CanView(grp_id, "Manager");
                    Session["ActivityTracker"] = db.appFacade.CanView(grp_id, "Activity Tracker");
                    Session["NPTTracker"] = db.appFacade.CanView(grp_id, "NPT Tracker");
                    Session["ManualEntry"] = db.appFacade.CanView(grp_id, "Manual Entries");
                    Session["Import"] = db.appFacade.CanView(grp_id, "Import Data");
                    Session["Scorecard"] = db.appFacade.CanView(grp_id, "Scorecards");
                    Session["IndividualScorecard"] = db.appFacade.CanView(grp_id, "Individual Scorecard");
                    Session["TeamScorecard"] = db.appFacade.CanView(grp_id, "Team Scorecard");
                }
            }
            ViewBag.UserInfo = s;
            return View();
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

        public PartialViewResult SideNav()
        {
            return PartialView("SideNav");
        }
    }
}