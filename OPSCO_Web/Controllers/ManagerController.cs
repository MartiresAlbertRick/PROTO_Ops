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
    public class ManagerController : Controller
    {
        #region "ContextInitializer"
        private OSCContext db = new OSCContext();
        #endregion "ContextInitializer"
        #region "Index"
        // GET: Manager
        public ActionResult Index(int? page, int? pageSize, string searchString)
        {
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanView = db.appFacade.CanView(grp_id, "Manager");
                ViewBag.CanAdd = db.appFacade.CanAdd(grp_id, "Manager");
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "Manager");
                ViewBag.CanDelete = db.appFacade.CanDelete(grp_id, "Manager");

                if (!ViewBag.CanView) return HttpNotFound();
            }
            catch (Exception exception)
            {
                //session expired
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "Table"
            int? defaultPageSize = 10;
            if (pageSize != null) defaultPageSize = pageSize;
            var managers = (from m in db.Managers select m);
            if (!String.IsNullOrEmpty(searchString)) managers = managers.Where(r => r.PRDUserId.Contains(searchString) || r.Name.Contains(searchString));
            #endregion "Table"
            #region "Return"
            return View(managers.OrderBy(r => r.Name).ToPagedList(page ?? 1, (int)defaultPageSize));
            #endregion "Return"
        }
        #endregion "Index"
        #region "Details"
        // GET: Manager/Details/5
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
                ViewBag.CanView = db.appFacade.CanView(grp_id, "Manager");
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "Manager");
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
            OSC_Manager oSC_Manager = db.Managers.Find(id);
            if (oSC_Manager == null) return HttpNotFound();
            #endregion "Method"
            #region "Return"
            return View(oSC_Manager);
            #endregion "Return"
        }
        #endregion "Details"
        #region "ManagerMethodCreate"
        // GET: Manager/Create
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
                ViewBag.CanAdd = db.appFacade.CanAdd(grp_id, "Manager");
                if (!ViewBag.CanAdd) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "Return"
            return View();
            #endregion "Return"
        }

        // POST: Manager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ManagerId,PRDUserId,Name,IsActive")] OSC_Manager oSC_Manager)
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
            #region "Method"
            if (ModelState.IsValid)
            {
                db.Managers.Add(oSC_Manager);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            #endregion "Method"
            #region "Return"
            return View(oSC_Manager);
            #endregion "Return"
        }
        #endregion "ManagerMethodCreate"
        #region "ManagerMethodEdit"
        // GET: Manager/Edit/5
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
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "Manager");
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
            OSC_Manager oSC_Manager = db.Managers.Find(id);
            if (oSC_Manager == null) return HttpNotFound();
            #endregion "Method"
            #region "Return"
            return View(oSC_Manager);
            #endregion "Return"
        }

        // POST: Manager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ManagerId,PRDUserId,Name,IsActive")] OSC_Manager oSC_Manager)
        {
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "Manager");
                if (!ViewBag.CanEdit) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "Method
            if (ModelState.IsValid)
            {
                db.Entry(oSC_Manager).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            #endregion "Method"
            #region "Return"
            return View(oSC_Manager);
            #endregion "Return"
        }
        #endregion "ManagerMethodEdit"
        #region "ManagerMethodDelete"
        // GET: Manager/Delete/5
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
                ViewBag.CanDelete = db.appFacade.CanDelete(grp_id, "Manager");
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
            OSC_Manager oSC_Manager = db.Managers.Find(id);
            if (oSC_Manager == null) return HttpNotFound();
            #endregion "Method"
            #region "Return"
            return View(oSC_Manager);
            #endregion "Return"
        }

        // POST: Manager/Delete/5
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
                ViewBag.CanDelete = db.appFacade.CanDelete(grp_id, "Manager");
                if (!ViewBag.CanDelete) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "Method"
            OSC_Manager oSC_Manager = db.Managers.Find(id);
            db.Managers.Remove(oSC_Manager);
            db.SaveChanges();
            #endregion "Method"
            #region "Return"
            return RedirectToAction("Index");
            #endregion "Return"
        }
        #endregion "ManagerMethodDelete"
        #region "Settings"
        public ActionResult Settings(long? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            OSC_Manager oSC_Manager = db.Managers.Find(id);
            if (oSC_Manager == null) return HttpNotFound();
            return View(oSC_Manager);
        }
        #endregion "Settings"
        #region "GroupsSection"
        public PartialViewResult GroupsSection(long? id)
        {
            ViewBag.ManagerId = id;
            ViewBag.Departments = db.Departments;
            ViewBag.Teams = db.Teams;
            return PartialView();
        }

        public JsonResult GetManagerGroups(long? id)
        {
            if (id == null) return Json(null);
            var groups = (from list in db.ManageGroups.Where(t => t.ManagerId == id) select list).ToList();
            return Json(groups, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveGroups(long? id, List<OSC_ManageGroup> objects)
        {
            object s = new { message = "Success" };
            if (id == null) return Json(null);
            List<OSC_ManageGroup> list = db.ManageGroups.AsNoTracking().Where(t => t.ManagerId == id).ToList();
            foreach (OSC_ManageGroup obj in list)
            {
                if (ModelState.IsValid)
                {
                    OSC_ManageGroup oSC_ManageGroup = db.ManageGroups.Find(obj.MGId);
                    db.ManageGroups.Remove(oSC_ManageGroup);
                    db.SaveChanges();
                }
            }
            foreach (OSC_ManageGroup obj in objects)
            {
                if (ModelState.IsValid)
                {
                    db.ManageGroups.Add(obj);
                    db.SaveChanges();
                }
            }
            return Json(s, JsonRequestBehavior.AllowGet);
        }
        #endregion "GroupsSection"
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
