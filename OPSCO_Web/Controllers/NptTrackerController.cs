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
    public class NptTrackerController : Controller
    {
        private OSCContext db = new OSCContext();

        // GET: NptTracker
        public ActionResult Index(int? page, int? pageSize, string searchString)
        {
            foreach (OSC_ImportNPT npt in db.NPT)
            {
                npt.Team = db.Teams.Find(npt.TeamId);
                npt.Representative = db.Representatives.Find(npt.RepId);
                npt.Representative.Location = db.Locations.Find(npt.Representative.LocationId);
                npt.Representative.CoreRole = db.CoreRoles.Find(npt.Representative.CoreRoleId);
                npt.Representative.FullName = npt.Representative.FirstName + " " + npt.Representative.LastName;
            }
            int? defaultPageSize = 10;

            if (pageSize != null)
            {
                defaultPageSize = pageSize;
            }

            var npts = (from n in db.NPT where n.Source == "Manual" select n);

            if (!String.IsNullOrEmpty(searchString))
            {
                npts = npts.Where(n => n.DateOfActivity.ToString().Contains(searchString) || n.TypeOfActivity.Contains(searchString));
            }

            return View(npts.OrderByDescending(n => n.DateOfActivity).ToPagedList(page ?? 1, (int)defaultPageSize));
        }

        // GET: NptTracker/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_ImportNPT oSC_ImportNPT = db.NPT.Find(id);
            if (oSC_ImportNPT == null)
            {
                return HttpNotFound();
            }
            return View(oSC_ImportNPT);
        }

        // GET: NptTracker/Create
        public ActionResult Create(long? teamId)
        {
            foreach (OSC_Representative rep in db.Representatives)
            { rep.FullName = rep.FirstName + " " + rep.LastName; }
            ViewBag.Teams = new SelectList(db.Teams, "TeamId", "TeamName");
            long defaultTeamId = 0;
            var reps = (from r in db.Representatives select r);
            var teamNptCategories = (from t in db.TeamNptCategories select t);
            if (teamId != null)
            {
                reps = reps.Where(r => r.TeamId == teamId);
                teamNptCategories = teamNptCategories.Where(t => t.TeamId == teamId);
            }
            else
            {
                reps = reps.Where(r => r.TeamId == defaultTeamId);
                teamNptCategories = teamNptCategories.Where(t => t.TeamId == defaultTeamId);
            }
            ViewBag.Representatives = new SelectList(reps, "RepId", "FullName");
            //ViewBag.TeamNptCategories = new SelectList();
            
            //View
            //Team
            //Representative
            //Category
            //ActivityDescription
            //DateOfActivity
            //TimeSpent
            //ActiveFlag

            return View();
        }

        // POST: NptTracker/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NPTReportId,RepId,Activity,DateOfActivity,TimeSpent,TypeOfActivity,CreatedBy,ItemType,Path,TeamId,Month,Year,DateUploaded,UploadedBy,Source,CategoryId,SubCategoryId,IsActive")] OSC_ImportNPT oSC_ImportNPT)
        {
            if (ModelState.IsValid)
            {
                db.NPT.Add(oSC_ImportNPT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(oSC_ImportNPT);
        }

        // GET: NptTracker/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_ImportNPT oSC_ImportNPT = db.NPT.Find(id);
            if (oSC_ImportNPT == null)
            {
                return HttpNotFound();
            }
            return View(oSC_ImportNPT);
        }

        // POST: NptTracker/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NPTReportId,RepId,Activity,DateOfActivity,TimeSpent,TypeOfActivity,CreatedBy,ItemType,Path,TeamId,Month,Year,DateUploaded,UploadedBy,Source,CategoryId,SubCategoryId,IsActive")] OSC_ImportNPT oSC_ImportNPT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oSC_ImportNPT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oSC_ImportNPT);
        }

        // GET: NptTracker/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_ImportNPT oSC_ImportNPT = db.NPT.Find(id);
            if (oSC_ImportNPT == null)
            {
                return HttpNotFound();
            }
            return View(oSC_ImportNPT);
        }

        // POST: NptTracker/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            OSC_ImportNPT oSC_ImportNPT = db.NPT.Find(id);
            db.NPT.Remove(oSC_ImportNPT);
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
