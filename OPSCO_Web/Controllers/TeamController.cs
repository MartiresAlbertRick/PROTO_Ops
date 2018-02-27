using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OPSCO_Web.Models;
using OPSCO_Web.BL;
using PagedList;
using PagedList.Mvc;

namespace OPSCO_Web.Controllers
{
    public class TeamController : Controller
    {
        #region "ContextInitializer"
        private OSCContext db = new OSCContext();
        private AppFacade af = new AppFacade();
        #endregion "ContextInitializer"
        #region "Index"
        // GET: Team
        public ActionResult Index(int? page, int? pageSize, string searchString)
        {
            #region "Initialize"
            af.InitializeTeams(db);
            #endregion "Initialize"
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanView = af.CanView(grp_id, "Team");
                ViewBag.CanAdd = af.CanAdd(grp_id, "Team");
                ViewBag.CanEdit = af.CanEdit(grp_id, "Team");
                ViewBag.CanDelete = af.CanDelete(grp_id, "Team");

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
                        if (af.IsManaged(obj.TeamId, user_name, role))
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
                ViewBag.CanView = af.CanView(grp_id, "Team");
                ViewBag.CanEdit = af.CanEdit(grp_id, "Team");
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
            if (!af.IsManaged(oSC_Team.TeamId, user_name, role)) return HttpNotFound();
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
                ViewBag.CanAdd = af.CanAdd(grp_id, "Team");
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
                ViewBag.CanAdd = af.CanAdd(grp_id, "Team");
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
                ViewBag.CanEdit = af.CanEdit(grp_id, "Team");
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
            oSC_Team.GroupIds = af.GetGroupIds(id);
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
                ViewBag.CanEdit = af.CanEdit(grp_id, "Team");
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
                ViewBag.CanDelete = af.CanDelete(grp_id, "Team");
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
            if (!af.IsManaged(oSC_Team.TeamId, user_name, role)) return HttpNotFound();
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
                ViewBag.CanDelete = af.CanDelete(grp_id, "Team");
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
            if (oSC_Team == null) return HttpNotFound();
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

        public JsonResult SaveGroupIds(long? id, List<OSC_TeamGroupIds> objects)
        {
            object s = new { type = "failed", message = "Saving failed!" };
            if (id == null) return Json(s, JsonRequestBehavior.AllowGet);
            try
            {
                List<OSC_TeamGroupIds> list = db.TeamGroupIds.AsNoTracking().Where(t => t.TeamId == id).ToList();
                foreach (OSC_TeamGroupIds obj in list)
                {
                    if (objects.Where(t => t.TGIId == obj.TGIId && t.TGIId != 0).FirstOrDefault() == null)
                    {
                        if (ModelState.IsValid)
                        {
                            OSC_TeamGroupIds oSC_TeamGroupIds = db.TeamGroupIds.Find(obj.TGIId);
                            db.TeamGroupIds.Remove(oSC_TeamGroupIds);
                            db.SaveChanges();
                        }
                    }
                }
                foreach (OSC_TeamGroupIds obj in objects)
                {
                    if (ModelState.IsValid)
                    {
                        if (obj.TGIId == 0)
                        {
                            db.TeamGroupIds.Add(obj);
                            db.SaveChanges();
                        }
                        else
                        {
                            db.Entry(obj).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
                s = new { type = "success", message = "Successfully saved!" };
            }
            catch (Exception ex)
            {
                s = new { type = "failed", message = "Saving failed!\nError occured:\n" + ex.Message.ToString() };
            }
            return Json(s, JsonRequestBehavior.AllowGet);
        }
        #endregion "GroupIdSection"
        #region "TimingsSection"
        public PartialViewResult TimingsSection(long? id)
        {
            ViewBag.TeamId = id;
            ViewBag.Years = db.years;
            ViewBag.BusinessAreas = db.BusinessAreas;
            ViewBag.WorkTypes = db.WorkTypes;
            ViewBag.Status = db.Statuses;
            return PartialView();
        }
        public JsonResult GetTimings(long? id, int? year)
        {
            if (id == null) return Json(null);
            if (year == null || year == 0 || year.ToString() == "") return Json(null);
            var timings = (from list in db.TeamWorkItems where list.TeamId == (long)id && list.Year == (int)year select list).ToList();
            if (timings.Count == 0 || timings == null) timings = (from list in db.TeamWorkItems where list.TeamId == (long)id && list.Year == ((int)year - 1) select list).ToList();
            if (timings.Count == 0 || timings == null) timings = (from list in db.TeamWorkItems where list.TeamId == (long)id && list.Year == ((int)year + 1) select list).ToList();
            return Json(timings, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveTimings(long? id, List<OSC_TeamWorkItem> objects)
        {
            object s = new { type = "failed", message = "Saving failed!" };
            if (id == null) return Json(s, JsonRequestBehavior.AllowGet);
            try
            {
                List<OSC_TeamWorkItem> list = db.TeamWorkItems.AsNoTracking().Where(t => t.TeamId == id).ToList();
                foreach (OSC_TeamWorkItem obj in list)
                {
                    if (objects.Where(t => t.WorkItemNo == obj.WorkItemNo && t.WorkItemNo != 0).FirstOrDefault() == null)
                    {
                        if (ModelState.IsValid)
                        {
                            OSC_TeamWorkItem oSC_TeamWorkItem = db.TeamWorkItems.Find(obj.WorkItemNo);
                            db.TeamWorkItems.Remove(oSC_TeamWorkItem);
                            db.SaveChanges();
                        }
                    }
                }
                foreach (OSC_TeamWorkItem obj in objects)
                {
                    if (ModelState.IsValid)
                    {
                        if (obj.WorkItemNo == 0)
                        {
                            db.TeamWorkItems.Add(obj);
                            db.SaveChanges();
                        }
                        else
                        {
                            if (list.Where(t => t.WorkItemNo == obj.WorkItemNo && t.Year == obj.Year).FirstOrDefault() == null)
                            {
                                db.Entry(obj).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                db.TeamWorkItems.Add(obj);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                s = new { type = "success", message = "Successfully saved!" };
            }
            catch (Exception ex)
            {
                s = new { type = "failed", message = "Saving failed!\nError occured:\n" + ex.Message.ToString() };
            }
            return Json(s, JsonRequestBehavior.AllowGet);
        }
        #endregion "TimingsSection"
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

        public JsonResult SaveCategories(long? id, List<OSC_TeamNptCategory> objects)
        {
            object s = new { type = "failed", message = "Saving failed!" };
            if (id == null) return Json(s, JsonRequestBehavior.AllowGet);
            try
            {
                var list = db.TeamNptCategories.Where(t => t.TeamId == id).ToList();
                foreach (OSC_TeamNptCategory obj in list)
                {
                    if (ModelState.IsValid)
                    {
                        db.TeamNptCategories.Remove(obj);
                        db.SaveChanges();
                    }
                }
                foreach (OSC_TeamNptCategory obj in objects)
                {
                    if (ModelState.IsValid)
                    {
                        db.TeamNptCategories.Add(obj);
                        db.SaveChanges();
                    }
                }
                s = new { type = "success", message = "Successfully saved!" };
            }
            catch (Exception ex)
            {
                s = new { type = "failed", message = "Saving failed!\nError occured:\n" + ex.Message.ToString() };
            }
            return Json(s, JsonRequestBehavior.AllowGet);
        }
        #endregion "NptCategorySection"
        #region "CustomizeScorecardSection"
        public PartialViewResult CustomizeScorecardSection(long? id)
        {
            ViewBag.TeamId = id;
            ViewBag.Years = db.years;
            ViewBag.Months = db.months;
            ViewBag.Scorecards = db.scorecardOptions;
            return PartialView();
        }

        public JsonResult GetScorecardFields(long? id, int? month, int? year, string view)
        {
            if (id == null) return Json(null);
            if (month == null || month == 0 || month.ToString() == "") return Json(null);
            if (year == null || year == 0 || year.ToString() == "") return Json(null);
            if (view == "") return Json(null);
            List<int> viewId = new List<int>();
            //viewId = (from list in db.CustomizeScorecards.OrderBy(t => t.Order) where list.TeamId == (long)id && list.Month == (int)month && list.Year == (int)year select list.FieldId).ToList();
            viewId = (from list in db.CustomizeScorecards.OrderBy(t => t.Order) where list.TeamId == (long)id && list.Month == (int)month && list.Year == (int)year && list.ScorecardType==view select list.FieldId).ToList();
            if (viewId == null || viewId.Count == 0)
            {
                //if (month > 1) viewId = (from list in db.CustomizeScorecards.OrderBy(t => t.Order) where list.TeamId == (long)id && list.Month == ((int)month - 1) && list.Year == (int)year select list.FieldId).ToList();
                //else viewId = (from list in db.CustomizeScorecards.OrderBy(t => t.Order) where list.TeamId == (long)id && list.Month == 12 && list.Year == ((int)year - 1) select list.FieldId).ToList();
                if (month > 1) viewId = (from list in db.CustomizeScorecards.OrderBy(t => t.Order) where list.TeamId == (long)id && list.Month == ((int)month - 1) && list.Year == (int)year && list.ScorecardType==view select list.FieldId).ToList();
                else viewId = (from list in db.CustomizeScorecards.OrderBy(t => t.Order) where list.TeamId == (long)id && list.Month == 12 && list.Year == ((int)year - 1) && list.ScorecardType==view select list.FieldId).ToList();
            }
            List<OSC_ScorecardField> scorecardFields = new List<OSC_ScorecardField>();
            if (viewId == null || viewId.Count == 0)
            {
                foreach (OSC_ScorecardField item in db.ScorecardFields.Where(t => (bool)t.IsActive && (bool)t.IsCore == false))
                {
                    scorecardFields.Add(item);
                }
            }
            else
            {
                foreach (OSC_ScorecardField item in db.ScorecardFields.Where(t => !viewId.Contains(t.FieldId) && (bool)t.IsActive))
                {
                    scorecardFields.Add(item);
                }
            }
            return Json(scorecardFields, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetScorecardView(long? id, int? month, int? year, string view)
        {
            if (id == null) return Json(null);
            if (month == null || month == 0 || month.ToString() == "") return Json(null);
            if (year == null || year == 0 || year.ToString() == "") return Json(null);
            if (view == "") return Json(null);
            List<int> viewId = new List<int>();
            //viewId = (from list in db.CustomizeScorecards.OrderBy(t => t.Order) where list.TeamId == (long)id && list.Month == (int)month && list.Year == (int)year select list.FieldId).ToList();
            viewId = (from list in db.CustomizeScorecards.OrderBy(t => t.Order) where list.TeamId == (long)id && list.Month == (int)month && list.Year == (int)year && list.ScorecardType==view select list.FieldId).ToList();
            if (viewId == null || viewId.Count == 0)
            {
                //if (month > 1) viewId = (from list in db.CustomizeScorecards.OrderBy(t => t.Order) where list.TeamId == (long)id && list.Month == ((int)month - 1) && list.Year == (int)year select list.FieldId).ToList();
                //else viewId = (from list in db.CustomizeScorecards.OrderBy(t => t.Order) where list.TeamId == (long)id && list.Month == 12 && list.Year == ((int)year - 1) select list.FieldId).ToList();
                if (month > 1) viewId = (from list in db.CustomizeScorecards.OrderBy(t => t.Order) where list.TeamId == (long)id && list.Month == ((int)month - 1) && list.Year == (int)year && list.ScorecardType==view select list.FieldId).ToList();
                else viewId = (from list in db.CustomizeScorecards.OrderBy(t => t.Order) where list.TeamId == (long)id && list.Month == 12 && list.Year == ((int)year - 1) && list.ScorecardType==view select list.FieldId).ToList();
            }
            List<OSC_ScorecardField> scorecardFields = new List<OSC_ScorecardField>();
            if (viewId == null || viewId.Count == 0)
            {
                //foreach (OSC_ScorecardField item in db.ScorecardFields.Where(t => (bool)t.IsActive && (bool)t.IsCore && t.ScorecardType==view))
                foreach (OSC_ScorecardField item in db.ScorecardFields.Where(t => (bool)t.IsActive && (bool)t.IsCore))
                {
                    scorecardFields.Add(item);
                }
            }
            else
            {
                foreach (int i in viewId)
                {
                    OSC_ScorecardField item = db.ScorecardFields.Find(i);
                    //if ((bool)item.IsActive && t.ScorecardType==view)
                    if ((bool)item.IsActive)
                    {
                        scorecardFields.Add(item);
                    }
                }
            }
            return Json(scorecardFields, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveScorecardView(long? id, int? month, int? year, string view, List<OSC_CustomizeScorecard> objects)
        {
            object s = new { type = "failed", message = "Saving failed!" };
            if (id == null) return Json(s, JsonRequestBehavior.AllowGet);
            try
            {
                var list = db.CustomizeScorecards.Where(t => t.TeamId == (long)id && t.Year == (int)year && t.Month == (int)month).ToList();
                //var list = db.CustomizeScorecards.Where(t => t.TeamId == (long)id && t.Year == (int)year && t.Month == (int)month && t.ScorecardView==view).ToList();
                foreach (OSC_CustomizeScorecard obj in list)
                {
                    if (ModelState.IsValid)
                    {
                        db.CustomizeScorecards.Remove(obj);
                        db.SaveChanges();
                    }
                }
                foreach (OSC_CustomizeScorecard obj in objects)
                {
                    if (ModelState.IsValid)
                    {
                        db.CustomizeScorecards.Add(obj);
                        db.SaveChanges();
                    }
                }
                s = new { type = "success", message = "Successfully saved!" };
            }
            catch (Exception ex)
            {
                s = new { type = "failed", message = "Saving failed!\nError occured:\n" + ex.Message.ToString() };
            }
            return Json(s, JsonRequestBehavior.AllowGet);
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
