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
            #region "Initialize"
            db.InitializeTeams();
            #endregion "Initialize"
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanView = db.appFacade.CanView(grp_id, "Team");
                ViewBag.CanAdd = db.appFacade.CanAdd(grp_id, "Team");
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "Team");
                ViewBag.CanDelete = db.appFacade.CanDelete(grp_id, "Team");

                if (!ViewBag.CanView) return HttpNotFound();
            }
            catch (Exception exception)
            {
                //session expired
                string result = exception.Message.ToString();
                return HttpNotFound();
            }

            var teams = (from t in db.Teams
                         select t);
            List<long> TeamIds = new List<long>();
            switch (role)
            {
                case "Manager":
                case "Team Leader":
                case "Department Analyst":
                case "Staff":
                    foreach (OSC_Team obj in teams)
                    {
                        if (db.IsManaged(obj.TeamId, user_name, role))
                            if (!TeamIds.Contains(obj.TeamId))
                                TeamIds.Add(obj.TeamId);
                    }
                    teams = (from t in db.Teams
                            where TeamIds.Contains(t.TeamId) && t.IsActive
                            select t);
                    break;
            }
            #endregion "BTSS"
            #region "Table"
            int? defaultPageSize = 10;
            if (pageSize != null) defaultPageSize = pageSize;
            if (!String.IsNullOrEmpty(searchString)) teams = teams.Where(t => t.TeamName.Contains(searchString));
            #endregion "Table"
            #region "Return"
            return View(teams.OrderBy(t => t.DepartmentId).ThenBy(t => t.TeamName).ToPagedList(page ?? 1, (int)defaultPageSize));
            #endregion "Return"

        }

        // GET: Team/Details/5
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
                ViewBag.CanView = db.appFacade.CanView(grp_id, "Team");
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "Team");
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
            OSC_Team oSC_Team = db.Teams.Find(id);
            if (oSC_Team == null) return HttpNotFound();
            if (!db.IsManaged(oSC_Team.TeamId, user_name, role)) return HttpNotFound();
            oSC_Team.Department = db.Departments.Find(oSC_Team.DepartmentId);
            #endregion "Method"
            #region "Return"
            return View(oSC_Team);
            #endregion "Return"
        }

        // GET: Team/Create
        public ActionResult Create()
        {
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanAdd = db.appFacade.CanAdd(grp_id, "Team");
                if (!ViewBag.CanAdd) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "ViewBagDepartments"
            ViewBag.Departments = new SelectList(db.Departments, "DepartmentId", "DepartmentName");
            #endregion "ViewBagDepartments"
            #region "Return"
            return View();
            #endregion "Return"
        }

        // POST: Team/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TeamId,TeamName,DepartmentId,IsActive,BIUserGroup,AIQUserGroup")] OSC_Team oSC_Team)
        {
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanAdd = db.appFacade.CanAdd(grp_id, "Team");
                if (!ViewBag.CanAdd) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "AddValues"
            oSC_Team.IsActive = true;
            #endregion "AddValues"
            #region "Method"
            if (ModelState.IsValid)
            {
                db.Teams.Add(oSC_Team);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            #endregion "Method"
            #region "Return"
            return View(oSC_Team);
            #endregion "Return"
        }

        // GET: Team/Edit/5
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
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "Team");
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
            OSC_Team oSC_Team = db.Teams.Find(id);
            if (oSC_Team == null) return HttpNotFound();
            oSC_Team.GroupIds = db.GetGroupIds(id);
            #endregion "Method"
            #region "ViewBagDepartments"
            ViewBag.Departments = new SelectList(db.Departments, "DepartmentId", "DepartmentName");
            #endregion "ViewBagDepartments"
            #region "Return"
            return View(oSC_Team);
            #endregion "Return"
        }

        // POST: Team/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeamId,TeamName,DepartmentId,IsActive,BIUserGroup,AIQUserGroup")] OSC_Team oSC_Team)
        {
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "Team");
                if (!ViewBag.CanEdit) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "AddValues"
            if (Session["role"].ToString() != "Admin") oSC_Team.IsActive = true;
            #endregion "AddValues"
            #region "Method"
            if (ModelState.IsValid)
            {
                db.Entry(oSC_Team).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            #endregion "Method"
            #region "Return"
            return View(oSC_Team);
            #endregion "Return"
        }

        // GET: Team/Delete/5
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
                ViewBag.CanDelete = db.appFacade.CanDelete(grp_id, "Team");
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
            OSC_Team oSC_Team = db.Teams.Find(id);
            if (oSC_Team == null) return HttpNotFound();
            if (!db.IsManaged(oSC_Team.TeamId, user_name, role)) return HttpNotFound();
            oSC_Team.Department = db.Departments.Find(oSC_Team.DepartmentId);
            #endregion "Method"
            #region "Return"
            return View(oSC_Team);
            #endregion "Return"
        }

        // POST: Team/Delete/5
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
                ViewBag.CanDelete = db.appFacade.CanDelete(grp_id, "Team");
                if (!ViewBag.CanDelete) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "AddValues"
            OSC_Team oSC_Team = db.Teams.Find(id);
            if (oSC_Team == null) return HttpNotFound();
            oSC_Team.IsActive = false;
            #endregion "AddValues"
            #region "Method"
            if (ModelState.IsValid)
            {
                db.Entry(oSC_Team).State = EntityState.Modified;
                db.SaveChanges();
            }
            #endregion "Method"
            #region "Return"
            return RedirectToAction("Index");
            #endregion "Return
            //old delete method
            //OSC_Team oSC_Team = db.Teams.Find(id);
            //db.Teams.Remove(oSC_Team);
            //db.SaveChanges();
            //return RedirectToAction("Index");
        }

        public ActionResult Settings(long? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
