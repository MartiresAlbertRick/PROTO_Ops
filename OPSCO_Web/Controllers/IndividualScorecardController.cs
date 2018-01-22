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
            foreach (OSC_Representative rep in db.Representatives)
            { rep.FullName = rep.FirstName + " " + rep.LastName; }
            if (month != null)
            { ViewBag.Months = new SelectList(db.months, "Value", "Text", month); }
            else
            { ViewBag.Months = db.months; }
            if (year != null)
            { ViewBag.Years = new SelectList(db.years, "Value", "Text", year); }
            else
            { ViewBag.Years = db.years; }
            ViewBag.Teams = new SelectList(db.Teams, "TeamId", "TeamName");
            long defaultTeamId = 0;
            var reps = (from r in db.Representatives select r);
            if (teamId != null)
            { reps = reps.Where(r => r.TeamId == teamId); }
            else
            { reps = reps.Where(r => r.TeamId == defaultTeamId); }
            ViewBag.Representatives = new SelectList(reps, "RepId", "FullName");
            return View();
        }
    }
}