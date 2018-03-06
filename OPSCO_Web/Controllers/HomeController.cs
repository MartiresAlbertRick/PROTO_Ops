using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPSCO_Web.Models;
using OPSCO_Web.BL;

namespace OPSCO_Web.Controllers
{
    public class HomeController : Controller
    {
        private OSCContext db = new OSCContext();
        private AppFacade af = new AppFacade();

        public ActionResult Index()
        {
            string logon_user = Request.ServerVariables["LOGON_USER"].ToString();
            logon_user = logon_user.Remove(0, logon_user.IndexOf('\\') + 1);
            //logon_user = "martiab";
            Session["logon_user"] = logon_user;
            Session["user_id"] = "";
            try
            {
                var s = af.GetUserInfo(logon_user);
                if (s != null)
                {
                    string user_id = af.GetUserInfo(logon_user).user_id;
                    string grp_id = af.GetUserAccess(user_id).grp_id;
                    string full_name = af.GetUserInfo(logon_user).user_first_name + " " + af.GetUserInfo(logon_user).user_last_name;
                    string role = "";
                    Session["user_full_name"] = full_name;
                    Session["user_id"] = user_id;
                    Session["grp_id"] = grp_id;

                    if (af.IsAdmin(grp_id))
                    {
                        role = "Admin";
                        Session["role"] = role;
                    }
                    else
                    {
                        if (af.IsManager(grp_id)) { Session["role"] = "Manager"; }
                        if (af.IsTeamLeader(grp_id)) { Session["role"] = "Team Leader"; }
                        if (af.IsDepAnalyst(grp_id)) { Session["role"] = "Department Analyst"; }
                        if (af.IsStaff(grp_id)) { Session["role"] = "Staff"; }
                        Session["Maintenance"] = af.CanView(grp_id, "Maintenance");
                        Session["Department"] = af.CanView(grp_id, "Department");
                        Session["Team"] = af.CanView(grp_id, "Team");
                        Session["Representative"] = af.CanView(grp_id, "Representative");
                        Session["Manager"] = af.CanView(grp_id, "Manager");
                        Session["ActivityTracker"] = af.CanView(grp_id, "Activity Tracker");
                        Session["NPTTracker"] = af.CanView(grp_id, "NPT Tracker");
                        Session["ManualEntry"] = af.CanView(grp_id, "Manual Entries");
                        Session["Import"] = af.CanView(grp_id, "Import Data");
                        Session["Scorecard"] = af.CanView(grp_id, "Scorecards");
                        Session["IndividualScorecard"] = af.CanView(grp_id, "Individual Scorecard");
                        Session["TeamScorecard"] = af.CanView(grp_id, "Team Scorecard");
                    }
                }
                ViewBag.UserInfo = s;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("~/Views/Shared/UnableToAccess.cshtml");
            }
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