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
    public class DepartmentController : Controller
    {
        private OSCContext db = new OSCContext();
        private AppFacade af = new AppFacade();

        // GET: Department
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
                ViewBag.CanView = af.CanView(grp_id, "Department");
                ViewBag.CanAdd = af.CanAdd(grp_id, "Department");
                ViewBag.CanEdit = af.CanEdit(grp_id, "Department");
                ViewBag.CanDelete = af.CanDelete(grp_id, "Department");

                if (!ViewBag.CanView) return HttpNotFound();
            }
            catch (Exception exception)
            {
                //session expired
                string result = exception.Message.ToString();
                return HttpNotFound();
            }

            var departments = from d in db.Departments
                              select d;
            switch (role)
            {
                case "Manager":
                    List<long> depIds = new List<long>();
                    OSC_Manager mgr = db.Managers.Where(t => t.PRDUserId == user_name).FirstOrDefault();
                    if (mgr == null)
                    {
                        departments = (from d in db.Departments
                                       where d.DepartmentId == 0
                                       select d); ;
                    }
                    else
                    { 
                        foreach (OSC_ManageGroup obj in db.ManageGroups.Where(t => t.ManagerId == mgr.ManagerId))
                        {
                            if (obj.Type == "DEPT")
                            {
                                depIds.Add(obj.EntityId);
                            }
                        }
                        departments = (from d in db.Departments
                                    where depIds.Contains(d.DepartmentId) && d.IsActive
                                    select d);
                    }
                    break;
                case "Team Leader":
                case "Department Analyst":
                case "Staff":
                    departments = (from d in db.Departments
                                   where d.DepartmentId == 0
                                    select d);
                    break;
            }
            #endregion "BTSS"
            #region "Table"
            int? defaultPageSize = 10;
            if (pageSize != null) defaultPageSize = pageSize;
            if (!String.IsNullOrEmpty(searchString)) departments = departments.Where(d => d.DepartmentName.Contains(searchString));
            #endregion "Table"
            #region "Return"
            return View(departments.OrderBy(d => d.DepartmentName).ToPagedList(page ?? 1, (int)defaultPageSize));
            #endregion "Return"
        }

        // GET: Department/Details/5
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
                ViewBag.CanView = af.CanView(grp_id, "Department");
                ViewBag.CanEdit = af.CanEdit(grp_id, "Department");
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
            OSC_Department oSC_Department = db.Departments.Find(id);
            if (oSC_Department == null) return HttpNotFound();
            #endregion "Method"
            #region "Return"
            return View(oSC_Department);
            #endregion "Return"
        }

        // GET: Department/Create
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
                ViewBag.CanAdd = af.CanAdd(grp_id, "Department");
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

        // POST: Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DepartmentId,DepartmentName,IsActive")] OSC_Department oSC_Department)
        {
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanAdd = af.CanAdd(grp_id, "Department");
                if (!ViewBag.CanAdd) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "AddValues"
            oSC_Department.IsActive = true;
            #endregion "AddValues"
            #region "Method"
            if (ModelState.IsValid)
            {
                db.Departments.Add(oSC_Department);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            #endregion "Method"
            #region "Return"
            return View(oSC_Department);
            #endregion "Return"
        }

        // GET: Department/Edit/5
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
                ViewBag.CanEdit = af.CanEdit(grp_id, "Department");
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
            OSC_Department oSC_Department = db.Departments.Find(id);
            if (oSC_Department == null) return HttpNotFound();
            #endregion "Method"
            #region "Return"
            return View(oSC_Department);
            #endregion "Return"
        }

        // POST: Department/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DepartmentId,DepartmentName,IsActive")] OSC_Department oSC_Department)
        {
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanEdit = af.CanEdit(grp_id, "Department");
                if (!ViewBag.CanEdit) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "AddValues"
            if (Session["role"].ToString() != "Admin") oSC_Department.IsActive = true;
            #endregion "AddValues"
            #region "Method"
            if (ModelState.IsValid)
            {
                db.Entry(oSC_Department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            #endregion "Method"
            #region "Return"
            return View(oSC_Department);
            #endregion "Return"
        }

        // GET: Department/Delete/5
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
                ViewBag.CanDelete = af.CanDelete(grp_id, "Department");
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
            OSC_Department oSC_Department = db.Departments.Find(id);
            if (oSC_Department == null) return HttpNotFound();
            #endregion "Method"
            #region "Return"
            return View(oSC_Department);
            #endregion "Return"
        }

        // POST: Department/Delete/5
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
                ViewBag.CanDelete = af.CanDelete(grp_id, "Department");
                if (!ViewBag.CanDelete) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "AddValues"
            OSC_Department oSC_Department = db.Departments.Find(id);
            if (oSC_Department == null) return HttpNotFound();
            oSC_Department.IsActive = false;
            #endregion "AddValues"
            #region "Method"
            if (ModelState.IsValid)
            {
                db.Entry(oSC_Department).State = EntityState.Modified;
                db.SaveChanges();
            }
            #endregion "Method"
            #region "Return"
            return RedirectToAction("Index");
            #endregion "Return

            //OSC_Department oSC_Department = db.Departments.Find(id);
            //db.Departments.Remove(oSC_Department);
            //db.SaveChanges();
            //return RedirectToAction("Index");
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
