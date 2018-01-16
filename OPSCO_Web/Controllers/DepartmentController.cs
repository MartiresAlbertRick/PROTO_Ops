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
    public class DepartmentController : Controller
    {
        private OSCContext db = new OSCContext();

        // GET: Department
        public ActionResult Index(int? page, int? pageSize, string searchString)
        {
            int? defaultPageSize = 10;

            if (pageSize != null)
            {
                defaultPageSize = pageSize;
            }

            var departments = from d in db.Departments
                              select d;

            if (!String.IsNullOrEmpty(searchString))
            {
                departments = departments.Where(d => d.DepartmentName.Contains(searchString));
            }

            return View(departments.OrderBy(d => d.DepartmentName).ToPagedList(page ?? 1, (int)defaultPageSize));
        }

        // GET: Department/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_Department oSC_Department = db.Departments.Find(id);
            if (oSC_Department == null)
            {
                return HttpNotFound();
            }
            return View(oSC_Department);
        }

        // GET: Department/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DepartmentId,DepartmentName,IsActive")] OSC_Department oSC_Department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(oSC_Department);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oSC_Department);
        }

        // GET: Department/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_Department oSC_Department = db.Departments.Find(id);
            if (oSC_Department == null)
            {
                return HttpNotFound();
            }
            return View(oSC_Department);
        }

        // POST: Department/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DepartmentId,DepartmentName,IsActive")] OSC_Department oSC_Department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oSC_Department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oSC_Department);
        }

        // GET: Department/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_Department oSC_Department = db.Departments.Find(id);
            if (oSC_Department == null)
            {
                return HttpNotFound();
            }
            return View(oSC_Department);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            OSC_Department oSC_Department = db.Departments.Find(id);
            db.Departments.Remove(oSC_Department);
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
