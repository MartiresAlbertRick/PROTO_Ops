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
using System.IO;
using ClosedXML.Excel;

namespace OPSCO_Web.Controllers
{
    public class NptTrackerController : Controller
    {
        private OSCContext db = new OSCContext();
        private AppFacade af = new AppFacade();
        private _AppFacade _af = new _AppFacade();

        // GET: NptTracker
        public ActionResult Index(int? page, int? pageSize, string searchByCategory, string searchByTeam, string searchByRep, string searchByDate, string searchByTimeSpent)
        {
            #region "Initialize"
            af.InitializeNpts(db);
            #endregion "Initialize"
            #region "BTSS"
            string role;
            string user_name;
            try
            {
                role = Session["role"].ToString();
                user_name = Session["logon_user"].ToString();
                string grp_id = Session["grp_id"].ToString();
                ViewBag.CanView = af.CanView(grp_id, "NPT Tracker");
                ViewBag.CanAdd = af.CanAdd(grp_id, "NPT Tracker");
                ViewBag.CanEdit = af.CanEdit(grp_id, "NPT Tracker");
                ViewBag.CanDelete = af.CanDelete(grp_id, "NPT Tracker");

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
                        if (af.IsManaged(npt.TeamId, user_name, role))
                            if (!TeamIds.Contains((long)npt.TeamId))
                                TeamIds.Add((long)npt.TeamId);
                    }
                    npts = (from n in db.NPT
                            where n.Source == "Manual" && TeamIds.Contains((long)n.TeamId) && n.IsActive
                            select n);
                    break;
                case "Staff":
                    OSC_Representative oSC_Representative = af.GetRepresentativeByPRD(user_name);
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
            if (searchByCategory != null || searchByTeam != null || searchByRep != null || searchByDate != null || searchByTimeSpent != null)
            {
                Session["NptFilter"] = true;
                List<long> TeamIdResult = new List<long>();
                TeamIdResult = null;
                if (!String.IsNullOrEmpty(searchByTeam))
                {
                    TeamIdResult = (from list in db.Teams where list.TeamName == searchByTeam select list.TeamId).ToList();
                    if (role != "Admin")
                    {
                        TeamIdResult = (from list in db.Teams where list.TeamName == searchByTeam && list.IsActive select list.TeamId).ToList();
                    }
                }
                List<long> RepIdResult = new List<long>();
                if (!String.IsNullOrEmpty(searchByRep))
                {
                    RepIdResult = (from list in db.Representatives where list.FirstName + " " + list.LastName == searchByRep select list.RepId).ToList();
                    if (role != "Admin")
                    {
                        RepIdResult = (from list in db.Representatives where (list.FirstName + " " + list.LastName == searchByRep) && list.IsActive select list.RepId).ToList();
                    }
                }

                if (searchByCategory != "")
                {
                    npts = npts.Where(t => t.TypeOfActivity == searchByCategory);
                }
                if (searchByTeam != "")
                {
                    npts = npts.Where(t => TeamIdResult.Contains((long)t.TeamId));
                }
                if (searchByRep != "")
                {
                    npts = npts.Where(t => RepIdResult.Contains((long)t.RepId));
                }
                if (searchByDate != "")
                {
                    var dt = Convert.ToDateTime(searchByDate);
                    npts = npts.Where(t => dt == t.DateOfActivity);
                }
                if (searchByTimeSpent != "")
                {
                    npts = npts.Where(t => Convert.ToDouble(searchByTimeSpent) == t.TimeSpent);
                }
                if (role != "Admin")
                {
                    npts = npts.Where(t => t.IsActive);
                }
            }
            #endregion "Table"
            #region "ViewBagFilters"
            if (searchByCategory != null) {
                Session["NptFilter_Category"] = searchByCategory;
                ViewBag.Category = searchByCategory;
            }
            else {
                Session["NptFilter_Category"] = "";
                ViewBag.Category = "";
            }
            if (searchByTeam != null) {
                Session["NptFilter_Team"] = searchByTeam;
                ViewBag.Team = searchByTeam;
            }
            else {
                Session["NptFilter_Team"] = "";
                ViewBag.Team = "";
            }
            if (searchByRep != null) {
                Session["NptFilter_Rep"] = searchByRep;
                ViewBag.Representative = searchByRep;
            }
            else {
                Session["NptFilter_Rep"] = "";
                ViewBag.Representative = "";
            }
            if (searchByDate != null) {
                Session["NptFilter_DoA"] = searchByDate;
                ViewBag.DateOfActivity = searchByDate;
            }
            else {
                Session["NptFilter_DoA"] = "";
                ViewBag.DateOfActivity = "";
            }
            if (searchByTimeSpent != null) {
                Session["NptFilter_TimeSpent"] = searchByTimeSpent;
                ViewBag.TimeSpent = searchByTimeSpent;
            }
            else {
                Session["NptFilter_TimeSpent"] = "";
                ViewBag.TimeSpent = "";
            }
            #endregion"ViewBagFilters"
            #region "Return"
            return View(npts.OrderByDescending(n => n.DateOfActivity).ToPagedList(page ?? 1, (int)defaultPageSize));
            #endregion "Return"
        }

