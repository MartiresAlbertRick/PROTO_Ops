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

        #region "Index"
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
        #endregion "Index"
        #region "Details"
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
        #endregion "Details"
        #region "TeamMethodCreate"
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
        #endregion "TeamMethodCreate"
        #region "TeamMethodEdit"
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
        #endregion "TeamMethodEdit"
        #region "TeamMethodDelete"
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
        #endregion "TeamMethodDelete"
        #region "Settings"
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
        #endregion "Settings"
        #region "GroupIdSection"
        public PartialViewResult GroupIdSection(long? id)
        {
            ViewBag.TeamId = id;
            return PartialView();
        }
        
        public JsonResult GetTeamGroupIds(long? id)
        {
            if (id == null) return Json(null);
            var teamGroupIds = (from list in db.TeamGroupIds.Where(t => t.TeamId == id) select list).ToList();
            return Json(teamGroupIds, JsonRequestBehavior.AllowGet);
        }
        #endregion "GroupIdSection"

        public PartialViewResult TimingsSection(long? id)
        {
            ViewBag.TeamId = id;
            return PartialView();
        }

        #region "NptCategorySection"
        public PartialViewResult NptCategorySection(long? id)
        {
            ViewBag.TeamId = id;
            return PartialView();
        }

        public JsonResult GetNptCategories(long? id)
        {
            if (id == null) return Json(null);
            List<long> catId = (from list in db.TeamNptCategories where list.TeamId == (long)id select list.CategoryId).ToList();
            var nptCategories = (from list in db.NptCategories where (!catId.Contains(list.CategoryId) && list.IsActive) select list);
            return Json(nptCategories, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTeamNptCategories(long? id)
        {
            if (id == null) return Json(null);
            List<long> catId = (from list in db.TeamNptCategories where list.TeamId == (long)id select list.CategoryId).ToList();
            var nptCategories = (from list in db.NptCategories where (catId.Contains(list.CategoryId) && list.IsActive) select list);
            return Json(nptCategories, JsonRequestBehavior.AllowGet);
        }

        #endregion "NptCategorySection"

        #region "CustomizeScorecardSection"
        public PartialViewResult CustomizeScorecardSection(long? id)
        {
            ViewBag.TeamId = id;
            return PartialView();
        }

        public JsonResult GetScorecardFields(long? id)
        {
            if (id == null) return Json(null);
            List<int> viewId = (from list in db.CustomizeScorecards.OrderBy( t => t.Order) where list.TeamId == (long)id select list.FieldId).ToList();
            var scorecardFields = (from list in db.ScorecardFields.OrderBy( t => t.IsCore).ThenBy( t => t.FieldId) where (!viewId.Contains(list.FieldId)) && (bool)list.IsActive select list);
            return Json(scorecardFields, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetScorecardView(long? id)
        {
            if (id == null) return Json(null);
            List<int> viewId = (from list in db.CustomizeScorecards.OrderBy(t => t.Order) where list.TeamId == (long)id select list.FieldId).ToList();
            var scorecardFields = (from list in db.ScorecardFields.OrderBy( t => viewId.Contains(t.FieldId)) where (viewId.Contains(list.FieldId)) && (bool)list.IsActive select list);
            return Json(scorecardFields, JsonRequestBehavior.AllowGet);
        }
        #endregion "CustomizeScorecardSection"

        #region "Dispose"
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion "Dispose"
    }
}
