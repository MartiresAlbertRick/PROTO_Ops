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
    public class TeamController : Controller
    {
        private OSCContext db = new OSCContext();

        // GET: Team
        public ActionResult Index(int? page, int? pageSize, string searchString)
        {
            foreach (OSC_Team team in db.Teams)
            {
                team.Department = db.Departments.Find(team.DepartmentId);
            }

            int? defaultPageSize = 10;

            if (pageSize != null)
            {
                defaultPageSize = pageSize;
            }

            var teams = (from t in db.Teams
                        select t);

            if (!String.IsNullOrEmpty(searchString))
            {
                teams = teams.Where(t => t.TeamName.Contains(searchString));
            }

            return View(teams.OrderBy(t => t.DepartmentId).ThenBy(t => t.TeamName).ToPagedList(page ?? 1, (int)defaultPageSize));
            //return View(db.Teams.ToList());
        }

        // GET: Team/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_Team oSC_Team = db.Teams.Find(id);
            oSC_Team.Department = db.Departments.Find(oSC_Team.DepartmentId);
            if (oSC_Team == null)
            {
                return HttpNotFound();
            }
            return View(oSC_Team);
        }

        // GET: Team/Create
        public ActionResult Create()
        {
            ViewBag.Departments = new SelectList(db.Departments, "DepartmentId", "DepartmentName");
            return View();
        }

        // POST: Team/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TeamId,TeamName,DepartmentId,IsActive,BIUserGroup,AIQUserGroup")] OSC_Team oSC_Team)
        {
            if (ModelState.IsValid)
            {
                db.Teams.Add(oSC_Team);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(oSC_Team);
        }

        // GET: Team/Edit/5
        public ActionResult Edit(long? id)
        {
            ViewBag.Departments = new SelectList(db.Departments, "DepartmentId", "DepartmentName");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_Team oSC_Team = db.Teams.Find(id);
            oSC_Team.GroupIds = db.GetGroupIds(id);
            if (oSC_Team == null)
            {
                return HttpNotFound();
            }
            return View(oSC_Team);
        }

        // POST: Team/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeamId,TeamName,DepartmentId,IsActive,BIUserGroup,AIQUserGroup")] OSC_Team oSC_Team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oSC_Team).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oSC_Team);
        }

        // GET: Team/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_Team oSC_Team = db.Teams.Find(id);
            oSC_Team.Department = db.Departments.Find(oSC_Team.DepartmentId);
            if (oSC_Team == null)
            {
                return HttpNotFound();
            }
            return View(oSC_Team);
        }

        // POST: Team/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            OSC_Team oSC_Team = db.Teams.Find(id);
            db.Teams.Remove(oSC_Team);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Settings(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_Team oSC_Team = db.Teams.Find(id);
            oSC_Team.Department = db.Departments.Find(oSC_Team.DepartmentId);
            if (oSC_Team == null)
            {
                return HttpNotFound();
            }
            return View(oSC_Team);
        }

        [HttpPost]
        public PartialViewResult _SaveGroupId([Bind(Include = "TeamId,GroupId,GroupType")]OSC_TeamGroupIds oSC_TeamGroupIds, long? teamId)
        {
            oSC_TeamGroupIds.TeamId = (long)teamId;
            db.TeamGroupIds.Add(oSC_TeamGroupIds);
            db.SaveChanges();
            ViewBag.TeamId = teamId;
            ViewBag.GroupTypes = db.userGroupType;
            return PartialView("_GroupIdSection", db.TeamGroupIds.Where(t => t.TeamId == teamId));
        }

        public PartialViewResult _GroupIdSection(long? id)
        {
            ViewBag.TeamId = id;
            ViewBag.GroupTypes = db.userGroupType;
            return PartialView(db.TeamGroupIds.Where(t => t.TeamId == id));
        }

        public PartialViewResult _GroupIdRow(OSC_TeamGroupIds oSC_TeamGroupIds)
        {
            ViewBag.GroupTypes = db.userGroupType;
            return PartialView("_GroupIdRow", oSC_TeamGroupIds);
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