        [HttpPost]
        public FileResult Export()
        {
            #region "Initialize"
            af.InitializeNpts(db);
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
                        if (af.IsManaged(npt.TeamId, user_name, role))
                            if (!TeamIds.Contains((long)npt.TeamId))
                                TeamIds.Add((long)npt.TeamId);
                    }
                    npts = (from n in db.NPT
                            where n.Source == "Manual" && TeamIds.Contains((long)n.TeamId) && n.IsActive
                            select n);
                    break;
                case "Staff":
                    OSC_Representative oSC_Representative = af.GetRepresentativeByPRD(user_name);
                    long repId;
                    repId = 0;
                    if (oSC_Representative != null)
                    { repId = oSC_Representative.RepId; }
                    npts = npts.Where(n => n.RepId == repId && n.IsActive);
                    break;
            }
            string searchByCategory = "", searchByTeam = "", searchByRep = "", searchByDate = "", searchByTimeSpent ="";
            if ((bool)Session["NptFilter"])
            {
                searchByCategory = (string)Session["NptFilter_Category"];
                searchByTeam = (string)Session["NptFilter_Team"];
                searchByRep = (string)Session["NptFilter_Rep"];
                searchByDate = (string)Session["NptFilter_DoA"];
                searchByTimeSpent = (string)Session["NptFilter_TimeSpent"];
            }
            List<long> TeamIdResult = new List<long>();
            TeamIdResult = null;
            if (!String.IsNullOrEmpty(searchByTeam))
            {
                TeamIdResult = (from list in db.Teams where list.TeamName == searchByTeam select list.TeamId).ToList();
                if (role != "Admin")
                {
                    TeamIdResult = (from list in db.Teams where list.TeamName == searchByTeam && list.IsActive select list.TeamId).ToList();
                }
            }
            List<long> RepIdResult = new List<long>();
            if (!String.IsNullOrEmpty(searchByRep))
            {
                RepIdResult = (from list in db.Representatives where list.FirstName + " " + list.LastName == searchByRep select list.RepId).ToList();
                if (role != "Admin")
                {
                    RepIdResult = (from list in db.Representatives where (list.FirstName + " " + list.LastName == searchByRep) && list.IsActive select list.RepId).ToList();
                }
            }

            if (searchByCategory != "")
            {
                npts = npts.Where(t => t.TypeOfActivity == searchByCategory);
            }
            if (searchByTeam != "")
            {
                npts = npts.Where(t => TeamIdResult.Contains((long)t.TeamId));
            }
            if (searchByRep != "")
            {
                npts = npts.Where(t => RepIdResult.Contains((long)t.RepId));
            }
            if (searchByDate != "")
            {
                var doa = Convert.ToDateTime(searchByDate);
                npts = npts.Where(t => doa == t.DateOfActivity);
            }
            if (searchByTimeSpent != "")
            {
                npts = npts.Where(t => Convert.ToDouble(searchByTimeSpent) == t.TimeSpent);
            }
            if (role != "Admin")
            {
                npts = npts.Where(t => t.IsActive);
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
        public JsonResult NptCategoriesAutoComplete(string prefix)
        {
            var nptCategories = (from obj in db.NptCategories
                                 where obj.CategoryDesc.StartsWith(prefix)
                                 select new { label = obj.CategoryDesc, val = obj.CategoryDesc }).ToList();
            return Json(nptCategories);
        }

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
                ViewBag.CanView = af.CanView(grp_id, "NPT Tracker");
                ViewBag.CanEdit = af.CanEdit(grp_id, "NPT Tracker");
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
            if (!af.IsManaged(oSC_ImportNPT.TeamId, user_name, role)) return HttpNotFound();
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
                ViewBag.CanAdd = af.CanAdd(grp_id, "NPT Tracker");
                if (!ViewBag.CanAdd) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "Initialize"
            af.InitializeRepresentatives(db);
            af.InitializeTeamNptCategories(db);
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
                        if (af.IsManaged(team.TeamId, user_name, role))
                            if (!TeamIds.Contains(team.TeamId))
                                TeamIds.Add(team.TeamId);
                    }
                    ViewBag.Teams = new SelectList(db.Teams.Where(x => TeamIds.Contains(x.TeamId) && x.IsActive), "TeamId", "TeamName");
                    break;
                case "Staff":
                    long repTeamId = (long)af.GetRepresentativeByPRD(user_name).TeamId;
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
                long repId = af.GetRepresentativeByPRD(user_name).RepId;
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
                ViewBag.CanAdd = af.CanAdd(grp_id, "NPT Tracker");
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
                ViewBag.CanEdit = af.CanEdit(grp_id, "NPT Tracker");
                if (!ViewBag.CanEdit) return HttpNotFound();
            }
            catch (Exception exception)
            {
                string result = exception.Message.ToString();
                return HttpNotFound();
            }
            #endregion "BTSS"
            #region "Initialize"
            af.InitializeTeamNptCategories(db);
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
                ViewBag.CanEdit = af.CanEdit(grp_id, "NPT Tracker");
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
                ViewBag.CanDelete = af.CanDelete(grp_id, "NPT Tracker");
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
            if (!af.IsManaged(oSC_ImportNPT.TeamId, user_name, role)) return HttpNotFound();
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
                ViewBag.CanDelete = af.CanDelete(grp_id, "NPT Tracker");
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
