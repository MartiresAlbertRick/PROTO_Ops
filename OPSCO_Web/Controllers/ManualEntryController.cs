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
            foreach (OSC_ManualEntry me in db.ManualEntries)
            {
                me.MonthName = db.months.Where(m => m.Value == Convert.ToString(me.Month)).First().Text;
                me.Team = db.Teams.Find(me.TeamId);
                me.Representative = db.Representatives.Find(me.RepId);
                me.Representative.Location = db.Locations.Find(me.Representative.LocationId);
                me.Representative.CoreRole = db.CoreRoles.Find(me.Representative.CoreRoleId);
                me.Representative.FullName = me.Representative.FirstName + " " + me.Representative.LastName;
            }
            int? defaultPageSize = 10;

            if (pageSize != null)
            {
                defaultPageSize = pageSize;
            }

            var mes = (from m in db.ManualEntries
                       select m);

            if (!String.IsNullOrEmpty(searchString))
            {
                mes = mes.Where(m => m.MonthName.Contains(searchString) || m.Year.ToString().Contains(searchString));
            }

            return View(mes.OrderByDescending(m => m.Year).ThenByDescending(m => m.Month).ThenByDescending(m => m.TeamId).ToPagedList(page ?? 1, (int)defaultPageSize));
        }

        // GET: ManualEntry/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_ManualEntry oSC_ManualEntry = db.ManualEntries.Find(id);
            oSC_ManualEntry.Team = db.Teams.Find(oSC_ManualEntry.TeamId);
            oSC_ManualEntry.Representative = db.Representatives.Find(oSC_ManualEntry.RepId);
            oSC_ManualEntry.Representative.FullName = oSC_ManualEntry.Representative.FirstName + " " + oSC_ManualEntry.Representative.LastName;
            oSC_ManualEntry.PeriodCoverage = Convert.ToDateTime(oSC_ManualEntry.Month + "/1/" + oSC_ManualEntry.Year);
            if (oSC_ManualEntry == null)
            {
                return HttpNotFound();
            }
            return View(oSC_ManualEntry);
        }

        // GET: ManualEntry/Create
        public ActionResult Create(long? teamId)
        {
            foreach (OSC_Representative rep in db.Representatives)
            { rep.FullName = rep.FirstName + " " + rep.LastName; }
            ViewBag.Teams = new SelectList(db.Teams, "TeamId", "TeamName");
            ViewBag.ProjectResponsibilities = db.projectResponsibilities;
            long defaultTeamId = 0;
            var reps = (from r in db.Representatives select r);
            if (teamId != null)
            { reps = reps.Where(r => r.TeamId == teamId); }
            else
            { reps = reps.Where(r => r.TeamId == defaultTeamId); }
            ViewBag.Representatives = new SelectList(reps, "RepId", "FullName");
            return View();
        }

        // POST: ManualEntry/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EntryId,TeamId,RepId,GainLossOccurances,GainLossAmount,CallManagementScore,ProjectResponsibility,ScheduleAdherence,Compliance,ProductAccuracy,Commitment,JHValues,CallEfficiency,Engagement,AdministrativeProcedures,Month,Year,DateUploaded,UploadedBy,IsActive,ActiveProjects,CompletedProjects,PeriodCoverage")] OSC_ManualEntry oSC_ManualEntry)
        {
            oSC_ManualEntry.Month = Convert.ToDateTime(oSC_ManualEntry.PeriodCoverage).Month;
            oSC_ManualEntry.Year = Convert.ToDateTime(oSC_ManualEntry.PeriodCoverage).Year;
            oSC_ManualEntry.DateUploaded = DateTime.Now;
            oSC_ManualEntry.UploadedBy = "svcBizTech";
            if (ModelState.IsValid)
            {
                db.ManualEntries.Add(oSC_ManualEntry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(oSC_ManualEntry);
        }

        // GET: ManualEntry/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_ManualEntry oSC_ManualEntry = db.ManualEntries.Find(id);
            oSC_ManualEntry.Team = db.Teams.Find(oSC_ManualEntry.TeamId);
            oSC_ManualEntry.Representative = db.Representatives.Find(oSC_ManualEntry.RepId);
            oSC_ManualEntry.Representative.FullName = oSC_ManualEntry.Representative.FirstName + " " + oSC_ManualEntry.Representative.LastName;
            ViewBag.ProjectResponsibilities = db.projectResponsibilities;
            oSC_ManualEntry.PeriodCoverage = Convert.ToDateTime(oSC_ManualEntry.Month + "/1/" + oSC_ManualEntry.Year);
            if (oSC_ManualEntry == null)
            {
                return HttpNotFound();
            }
            return View(oSC_ManualEntry);
        }

        // POST: ManualEntry/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EntryId,TeamId,RepId,GainLossOccurances,GainLossAmount,CallManagementScore,ProjectResponsibility,ScheduleAdherence,Compliance,ProductAccuracy,Commitment,JHValues,CallEfficiency,Engagement,AdministrativeProcedures,Month,Year,DateUploaded,UploadedBy,IsActive,ActiveProjects,CompletedProjects")] OSC_ManualEntry oSC_ManualEntry)
        {
            oSC_ManualEntry.Month = Convert.ToDateTime(oSC_ManualEntry.PeriodCoverage).Month;
            oSC_ManualEntry.Year = Convert.ToDateTime(oSC_ManualEntry.PeriodCoverage).Year;
            oSC_ManualEntry.DateUploaded = DateTime.Now;
            oSC_ManualEntry.UploadedBy = "svcBizTech";
            if (ModelState.IsValid)
            {
                db.Entry(oSC_ManualEntry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oSC_ManualEntry);
        }

        // GET: ManualEntry/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_ManualEntry oSC_ManualEntry = db.ManualEntries.Find(id);
            oSC_ManualEntry.Team = db.Teams.Find(oSC_ManualEntry.TeamId);
            oSC_ManualEntry.Representative = db.Representatives.Find(oSC_ManualEntry.RepId);
            oSC_ManualEntry.Representative.FullName = oSC_ManualEntry.Representative.FirstName + " " + oSC_ManualEntry.Representative.LastName;
            oSC_ManualEntry.PeriodCoverage = Convert.ToDateTime(oSC_ManualEntry.Month + "/1/" + oSC_ManualEntry.Year);
            if (oSC_ManualEntry == null)
            {
                return HttpNotFound();
            }
            return View(oSC_ManualEntry);
        }

        // POST: ManualEntry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            OSC_ManualEntry oSC_ManualEntry = db.ManualEntries.Find(id);
            db.ManualEntries.Remove(oSC_ManualEntry);
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
