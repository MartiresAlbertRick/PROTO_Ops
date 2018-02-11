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
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanView = db.appFacade.CanView(grp_id, "Activity Tracker");
                ViewBag.CanAdd = db.appFacade.CanAdd(grp_id, "Activity Tracker");
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "Activity Tracker");
                ViewBag.CanDelete = db.appFacade.CanDelete(grp_id, "Activity Tracker");

                if (!ViewBag.CanView) return HttpNotFound();
            }
            catch (Exception exception)
            {
                //session expired
                string result = exception.Message.ToString();
                return HttpNotFound();
            }

            var acts = (from a in db.ActivityTrackers
                        where a.Activity != "Attendance"
                        select a);
            List<long> TeamIds = new List<long>();
            switch (role)
            {
                case "Manager":
                case "Team Leader":
                case "Department Analyst":
                    foreach (OSC_ActivityTracker obj in acts)
                    {
                        if (db.IsManaged(obj.TeamId, user_name, role))
                            if (!TeamIds.Contains(obj.TeamId))
                                TeamIds.Add(obj.TeamId);
                    }
                    acts = (from a in db.ActivityTrackers
                            where a.Activity != "Attendance" && TeamIds.Contains(a.TeamId) && a.IsActive == true
                            select a);
                    break;
                case "Staff":
                    long repId = db.GetRepresentativeByPRD(user_name).RepId;
                    acts = acts.Where(a => a.RepId == repId && a.IsActive);
                    break;
            }
            #endregion "BTSS"
            #region "Initialize"
            db.InitializeActivities();
            #endregion "Initialize"
            #region "Table"
            int? defaultPageSize = 10;
            if (pageSize != null) defaultPageSize = pageSize;
            if (!String.IsNullOrEmpty(searchString)) acts = acts.Where(a => a.Activity.Contains(searchString));
            #endregion "Table"
            #region "Return"
            return View(acts.OrderByDescending(a => a.Year).ThenByDescending(a => a.Month).ThenByDescending(a => a.TeamId).ToPagedList(page ?? 1, (int)defaultPageSize));
            #endregion "Return"
        }

        // GET: ActivityTracker/Details/5
        public ActionResult Details(long? id)
        {
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanView = db.appFacade.CanView(grp_id, "Activity Tracker");
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "Activity Tracker");
                if (!ViewBag.CanView) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "Method"
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            OSC_ActivityTracker oSC_ActivityTracker = db.ActivityTrackers.Find(id);
            if (oSC_ActivityTracker == null) return HttpNotFound();
            if (!db.IsManaged(oSC_ActivityTracker.TeamId, user_name, role)) return HttpNotFound();
            oSC_ActivityTracker.Team = db.Teams.Find(oSC_ActivityTracker.TeamId);
            oSC_ActivityTracker.Representative = db.Representatives.Find(oSC_ActivityTracker.RepId);
            oSC_ActivityTracker.Representative.FullName = oSC_ActivityTracker.Representative.FirstName + " " + oSC_ActivityTracker.Representative.LastName;
            #endregion "Method"
            #region "Return"
            return View(oSC_ActivityTracker);
            #endregion "Return"
        }

        // GET: ActivityTracker/Create
        public ActionResult Create(long? teamId, long? repId)
        {
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanAdd = db.appFacade.CanAdd(grp_id, "Activity Tracker");
                if (!ViewBag.CanAdd) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "Initialize"
            db.InitializeRepresentatives();
            #endregion
            #region "ViewBagActivity"
            ViewBag.Activity = db.activities;
            #endregion "ViewBagActivity"
            #region "ViewBagTeam"
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
                    ViewBag.Teams = new SelectList(db.Teams.Where(x => TeamIds.Contains(x.TeamId) && x.IsActive == true), "TeamId", "TeamName");
                    break;
                case "Staff":
                    long repTeamId = (long)db.GetRepresentativeByPRD(user_name).TeamId;
                    ViewBag.Teams = new SelectList(db.Teams.Where(t => t.TeamId == repTeamId), "TeamId", "TeamName");
                    break;
            }
            #endregion "ViewBagTeam"
            #region "ViewBagRepresentative"
            long defaultTeamId = 0;
            var reps = (from r in db.Representatives select r);
            if (teamId != null)
            {
                long repIdd = db.GetRepresentativeByPRD(user_name).RepId;
                if (role == "Staff") reps = reps.Where(r => r.TeamId == teamId && r.RepId == repIdd); 
                else reps = reps.Where(r => r.TeamId == teamId);
            }
            else
            {
                reps = reps.Where(r => r.TeamId == defaultTeamId);
            }
            ViewBag.Representatives = new SelectList(reps, "RepId", "FullName");
            #endregion "ViewBagRepresentative"
            #region "ViewBagWorkHours"
            if (repId != null) ViewBag.WorkHours = db.Representatives.Find(repId).WorkHours;
            #endregion "ViewBagWorkHours"
            #region "Return" 
            return View();
            #endregion "Return" 
        }

        // POST: ActivityTracker/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ActivityId,RepId,Month,Year,DateFrom,DateTo,Activity,NoOfHours,DateModified,ModifiedBy,NoOfDays,TeamId,IsActive")] OSC_ActivityTracker oSC_ActivityTracker)
        {
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanAdd = db.appFacade.CanAdd(grp_id, "Activity Tracker");
                if (!ViewBag.CanAdd) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "AddValues"
            oSC_ActivityTracker.Month = Convert.ToDateTime(oSC_ActivityTracker.DateFrom).Month;
            oSC_ActivityTracker.Year = Convert.ToDateTime(oSC_ActivityTracker.DateFrom).Year;
            oSC_ActivityTracker.DateModified = DateTime.Now;
            oSC_ActivityTracker.ModifiedBy = user_name;
            oSC_ActivityTracker.IsActive = true;
            #endregion "AddValues"
            #region "Method"
            if (ModelState.IsValid)
            {
                db.ActivityTrackers.Add(oSC_ActivityTracker);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            #endregion "Method"
            #region "Return"
            return View(oSC_ActivityTracker);
            #endregion "Return"
        }

        // GET: ActivityTracker/Edit/5
        public ActionResult Edit(long? id)
        {
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "Activity Tracker");
                if (!ViewBag.CanEdit) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "Method"
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            OSC_ActivityTracker oSC_ActivityTracker = db.ActivityTrackers.Find(id);
            ViewBag.Activity = db.activities;
            if (oSC_ActivityTracker == null) return HttpNotFound();
            if (!db.IsManaged(oSC_ActivityTracker.TeamId, user_name, role)) return HttpNotFound();
            oSC_ActivityTracker.Team = db.Teams.Find(oSC_ActivityTracker.TeamId);
            oSC_ActivityTracker.Representative = db.Representatives.Find(oSC_ActivityTracker.RepId);
            oSC_ActivityTracker.Representative.FullName = oSC_ActivityTracker.Representative.FirstName + " " + oSC_ActivityTracker.Representative.LastName;
            #endregion "Method"
            #region "Return"
            return View(oSC_ActivityTracker);
            #endregion "Return"
        }

        // POST: ActivityTracker/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ActivityId, RepId, Month, Year, DateFrom, DateTo, Activity, NoOfHours, DateModified, ModifiedBy, NoOfDays, TeamId, IsActive")] OSC_ActivityTracker oSC_ActivityTracker)
        {
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "Activity Tracker");
                if (!ViewBag.CanEdit) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "AddValues"
            oSC_ActivityTracker.Month = Convert.ToDateTime(oSC_ActivityTracker.DateFrom).Month;
            oSC_ActivityTracker.Year = Convert.ToDateTime(oSC_ActivityTracker.DateFrom).Year;
            oSC_ActivityTracker.DateModified = DateTime.Now;
            oSC_ActivityTracker.ModifiedBy = user_name;
            if (Session["role"].ToString() != "Admin") oSC_ActivityTracker.IsActive = true;
            #endregion "AddValues"
            #region "Method"
            if (ModelState.IsValid)
            {
                db.Entry(oSC_ActivityTracker).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            #endregion "Method"
            #region "Return"
            return View(oSC_ActivityTracker);
            #endregion "Return"
        }

        // GET: ActivityTracker/Delete/5
        public ActionResult Delete(long? id)
        {
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanDelete = db.appFacade.CanDelete(grp_id, "Activity Tracker");
                if (!ViewBag.CanDelete) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "Method"
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            OSC_ActivityTracker oSC_ActivityTracker = db.ActivityTrackers.Find(id);
            if (oSC_ActivityTracker == null) return HttpNotFound();
            if (!db.IsManaged(oSC_ActivityTracker.TeamId, user_name, role)) return HttpNotFound();
            oSC_ActivityTracker.Team = db.Teams.Find(oSC_ActivityTracker.TeamId);
            oSC_ActivityTracker.Representative = db.Representatives.Find(oSC_ActivityTracker.RepId);
            oSC_ActivityTracker.Representative.FullName = oSC_ActivityTracker.Representative.FirstName + " " + oSC_ActivityTracker.Representative.LastName;
            #endregion "Method"
            #region "Return"
            return View(oSC_ActivityTracker);
            #endregion "Return"
        }

        // POST: ActivityTracker/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanDelete = db.appFacade.CanDelete(grp_id, "Activity Tracker");
                if (!ViewBag.CanDelete) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "AddValues"
            OSC_ActivityTracker oSC_ActivityTracker = db.ActivityTrackers.Find(id);
            oSC_ActivityTracker.DateModified = DateTime.Now;
            oSC_ActivityTracker.ModifiedBy = user_name;
            oSC_ActivityTracker.IsActive = false;
            #endregion "AddValues"
            #region "Method"
            if (ModelState.IsValid)
            {
                db.Entry(oSC_ActivityTracker).State = EntityState.Modified;
                db.SaveChanges();
            }
            #endregion "Method"
            #region "Return"
            return RedirectToAction("Index");
            #endregion "Return"
            //old delete method
            //OSC_ActivityTracker oSC_ActivityTracker = db.ActivityTrackers.Find(id);
            //db.ActivityTrackers.Remove(oSC_ActivityTracker);
            //db.SaveChanges();
            //return RedirectToAction("Index");
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
