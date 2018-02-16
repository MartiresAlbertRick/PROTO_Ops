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
using System.IO;
using ClosedXML.Excel;

namespace OPSCO_Web.Controllers
{
    public class NptTrackerController : Controller
    {
        private OSCContext db = new OSCContext();

        // GET: NptTracker
        public ActionResult Index(int? page, int? pageSize, string searchString)
        {
            #region "Initialize"
            db.InitializeNpts();
            #endregion "Initialize"
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanView = db.appFacade.CanView(grp_id, "NPT Tracker");
                ViewBag.CanAdd = db.appFacade.CanAdd(grp_id, "NPT Tracker");
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "NPT Tracker");
                ViewBag.CanDelete = db.appFacade.CanDelete(grp_id, "NPT Tracker");

                if (!ViewBag.CanView) return HttpNotFound();
            }
            catch (Exception exception)
            {
                //session expired
                string result = exception.Message.ToString();
                return HttpNotFound();
            }

            var npts = (from n in db.NPT
                        where n.Source == "Manual"
                        select n);
            List<long> TeamIds = new List<long>();
            switch (role)
            {
                case "Manager":
                case "Team Leader":
                case "Department Analyst":
                    foreach (OSC_ImportNPT npt in npts)
                    {
                        if (db.IsManaged(npt.TeamId, user_name, role))
                            if (!TeamIds.Contains((long)npt.TeamId))
                                TeamIds.Add((long)npt.TeamId);
                    }
                    npts = (from n in db.NPT
                            where n.Source == "Manual" && TeamIds.Contains((long)n.TeamId) && n.IsActive == true
                            select n);
                    break;
                case "Staff":
                    OSC_Representative oSC_Representative = db.GetRepresentativeByPRD(user_name);
                    long repId;
                    repId = 0;
                    if (oSC_Representative != null)
                    { repId = oSC_Representative.RepId; }
                    npts = npts.Where(n => n.RepId == repId && n.IsActive);
                    break;
            }
            #endregion "BTSS"
            #region "Table"
            int? defaultPageSize = 10;
            if (pageSize != null) defaultPageSize = pageSize;
            if (!String.IsNullOrEmpty(searchString)) npts = npts.Where(n => n.DateOfActivity.ToString().Contains(searchString) || n.TypeOfActivity.Contains(searchString));
            #endregion "Table"
            #region "Return"
            return View(npts.OrderByDescending(n => n.DateOfActivity).ToPagedList(page ?? 1, (int)defaultPageSize));
            #endregion "Return"
        }

        [HttpPost]
        public FileResult Export()
        {
            #region "Initialize"
            db.InitializeNpts();
            #endregion "Initialize"
            #region "BTSS"
            string role;
            string user_name;
            role = Session["role"].ToString();
            user_name = Session["logon_user"].ToString();
            #endregion "BTSS"
            #region "TransferData"
            DataTable dt = new DataTable("NPT");
            dt.Columns.AddRange(new DataColumn[8] {
                new DataColumn("Team"),
                new DataColumn("Representative"),
                new DataColumn("Category"),
                new DataColumn("Activity Description"),
                new DataColumn("Date of Activity"),
                new DataColumn("Time Spent"),
                new DataColumn("Modified By"),
                new DataColumn("Date Modified")
            });
            var npts = (from n in db.NPT
                        where n.Source == "Manual"
                        select n);
            List<long> TeamIds = new List<long>();
            switch (role)
            {
                case "Manager":
                case "Team Leader":
                case "Department Analyst":
                    foreach (OSC_ImportNPT npt in npts)
                    {
                        if (db.IsManaged(npt.TeamId, user_name, role))
                            if (!TeamIds.Contains((long)npt.TeamId))
                                TeamIds.Add((long)npt.TeamId);
                    }
                    npts = (from n in db.NPT
                            where n.Source == "Manual" && TeamIds.Contains((long)n.TeamId) && n.IsActive
                            select n);
                    break;
                case "Staff":
                    OSC_Representative oSC_Representative = db.GetRepresentativeByPRD(user_name);
                    long repId;
                    repId = 0;
                    if (oSC_Representative != null)
                    { repId = oSC_Representative.RepId; }
                    npts = npts.Where(n => n.RepId == repId && n.IsActive);
                    break;
            }
            foreach (var npt in npts)
            {
                dt.Rows.Add(npt.Team.TeamName, npt.Representative.FullName, npt.TypeOfActivity, npt.Activity, npt.DateOfActivity, npt.TimeSpent, npt.UploadedBy, npt.DateUploaded);
            }
            #endregion "TransferData"
            #region "GenerateSpreadsheet"
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "NPTList_" + DateTime.Now.ToShortDateString() + ".xlsx");
                }
            }
            #endregion "GenerateSpreadsheet"
        }

        #region "AutoComplete"
        [HttpPost]
        public JsonResult TeamAutoComplete(string prefix)
        {
            var teams = (from obj in db.Teams
                         where obj.TeamName.StartsWith(prefix)
                         select new { label = obj.TeamName, val = obj.TeamId }).ToList();
            return Json(teams);
        }

        [HttpPost]
        public JsonResult RepresentativeAutoComplete(string prefix)
        {

            var reps = (from obj in db.Representatives
                        where obj.FirstName.StartsWith(prefix) || obj.LastName.StartsWith(prefix)
                        select new { label = obj.FirstName + " " + obj.LastName, val = obj.RepId });
            return Json(reps);
        }
        #endregion "AutoComplete"

        // GET: NptTracker/Details/5
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
                ViewBag.CanView = db.appFacade.CanView(grp_id, "NPT Tracker");
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "NPT Tracker");
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
            OSC_ImportNPT oSC_ImportNPT = db.NPT.Find(id);
            if (oSC_ImportNPT == null) return HttpNotFound();
            if (!db.IsManaged(oSC_ImportNPT.TeamId, user_name, role)) return HttpNotFound();
            oSC_ImportNPT.Team = db.Teams.Find(oSC_ImportNPT.TeamId);
            oSC_ImportNPT.Representative = db.Representatives.Find(oSC_ImportNPT.RepId);
            oSC_ImportNPT.Representative.FullName = oSC_ImportNPT.Representative.FirstName + " " + oSC_ImportNPT.Representative.LastName;
            #endregion "Method"
            #region "Return"
            return View(oSC_ImportNPT);
            #endregion "Return"
        }

        // GET: NptTracker/Create
        public ActionResult Create(long? teamId)
        {
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanAdd = db.appFacade.CanAdd(grp_id, "NPT Tracker");
                if (!ViewBag.CanAdd) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "Initialize"
            db.InitializeRepresentatives();
            db.InitializeTeamNptCategories();
            #endregion "Initialize"
            #region "ViewBagTeam"
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
                    ViewBag.Teams = new SelectList(db.Teams.Where(x => TeamIds.Contains(x.TeamId) && x.IsActive), "TeamId", "TeamName");
                    break;
                case "Staff":
                    long repTeamId = (long)db.GetRepresentativeByPRD(user_name).TeamId;
                    ViewBag.Teams = new SelectList(db.Teams.Where(t => t.TeamId == repTeamId && t.IsActive), "TeamId", "TeamName");
                    break;

            }
            #endregion "ViewBagTeam"
            #region "ViewBagRepresentativeTeamNptCategories"
            long defaultTeamId = 0;
            var reps = (from r in db.Representatives select r);
            var teamNptCategories = (from t in db.TeamNptCategories select t);
            if (teamId != null)
            {
                long repId = db.GetRepresentativeByPRD(user_name).RepId;
                switch (role)
                {
                    case "Admin":
                        reps = reps.Where(r => r.TeamId == teamId);
                        break;
                    case "Manager":
                    case "Team Leader":
                    case "Department Analyst":
                        reps = reps.Where(r => r.TeamId == teamId && r.IsActive);
                        break;
                    case "Staff":
                        reps = reps.Where(r => r.TeamId == teamId && r.RepId == repId && r.IsActive);
                        break;
                }
                teamNptCategories = teamNptCategories.Where(t => t.TeamId == teamId);
            }
            else
            {
                reps = reps.Where(r => r.TeamId == defaultTeamId);
                teamNptCategories = teamNptCategories.Where(t => t.TeamId == defaultTeamId);
            }
            ViewBag.Representatives = new SelectList(reps, "RepId", "FullName");
            ViewBag.TeamNptCategories = new SelectList(teamNptCategories, "CategoryDesc", "CategoryDesc");
            #endregion "ViewBagRepresentativeTeamNptCategories"
            #region "Return"
            return View();
            #endregion "Return"
        }

        // POST: NptTracker/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NPTReportId,RepId,Activity,DateOfActivity,TimeSpent,TypeOfActivity,TeamId,IsActive")] OSC_ImportNPT oSC_ImportNPT)
        {
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanAdd = db.appFacade.CanAdd(grp_id, "NPT Tracker");
                if (!ViewBag.CanAdd) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "AddValues"
            oSC_ImportNPT.Month = Convert.ToDateTime(oSC_ImportNPT.DateOfActivity).Month;
            oSC_ImportNPT.Year = Convert.ToDateTime(oSC_ImportNPT.DateOfActivity).Year;
            oSC_ImportNPT.DateUploaded = DateTime.Now;
            oSC_ImportNPT.UploadedBy = user_name;
            oSC_ImportNPT.IsActive = true;
            oSC_ImportNPT.Source = "Manual";
            #endregion "AddValues"
            #region "Method"
            if (ModelState.IsValid)
            {
                db.NPT.Add(oSC_ImportNPT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            #endregion "Method"
            #region "Return"
            return View(oSC_ImportNPT);
            #endregion "Return"
        }

        // GET: NptTracker/Edit/5
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
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "NPT Tracker");
                if (!ViewBag.CanEdit) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "Initialize"
            db.InitializeTeamNptCategories();
            #endregion "Initialize"
            #region "Method"
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            OSC_ImportNPT oSC_ImportNPT = db.NPT.Find(id);
            if (oSC_ImportNPT == null) return HttpNotFound();
            oSC_ImportNPT.Team = db.Teams.Find(oSC_ImportNPT.TeamId);
            oSC_ImportNPT.Representative = db.Representatives.Find(oSC_ImportNPT.RepId);
            oSC_ImportNPT.Representative.FullName = oSC_ImportNPT.Representative.FirstName + " " + oSC_ImportNPT.Representative.LastName;
            #endregion "Method"
            #region "ViewBagTeamNptCategory"
            var teamNptCategories = (from t in db.TeamNptCategories select t);
            teamNptCategories = teamNptCategories.Where(t => t.TeamId == oSC_ImportNPT.TeamId);
            ViewBag.TeamNptCategories = new SelectList(teamNptCategories, "CategoryDesc", "CategoryDesc");
            #endregion "ViewBagTeamNptCategory"
            #region "Return"
            return View(oSC_ImportNPT);
            #endregion "Return"
        }

        // POST: NptTracker/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NPTReportId,RepId,Activity,DateOfActivity,TimeSpent,TypeOfActivity,CreatedBy,ItemType,Path,TeamId,Month,Year,DateUploaded,UploadedBy,Source,CategoryId,SubCategoryId,IsActive")] OSC_ImportNPT oSC_ImportNPT)
        {
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanEdit = db.appFacade.CanEdit(grp_id, "NPT Tracker");
                if (!ViewBag.CanEdit) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "AddValues"
            oSC_ImportNPT.Month = Convert.ToDateTime(oSC_ImportNPT.DateOfActivity).Month;
            oSC_ImportNPT.Year = Convert.ToDateTime(oSC_ImportNPT.DateOfActivity).Year;
            oSC_ImportNPT.DateUploaded = DateTime.Now;
            oSC_ImportNPT.UploadedBy = user_name;
            oSC_ImportNPT.Source = "Manual";
            if (Session["role"].ToString() != "Admin") oSC_ImportNPT.IsActive = true;
            #endregion "AddValues"
            #region "Method"
            if (ModelState.IsValid)
            {
                db.Entry(oSC_ImportNPT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            #endregion "Method"
            #region "Return"
            return View(oSC_ImportNPT);
            #endregion "Return"
        }

        // GET: NptTracker/Delete/5
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
                ViewBag.CanDelete = db.appFacade.CanDelete(grp_id, "NPT Tracker");
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
            OSC_ImportNPT oSC_ImportNPT = db.NPT.Find(id);
            if (oSC_ImportNPT == null) return HttpNotFound();
            if (!db.IsManaged(oSC_ImportNPT.TeamId, user_name, role)) return HttpNotFound();
            oSC_ImportNPT.Team = db.Teams.Find(oSC_ImportNPT.TeamId);
            oSC_ImportNPT.Representative = db.Representatives.Find(oSC_ImportNPT.RepId);
            oSC_ImportNPT.Representative.FullName = oSC_ImportNPT.Representative.FirstName + " " + oSC_ImportNPT.Representative.LastName;
            #endregion "Method"
            #region "Return"
            return View(oSC_ImportNPT);
            #endregion "Return"
        }

        // POST: NptTracker/Delete/5
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
                ViewBag.CanDelete = db.appFacade.CanDelete(grp_id, "NPT Tracker");
                if (!ViewBag.CanDelete) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "AddValues"
            OSC_ImportNPT oSC_ImportNPT = db.NPT.Find(id);
            oSC_ImportNPT.DateUploaded = DateTime.Now;
            oSC_ImportNPT.UploadedBy = user_name;
            oSC_ImportNPT.IsActive = false;
            #endregion "AddValues"
            #region "Method"
            if (ModelState.IsValid)
            {
                db.Entry(oSC_ImportNPT).State = EntityState.Modified;
                db.SaveChanges();
            }
            #endregion "Method"
            #region "Return"
            return RedirectToAction("Index");
            #endregion "Return"
            //OSC_ImportNPT oSC_ImportNPT = db.NPT.Find(id);
            //db.NPT.Remove(oSC_ImportNPT);
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
