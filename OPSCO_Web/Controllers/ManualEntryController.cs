using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OPSCO_Web.Models;

namespace OPSCO_Web.Controllers
{
    public class ManualEntryController : Controller
    {
        private OSCContext db = new OSCContext();

        // GET: ManualEntry
        public ActionResult Index()
        {
            return View(db.ManualEntries.ToList());
        }

        // GET: ManualEntry/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_ManualEntry oSC_ManualEntry = db.ManualEntries.Find(id);
            if (oSC_ManualEntry == null)
            {
                return HttpNotFound();
            }
            return View(oSC_ManualEntry);
        }

        // GET: ManualEntry/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManualEntry/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EntryId,TeamId,RepId,GainLossOccurances,GainLossAmount,CallManagementScore,ProjectResponsibility,ScheduleAdherence,Compliance,ProductAccuracy,Commitment,JHValues,CallEfficiency,Engagement,AdministrativeProcedures,Month,Year,DateUploaded,UploadedBy,IsActive,ActiveProjects,CompletedProjects")] OSC_ManualEntry oSC_ManualEntry)
        {
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
