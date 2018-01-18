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
        private OSCContext db = new OSCContext();

        // GET: Manager
        public ActionResult Index(int? page, int? pageSize, string searchString)
        {

            int? defaultPageSize = 10;

            if (pageSize != null)
            {
                defaultPageSize = pageSize;
            }

            var managers = (from m in db.Managers
                        select m);

            if (!String.IsNullOrEmpty(searchString))
            {
                managers = managers.Where(r => r.PRDUserId.Contains(searchString) || r.Name.Contains(searchString));
            }

            return View(managers.OrderBy(r => r.Name).ToPagedList(page ?? 1, (int)defaultPageSize));
            //return View(db.Managers.ToList());
        }

        // GET: Manager/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_Manager oSC_Manager = db.Managers.Find(id);
            if (oSC_Manager == null)
            {
                return HttpNotFound();
            }
            return View(oSC_Manager);
        }

        // GET: Manager/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ManagerId,PRDUserId,Name,IsActive")] OSC_Manager oSC_Manager)
        {
            if (ModelState.IsValid)
            {
                db.Managers.Add(oSC_Manager);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(oSC_Manager);
        }

        // GET: Manager/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_Manager oSC_Manager = db.Managers.Find(id);
            if (oSC_Manager == null)
            {
                return HttpNotFound();
            }
            return View(oSC_Manager);
        }

        // POST: Manager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ManagerId,PRDUserId,Name,IsActive")] OSC_Manager oSC_Manager)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oSC_Manager).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oSC_Manager);
        }

        // GET: Manager/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OSC_Manager oSC_Manager = db.Managers.Find(id);
            if (oSC_Manager == null)
            {
                return HttpNotFound();
            }
            return View(oSC_Manager);
        }

        // POST: Manager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            OSC_Manager oSC_Manager = db.Managers.Find(id);
            db.Managers.Remove(oSC_Manager);
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
