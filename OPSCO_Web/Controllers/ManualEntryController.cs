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
    public class ManualEntryController : Controller
    {
        private OSCContext db = new OSCContext();

        // GET: ManualEntry
        public ActionResult Index(int? page, int? pageSize, string searchString)
        {
            #region "Initialize"
            db.InitializeManualEntries();
            #endregion "Initialize"
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanView = db.appFacade.CanView(grp_id, "Manual Entries");
                ViewBag.CanAdd = db.appFacade.CanAdd(grp_id, "Manual Entries");
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "Manual Entries");
                ViewBag.CanDelete = db.appFacade.CanDelete(grp_id, "Manual Entries");

                if (!ViewBag.CanView) return HttpNotFound();
            }
            catch (Exception exception)
            {
                //session expired
                string result = exception.Message.ToString();
                return HttpNotFound();
            }

            var mes = (from m in db.ManualEntries
                       select m);
            List<long> TeamIds = new List<long>();
            switch (role)
            {
                case "Manager":
                case "Team Leader":
                case "Department Analyst":
                    foreach (OSC_ManualEntry obj in mes)
                    {
                        if (db.IsManaged(obj.TeamId, user_name, role))
                            if (!TeamIds.Contains((long)obj.TeamId))
                                TeamIds.Add((long)obj.TeamId);
                    }
                    mes = (from m in db.ManualEntries
                            where TeamIds.Contains((long)m.TeamId) && m.IsActive == true
                            select m);
                    break;
                case "Staff":
                    OSC_Representative oSC_Representative = db.GetRepresentativeByPRD(user_name);
                    long repId;
                    repId = 0;
                    if (oSC_Representative != null)
                    { repId = oSC_Representative.RepId; }
                    mes = mes.Where(m => m.RepId == repId && m.IsActive);
                    break;
            }
            #endregion "BTSS"
            #region "Table"
            int? defaultPageSize = 10;
            if (pageSize != null) defaultPageSize = pageSize;
            if (!String.IsNullOrEmpty(searchString)) mes = mes.Where(m => m.MonthName.Contains(searchString) || m.Year.ToString().Contains(searchString));
            #endregion "Table"
            #region "Return"
            return View(mes.OrderByDescending(m => m.Year).ThenByDescending(m => m.Month).ThenByDescending(m => m.TeamId).ToPagedList(page ?? 1, (int)defaultPageSize));
            #endregion "Return"
        }

        // GET: ManualEntry/Details/5
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
                ViewBag.CanView = db.appFacade.CanView(grp_id, "Manual Entries");
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "Manual Entries");
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
            OSC_ManualEntry oSC_ManualEntry = db.ManualEntries.Find(id);
            if (oSC_ManualEntry == null) return HttpNotFound();
            if (!db.IsManaged(oSC_ManualEntry.TeamId, user_name, role)) return HttpNotFound();
            oSC_ManualEntry.Team = db.Teams.Find(oSC_ManualEntry.TeamId);
            oSC_ManualEntry.Representative = db.Representatives.Find(oSC_ManualEntry.RepId);
            oSC_ManualEntry.Representative.FullName = oSC_ManualEntry.Representative.FirstName + " " + oSC_ManualEntry.Representative.LastName;
            oSC_ManualEntry.PeriodCoverage = Convert.ToDateTime(oSC_ManualEntry.Month + "/1/" + oSC_ManualEntry.Year);
            #endregion "Method"
            #region "Return"
            return View(oSC_ManualEntry);
            #endregion "Return"
        }

        // GET: ManualEntry/Create
        public ActionResult Create(long? teamId)
        {
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanAdd = db.appFacade.CanAdd(grp_id, "Manual Entries");
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
            #endregion "Initialize"
            #region "ViewBagProjectResponsibilities"
            ViewBag.ProjectResponsibilities = db.projectResponsibilities;
            #endregion "ViewBagProjectResponsibilities"
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
                    ViewBag.Teams = new SelectList(db.Teams.Where(t => t.TeamId == repTeamId), "TeamId", "TeamName");
                    break;
            }
            #endregion "ViewBagTeams"
            #region "ViewBagRepresentative"
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
            #endregion "ViewBagRepresentative"
            #region "Return"
            return View();
            #endregion "Return"
        }

        // POST: ManualEntry/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EntryId,TeamId,RepId,GainLossOccurances,GainLossAmount,CallManagementScore,ProjectResponsibility,ScheduleAdherence,Compliance,ProductAccuracy,Commitment,JHValues,CallEfficiency,Engagement,AdministrativeProcedures,Month,Year,DateUploaded,UploadedBy,IsActive,ActiveProjects,CompletedProjects,PeriodCoverage")] OSC_ManualEntry oSC_ManualEntry)
        {
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanAdd = db.appFacade.CanAdd(grp_id, "Manual Entries");
                if (!ViewBag.CanAdd) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "AddValues"
            oSC_ManualEntry.Month = Convert.ToDateTime(oSC_ManualEntry.PeriodCoverage).Month;
            oSC_ManualEntry.Year = Convert.ToDateTime(oSC_ManualEntry.PeriodCoverage).Year;
            oSC_ManualEntry.DateUploaded = DateTime.Now;
            oSC_ManualEntry.UploadedBy = user_name;
            oSC_ManualEntry.IsActive = true;
            #endregion "AddValues"
            #region "Method"
            if (ModelState.IsValid)
            {
                db.ManualEntries.Add(oSC_ManualEntry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            #endregion "Method"
            #region "Return"
            return View(oSC_ManualEntry);
            #endregion "Return"
        }

        // GET: ManualEntry/Edit/5
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
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "Manual Entries");
                if (!ViewBag.CanEdit) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "ViewBagProjectResponsibilities"
            ViewBag.ProjectResponsibilities = db.projectResponsibilities;
            #endregion "ViewBagProjectResponsibilities"
            #region "Method"
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            OSC_ManualEntry oSC_ManualEntry = db.ManualEntries.Find(id);
            if (oSC_ManualEntry == null) return HttpNotFound();
            if (!db.IsManaged(oSC_ManualEntry.TeamId, user_name, role)) return HttpNotFound();
            oSC_ManualEntry.Team = db.Teams.Find(oSC_ManualEntry.TeamId);
            oSC_ManualEntry.Representative = db.Representatives.Find(oSC_ManualEntry.RepId);
            oSC_ManualEntry.Representative.FullName = oSC_ManualEntry.Representative.FirstName + " " + oSC_ManualEntry.Representative.LastName;
            oSC_ManualEntry.PeriodCoverage = Convert.ToDateTime(oSC_ManualEntry.Month + "/1/" + oSC_ManualEntry.Year);
            #endregion "Method"
            #region "Return"
            return View(oSC_ManualEntry);
            #endregion "Return"
        }

        // POST: ManualEntry/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EntryId,TeamId,RepId,GainLossOccurances,GainLossAmount,CallManagementScore,ProjectResponsibility,ScheduleAdherence,Compliance,ProductAccuracy,Commitment,JHValues,CallEfficiency,Engagement,AdministrativeProcedures,Month,Year,DateUploaded,UploadedBy,IsActive,ActiveProjects,CompletedProjects")] OSC_ManualEntry oSC_ManualEntry)
        {
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "Manual Entries");
                if (!ViewBag.CanEdit) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "AddValues"
            oSC_ManualEntry.Month = Convert.ToDateTime(oSC_ManualEntry.PeriodCoverage).Month;
            oSC_ManualEntry.Year = Convert.ToDateTime(oSC_ManualEntry.PeriodCoverage).Year;
            oSC_ManualEntry.DateUploaded = DateTime.Now;
            oSC_ManualEntry.UploadedBy = user_name;
            if (Session["role"].ToString() != "Admin") oSC_ManualEntry.IsActive = true;
            #endregion "AddValues"
            #region "Method"
            if (ModelState.IsValid)
            {
                db.Entry(oSC_ManualEntry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            #endregion "Method"
            #region "Return"
            return View(oSC_ManualEntry);
            #endregion "Return"
        }

        // GET: ManualEntry/Delete/5
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
                ViewBag.CanDelete = db.appFacade.CanDelete(grp_id, "Manual Entries");
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
            OSC_ManualEntry oSC_ManualEntry = db.ManualEntries.Find(id);
            if (oSC_ManualEntry == null) return HttpNotFound();
            if (!db.IsManaged(oSC_ManualEntry.TeamId, user_name, role)) return HttpNotFound();
            oSC_ManualEntry.Team = db.Teams.Find(oSC_ManualEntry.TeamId);
            oSC_ManualEntry.Representative = db.Representatives.Find(oSC_ManualEntry.RepId);
            oSC_ManualEntry.Representative.FullName = oSC_ManualEntry.Representative.FirstName + " " + oSC_ManualEntry.Representative.LastName;
            oSC_ManualEntry.PeriodCoverage = Convert.ToDateTime(oSC_ManualEntry.Month + "/1/" + oSC_ManualEntry.Year);
            #endregion "Method"
            #region "Return"
            return View(oSC_ManualEntry);
            #endregion "Return"
        }

        // POST: ManualEntry/Delete/5
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
                ViewBag.CanDelete = db.appFacade.CanDelete(grp_id, "Manual Entries");
                if (!ViewBag.CanDelete) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "AddValues"
            OSC_ManualEntry oSC_ManualEntry = db.ManualEntries.Find(id);
            if (oSC_ManualEntry == null) return HttpNotFound();
            oSC_ManualEntry.DateUploaded = DateTime.Now;
            oSC_ManualEntry.UploadedBy = user_name;
            oSC_ManualEntry.IsActive = false;
            #endregion "AddValues"
            #region "Method"
            if (ModelState.IsValid)
            {
                db.Entry(oSC_ManualEntry).State = EntityState.Modified;
                db.SaveChanges();
            }
            #endregion "Method"
            #region "Return"
            return RedirectToAction("Index");
            #endregion "Return"
            //old delete method
            //OSC_ManualEntry oSC_ManualEntry = db.ManualEntries.Find(id);
            //db.ManualEntries.Remove(oSC_ManualEntry);
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
