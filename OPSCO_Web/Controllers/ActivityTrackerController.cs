using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OPSCO_Web.Models;
using PagedList;
using PagedList.Mvc;

namespace OPSCO_Web.Controllers
{
    public class ActivityTrackerController : Controller
    {
        private OSCContext db = new OSCContext();

        // GET: ActivityTracker
        public ActionResult Index(int? page, int? pageSize, string searchString)
        {
            foreach (OSC_ActivityTracker act in db.ActivityTrackers)
            {
                act.Team = db.Teams.Find(act.TeamId);
                act.Representative = db.Representatives.Find(act.RepId);
                act.Representative.Location = db.Locations.Find(act.Representative.LocationId);
                act.Representative.CoreRole = db.CoreRoles.Find(act.Representative.CoreRoleId);
                act.Representative.FullName = act.Representative.FirstName + " " + act.Representative.LastName;
            }

            int? defaultPageSize = 10;

            if (pageSize != null)
            {
                defaultPageSize = pageSize;
            }

            var acts = (from a in db.ActivityTrackers
                        where a.Activity != "Attendance"
                        select a);

            if (!String.IsNullOrEmpty(searchString))
            {
                acts = acts.Where(a => a.Activity.Contains(searchString) || a.Representative.FullName.Contains(searchString));
            }

            return View(acts.OrderByDescending(a => a.Year).ThenByDescending(a => a.Month).ThenByDescending(a => a.TeamId).ToPagedList(page ?? 1, (int)defaultPageSize));
        }

        // GET: ActivityTracker/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_ActivityTracker oSC_ActivityTracker = db.ActivityTrackers.Find(id);
            oSC_ActivityTracker.Team = db.Teams.Find(oSC_ActivityTracker.TeamId);
            oSC_ActivityTracker.Representative = db.Representatives.Find(oSC_ActivityTracker.RepId);
            oSC_ActivityTracker.Representative.FullName = oSC_ActivityTracker.Representative.FirstName + " " + oSC_ActivityTracker.Representative.LastName;
            if (oSC_ActivityTracker == null)
            {
                return HttpNotFound();
            }
            return View(oSC_ActivityTracker);
        }

        // GET: ActivityTracker/Create
        public ActionResult Create(long? teamId, long? repId)
        {
            foreach (OSC_Representative rep in db.Representatives)
            { rep.FullName = rep.FirstName + " " + rep.LastName; }
            ViewBag.Teams = new SelectList(db.Teams, "TeamId", "TeamName");
            ViewBag.Activity = db.activities;
            long defaultTeamId = 0;
            var reps = (from r in db.Representatives select r);
            if (teamId != null)
            { reps = reps.Where(r => r.TeamId == teamId); }
            else
            { reps = reps.Where(r => r.TeamId == defaultTeamId); }
            ViewBag.Representatives = new SelectList(reps, "RepId", "FullName");
            if (repId != null)
            {
                ViewBag.WorkHours = db.Representatives.Find(repId).WorkHours;
            }
            return View();
        }

        // POST: ActivityTracker/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ActivityId,RepId,Month,Year,DateFrom,DateTo,Activity,NoOfHours,DateModified,ModifiedBy,NoOfDays,TeamId,IsActive")] OSC_ActivityTracker oSC_ActivityTracker)
        {
            oSC_ActivityTracker.Month = Convert.ToDateTime(oSC_ActivityTracker.DateFrom).Month;
            oSC_ActivityTracker.Year = Convert.ToDateTime(oSC_ActivityTracker.DateFrom).Year;
            oSC_ActivityTracker.DateModified = DateTime.Now;
            oSC_ActivityTracker.ModifiedBy = "svcBizTech";
            if (ModelState.IsValid)
            {
                db.ActivityTrackers.Add(oSC_ActivityTracker);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(oSC_ActivityTracker);
        }

        // GET: ActivityTracker/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_ActivityTracker oSC_ActivityTracker = db.ActivityTrackers.Find(id);
            oSC_ActivityTracker.Team = db.Teams.Find(oSC_ActivityTracker.TeamId);
            oSC_ActivityTracker.Representative = db.Representatives.Find(oSC_ActivityTracker.RepId);
            oSC_ActivityTracker.Representative.FullName = oSC_ActivityTracker.Representative.FirstName + " " + oSC_ActivityTracker.Representative.LastName;
            ViewBag.Activity = db.activities;
            if (oSC_ActivityTracker == null)
            {
                return HttpNotFound();
            }
            return View(oSC_ActivityTracker);
        }

        // POST: ActivityTracker/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ActivityId, RepId, Month, Year, DateFrom, DateTo, Activity, NoOfHours, DateModified, ModifiedBy, NoOfDays, TeamId, IsActive")] OSC_ActivityTracker oSC_ActivityTracker)
        {
            oSC_ActivityTracker.Month = Convert.ToDateTime(oSC_ActivityTracker.DateFrom).Month;
            oSC_ActivityTracker.Year = Convert.ToDateTime(oSC_ActivityTracker.DateFrom).Year;
            oSC_ActivityTracker.DateModified = DateTime.Now;
            oSC_ActivityTracker.ModifiedBy = "svcBizTech";
            if (ModelState.IsValid)
            {
                db.Entry(oSC_ActivityTracker).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oSC_ActivityTracker);
        }

        // GET: ActivityTracker/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_ActivityTracker oSC_ActivityTracker = db.ActivityTrackers.Find(id);
            oSC_ActivityTracker.Team = db.Teams.Find(oSC_ActivityTracker.TeamId);
            oSC_ActivityTracker.Representative = db.Representatives.Find(oSC_ActivityTracker.RepId);
            oSC_ActivityTracker.Representative.FullName = oSC_ActivityTracker.Representative.FirstName + " " + oSC_ActivityTracker.Representative.LastName;
            if (oSC_ActivityTracker == null)
            {
                return HttpNotFound();
            }
            return View(oSC_ActivityTracker);
        }

        // POST: ActivityTracker/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            OSC_ActivityTracker oSC_ActivityTracker = db.ActivityTrackers.Find(id);
            db.ActivityTrackers.Remove(oSC_ActivityTracker);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
