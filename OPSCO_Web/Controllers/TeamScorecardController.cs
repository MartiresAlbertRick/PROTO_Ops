using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPSCO_Web.Models;

namespace OPSCO_Web.Controllers
{
    public class TeamScorecardController : Controller
    {
        private OSCContext db = new OSCContext();

        // GET: TeamScorecard
        public ActionResult Index()
        {
            ViewBag.Months = db.months;
            ViewBag.Years = db.years;
            ViewBag.Teams = new SelectList(db.Teams, "TeamId", "TeamName");
            return View();
        }
    }
}