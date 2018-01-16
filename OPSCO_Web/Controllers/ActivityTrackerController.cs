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
    public class ActivityTrackerController : Controller
    {
        private OSCContext db = new OSCContext();

        // GET: ActivityTracker
        public ActionResult Index()
        {
            return View(db.ActivityTrackers.ToList());
        }

        // GET: ActivityTracker/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_ActivityTracker oSC_ActivityTracker = db.ActivityTrackers.Find(id);
            if (oSC_ActivityTracker == null)
            {
                return HttpNotFound();
            }
            return View(oSC_ActivityTracker);
        }

        // GET: ActivityTracker/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ActivityTracker/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ActivityId,RepId,Month,Year,DateFrom,DateTo,Activity,NoOfHours,DateModified,ModifiedBy,NoOfDays,TeamId,IsActive")] OSC_ActivityTracker oSC_ActivityTracker)
        {
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
        public ActionResult Edit([Bind(Include = "ActivityId,RepId,Month,Year,DateFrom,DateTo,Activity,NoOfHours,DateModified,ModifiedBy,NoOfDays,TeamId,IsActive")] OSC_ActivityTracker oSC_ActivityTracker)
        {
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
