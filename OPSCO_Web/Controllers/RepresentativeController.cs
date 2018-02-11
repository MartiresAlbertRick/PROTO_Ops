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
    public class RepresentativeController : Controller
    {
        private OSCContext db = new OSCContext();

        // GET: Representative
        public ActionResult Index(int? page, int? pageSize, string searchString)
        {
            #region "InitializeRepresentatives"
            db.InitializeRepresentatives();
            #endregion "InitializeRepresentatives"
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanView = db.appFacade.CanView(grp_id, "Representative");
                ViewBag.CanAdd = db.appFacade.CanAdd(grp_id, "Representative");
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "Representative");
                ViewBag.CanDelete = db.appFacade.CanDelete(grp_id, "Representative");

                if (!ViewBag.CanView) return HttpNotFound();
            }
            catch (Exception exception)
            {
                //session expired
                string result = exception.Message.ToString();
                return HttpNotFound();
            }

            var reps = (from r in db.Representatives
                        select r);
            List<long> TeamIds = new List<long>();
            switch (role)
            {
                case "Manager":
                case "Team Leader":
                case "Department Analyst":
                    foreach (OSC_Representative obj in reps)
                    {
                        if (db.IsManaged(obj.TeamId, user_name, role))
                            if (!TeamIds.Contains((long)obj.TeamId))
                                TeamIds.Add((long)obj.TeamId);
                    }
                    reps = (from r in db.Representatives
                             where TeamIds.Contains((long)r.TeamId) && r.IsActive == true
                             select r);
                    break;
                case "Staff":
                    OSC_Representative oSC_Representative = db.GetRepresentativeByPRD(user_name);
                    long repId;
                    repId = 0;
                    if (oSC_Representative != null)
                    { repId = oSC_Representative.RepId; }
                    reps = reps.Where(r => r.RepId == repId && r.IsActive);
                    break;
            }
            #endregion "BTSS"
            #region "Table"
            int? defaultPageSize = 10;
            if (pageSize != null) defaultPageSize = pageSize;
            if (!String.IsNullOrEmpty(searchString)) reps = reps.Where(r => r.FirstName.Contains(searchString) || r.LastName.Contains(searchString));
            #endregion "Table"
            #region "Return"
            return View(reps.OrderBy(r => r.TeamId).ThenBy(r => r.FirstName).ToPagedList(page ?? 1, (int)defaultPageSize));
            #endregion "Return"
        }

        // GET: Representative/Details/5
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
                ViewBag.CanView = db.appFacade.CanView(grp_id, "Representative");
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "Representative");
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
            OSC_Representative oSC_Representative = db.Representatives.Find(id);
            if (oSC_Representative == null) return HttpNotFound();
            if (!db.IsManaged(oSC_Representative.TeamId, user_name, role)) return HttpNotFound();
            oSC_Representative.Team = db.Teams.Find(oSC_Representative.TeamId);
            oSC_Representative.CoreRole = db.CoreRoles.Find(oSC_Representative.CoreRoleId);
            oSC_Representative.Location = db.Locations.Find(oSC_Representative.LocationId);
            #endregion "Method"
            #region "Return"
            return View(oSC_Representative);
            #endregion "Return"
        }

        // GET: Representative/Create
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
                ViewBag.CanAdd = db.appFacade.CanAdd(grp_id, "Representative");
                if (!ViewBag.CanAdd) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "ViewBagTeams"
            switch (role)
            {
                case "Admin":
                    ViewBag.Teams = new SelectList(db.Teams, "TeamId", "TeamName");
                    break;
                case "Manager":
                case "Team Leader":
                case "Department Analyst":
                    List<long> TeamIds = new List<long>();
                    foreach (OSC_Team team in db.Teams)
                    {
                        if (db.IsManaged(team.TeamId, user_name, role))
                            if (!TeamIds.Contains(team.TeamId))
                                TeamIds.Add(team.TeamId);
                    }
                    ViewBag.Teams = new SelectList(db.Teams.Where(x => TeamIds.Contains(x.TeamId) && x.IsActive == true), "TeamId", "TeamName");
                    break;
                case "Staff":
                    long repTeamId = (long)db.GetRepresentativeByPRD(user_name).TeamId;
                    ViewBag.Teams = new SelectList(db.Teams.Where(t => t.TeamId == repTeamId), "TeamId", "TeamName");
                    break;
            }
            #endregion "ViewBagTeams"
            #region "ViewBagLocations"
            ViewBag.Locations = new SelectList(db.Locations, "LocationId", "Location");
            #endregion "ViewBagLocations"
            #region "ViewBagCoreRoles"
            ViewBag.CoreRoles = new SelectList(db.CoreRoles, "CoreRoleId", "CoreRole");
            #endregion "ViewBagCoreRoles"
            #region "Return"
            return View();
            #endregion "Return"
        }

        // POST: Representative/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RepId,PRDUserId,AIQUserId,BIUserId,WorkdayId,FirstName,MiddleName,LastName,TeamId,CoreRoleId,StartDate,EndDate,Comments,OnShoreRep,PhoneRep,WorkHours,LocationId,HasPrevious,PreviousId,IsCurrent,IsActive")] OSC_Representative oSC_Representative)
        {
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanAdd = db.appFacade.CanAdd(grp_id, "Representative");
                if (!ViewBag.CanAdd) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "AddValues"
            oSC_Representative.IsActive = true;
            #endregion "AddValues"
            #region "Method"
            if (ModelState.IsValid)
            {
                db.Representatives.Add(oSC_Representative);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            #endregion "Method"
            #region "Return"
            return View(oSC_Representative);
            #endregion "Return"
        }

        // GET: Representative/Edit/5
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
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "Representative");
                if (!ViewBag.CanEdit) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "ViewBagTeams"
            ViewBag.Teams = new SelectList(db.Teams, "TeamId", "TeamName");
            #endregion "ViewBagTeams"
            #region "ViewBagLocations"
            ViewBag.Locations = new SelectList(db.Locations, "LocationId", "Location");
            #endregion "ViewBagLocations"
            #region "ViewBagCoreRoles"
            ViewBag.CoreRoles = new SelectList(db.CoreRoles, "CoreRoleId", "CoreRole");
            #endregion "ViewBagCoreRoles"
            #region "Method"
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            OSC_Representative oSC_Representative = db.Representatives.Find(id);
            if (oSC_Representative == null) return HttpNotFound();
            #endregion "Method"
            #region "Return"
            return View(oSC_Representative);
            #endregion "Return"
        }

        // POST: Representative/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RepId,PRDUserId,AIQUserId,BIUserId,WorkdayId,FirstName,MiddleName,LastName,TeamId,CoreRoleId,StartDate,EndDate,Comments,OnShoreRep,PhoneRep,WorkHours,LocationId,HasPrevious,PreviousId,IsCurrent,IsActive")] OSC_Representative oSC_Representative)
        {
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "Representative");
                if (!ViewBag.CanEdit) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "AddValues"
            if (Session["role"].ToString() != "Admin") oSC_Representative.IsActive = true;
            #endregion "AddValues"
            #region "Method"
            if (ModelState.IsValid)
            {
                db.Entry(oSC_Representative).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            #endregion "Method"
            #region "Return"
            return View(oSC_Representative);
            #endregion "Return"
        }

        // GET: Representative/Delete/5
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
            OSC_Representative oSC_Representative = db.Representatives.Find(id);
            if (oSC_Representative == null) return HttpNotFound();
            if (!db.IsManaged(oSC_Representative.TeamId, user_name, role)) return HttpNotFound();
            oSC_Representative.Team = db.Teams.Find(oSC_Representative.TeamId);
            oSC_Representative.CoreRole = db.CoreRoles.Find(oSC_Representative.CoreRoleId);
            oSC_Representative.Location = db.Locations.Find(oSC_Representative.LocationId);
            #endregion "Method"
            #region "Return"
            return View(oSC_Representative);
            #endregion "Return"
        }

        // POST: Representative/Delete/5
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
                ViewBag.CanDelete = db.appFacade.CanDelete(grp_id, "Representative");
                if (!ViewBag.CanDelete) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "AddValues"
            OSC_Representative oSC_Representative = db.Representatives.Find(id);
            if (oSC_Representative == null) return HttpNotFound();
            oSC_Representative.IsActive = false;
            #endregion "AddValues"
            #region "Method"
            if (ModelState.IsValid)
            {
                db.Entry(oSC_Representative).State = EntityState.Modified;
                db.SaveChanges();
            }
            #endregion "Method"
            #region "Return"
            return RedirectToAction("Index");
            #endregion "Return"
            //OSC_Representative oSC_Representative = db.Representatives.Find(id);
            //db.Representatives.Remove(oSC_Representative);
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
