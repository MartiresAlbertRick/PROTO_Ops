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
    public class RepresentativeController : Controller
    {
        private OSCContext db = new OSCContext();

        // GET: Representative
        public ActionResult Index()
        {
            return View(db.Representatives.ToList());
        }

        // GET: Representative/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_Representative oSC_Representative = db.Representatives.Find(id);
            if (oSC_Representative == null)
            {
                return HttpNotFound();
            }
            return View(oSC_Representative);
        }

        // GET: Representative/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Representative/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RepId,PRDUserId,AIQUserId,BIUserId,WorkdayId,FirstName,MiddleName,LastName,TeamId,CoreRoleId,StartDate,EndDate,Comments,OnShoreRep,PhoneRep,WorkHours,LocationId,HasPrevious,PreviousId,IsCurrent,IsActive")] OSC_Representative oSC_Representative)
        {
            if (ModelState.IsValid)
            {
                db.Representatives.Add(oSC_Representative);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(oSC_Representative);
        }

        // GET: Representative/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_Representative oSC_Representative = db.Representatives.Find(id);
            if (oSC_Representative == null)
            {
                return HttpNotFound();
            }
            return View(oSC_Representative);
        }

        // POST: Representative/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RepId,PRDUserId,AIQUserId,BIUserId,WorkdayId,FirstName,MiddleName,LastName,TeamId,CoreRoleId,StartDate,EndDate,Comments,OnShoreRep,PhoneRep,WorkHours,LocationId,HasPrevious,PreviousId,IsCurrent,IsActive")] OSC_Representative oSC_Representative)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oSC_Representative).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oSC_Representative);
        }

        // GET: Representative/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_Representative oSC_Representative = db.Representatives.Find(id);
            if (oSC_Representative == null)
            {
                return HttpNotFound();
            }
            return View(oSC_Representative);
        }

        // POST: Representative/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            OSC_Representative oSC_Representative = db.Representatives.Find(id);
            db.Representatives.Remove(oSC_Representative);
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
