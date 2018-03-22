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
        #region Variables
        private const string module = "NPT Tracker";
        private _AppFacade _af;
        private _Access a;
        //list initialize for autocomplete
        public List<OSC_NptCategory> nptL;
        public List<OSC_Team> teamL;
        public List<OSC_Representative> repL;
        #endregion Variables
        #region Constructor
        public NptTrackerController()
        {
            this._af = new _AppFacade();
            this.a = new _Access();
            this.nptL = this._af.blNptCategory.idb;
            this.teamL = this._af.blTeam.idb;
            this.repL = this._af.blRepresentative.idb;
        }
        #endregion Constructor
        #region Method
        #region Index
        // GET: NptTracker
        public ActionResult Index(int? page, int? pageSize, bool? filterOn, _Filter filter)
        {
            #region BTSS 
            try
            {
                if (Session["role"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else if (Session["logon_user"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else if (Session["grp_id"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else
                {
                    this.a = new _Access()
                    {
                        LogonUser = Session["logon_user"].ToString(),
                        Role = Session["role"].ToString(),
                        Group = Session["grp_id"].ToString(),
                        CanView = this._af.blAccess.CanView(Session["grp_id"].ToString(), module),
                        CanAdd = this._af.blAccess.CanAdd(Session["grp_id"].ToString(), module),
                        CanEdit = this._af.blAccess.CanEdit(Session["grp_id"].ToString(), module),
                        CanDelete = this._af.blAccess.CanDelete(Session["grp_id"].ToString(), module)
                    };
                    if (!this.a.CanView) return View("~/Views/Shared/NoAccess.cshtml");
                    else ViewBag.Access = this.a;
                }
            }
            catch (Exception ex)
            {
                _ErrorMessage err = new _ErrorMessage();
                err.MessageType = "Server Error - NPTTrackerController";
                err.MessageDescription = ex.Message.ToString();
                ViewBag.Error = err;
                return View("~/Views/Shared/SomethingWentWrong.cshtml");
            }
            #endregion BTSS
            #region Table
            List<OSC_ImportNPT> npts = new List<OSC_ImportNPT>();
            npts = this._af.blNpt.GetList(a);
            if (npts.Count == 0 && this._af.blNpt._err != null)
            {
                ViewBag.Error = this._af.blNpt._err;
                return View("~/Views/Shared/SomethingWentWrong.cshtml");
            }
            #endregion Table
            #region Filters
            if (filterOn != null && filterOn == true)
            {
                
            }
            else
            {
                string noVal = "";
                ViewBag.Category = noVal;
                ViewBag.Team = noVal;
                ViewBag.Representative = noVal;
                ViewBag.DateOfActivity = noVal;
                ViewBag.TimeSpent = noVal;
            }
            #endregion Filters
            #region Return
                int? defaultPageSize = 10;
                if (pageSize != null) defaultPageSize = pageSize;
                return View(npts.OrderByDescending(n => n.DateOfActivity).ToPagedList(page ?? 1, (int)defaultPageSize));
            #endregion Return
        }
        #endregion Index
        //[HttpPost]
        //public FileResult Export()
        //{
        //    #region "Initialize"
        //    af.InitializeNpts(db);
        //    #endregion "Initialize"
        //    #region "BTSS"
        //    string role;
        //    string user_name;
        //    role = Session["role"].ToString();
        //    user_name = Session["logon_user"].ToString();
        //    #endregion "BTSS"
        //    #region "TransferData"
        //    DataTable dt = new DataTable("NPT");
        //    dt.Columns.AddRange(new DataColumn[8] {
        //        new DataColumn("Team"),
        //        new DataColumn("Representative"),
        //        new DataColumn("Category"),
        //        new DataColumn("Activity Description"),
        //        new DataColumn("Date of Activity"),
        //        new DataColumn("Time Spent"),
        //        new DataColumn("Modified By"),
        //        new DataColumn("Date Modified")
        //    });
        //    var npts = (from n in db.NPT
        //                where n.Source == "Manual"
        //                select n);
        //    List<long> TeamIds = new List<long>();
        //    switch (role)
        //    {
        //        case "Manager":
        //        case "Team Leader":
        //        case "Department Analyst":
        //            foreach (OSC_ImportNPT npt in npts)
        //            {
        //                if (af.IsManaged(npt.TeamId, user_name, role))
        //                    if (!TeamIds.Contains((long)npt.TeamId))
        //                        TeamIds.Add((long)npt.TeamId);
        //            }
        //            npts = (from n in db.NPT
        //                    where n.Source == "Manual" && TeamIds.Contains((long)n.TeamId) && n.IsActive
        //                    select n);
        //            break;
        //        case "Staff":
        //            OSC_Representative oSC_Representative = af.GetRepresentativeByPRD(user_name);
        //            long repId;
        //            repId = 0;
        //            if (oSC_Representative != null)
        //            { repId = oSC_Representative.RepId; }
        //            npts = npts.Where(n => n.RepId == repId && n.IsActive);
        //            break;
        //    }
        //    string searchByCategory = "", searchByTeam = "", searchByRep = "", searchByDate = "", searchByTimeSpent ="";
        //    if ((bool)Session["NptFilter"])
        //    {
        //        searchByCategory = (string)Session["NptFilter_Category"];
        //        searchByTeam = (string)Session["NptFilter_Team"];
        //        searchByRep = (string)Session["NptFilter_Rep"];
        //        searchByDate = (string)Session["NptFilter_DoA"];
        //        searchByTimeSpent = (string)Session["NptFilter_TimeSpent"];
        //    }
        //    List<long> TeamIdResult = new List<long>();
        //    TeamIdResult = null;
        //    if (!String.IsNullOrEmpty(searchByTeam))
        //    {
        //        TeamIdResult = (from list in db.Teams where list.TeamName == searchByTeam select list.TeamId).ToList();
        //        if (role != "Admin")
        //        {
        //            TeamIdResult = (from list in db.Teams where list.TeamName == searchByTeam && list.IsActive select list.TeamId).ToList();
        //        }
        //    }
        //    List<long> RepIdResult = new List<long>();
        //    if (!String.IsNullOrEmpty(searchByRep))
        //    {
        //        RepIdResult = (from list in db.Representatives where list.FirstName + " " + list.LastName == searchByRep select list.RepId).ToList();
        //        if (role != "Admin")
        //        {
        //            RepIdResult = (from list in db.Representatives where (list.FirstName + " " + list.LastName == searchByRep) && list.IsActive select list.RepId).ToList();
        //        }
        //    }

        //    if (searchByCategory != "")
        //    {
        //        npts = npts.Where(t => t.TypeOfActivity == searchByCategory);
        //    }
        //    if (searchByTeam != "")
        //    {
        //        npts = npts.Where(t => TeamIdResult.Contains((long)t.TeamId));
        //    }
        //    if (searchByRep != "")
        //    {
        //        npts = npts.Where(t => RepIdResult.Contains((long)t.RepId));
        //    }
        //    if (searchByDate != "")
        //    {
        //        var doa = Convert.ToDateTime(searchByDate);
        //        npts = npts.Where(t => doa == t.DateOfActivity);
        //    }
        //    if (searchByTimeSpent != "")
        //    {
        //        npts = npts.Where(t => Convert.ToDouble(searchByTimeSpent) == t.TimeSpent);
        //    }
        //    if (role != "Admin")
        //    {
        //        npts = npts.Where(t => t.IsActive);
        //    }
        //    foreach (var npt in npts)
        //    {
        //        dt.Rows.Add(npt.Team.TeamName, npt.Representative.FullName, npt.TypeOfActivity, npt.Activity, npt.DateOfActivity, npt.TimeSpent, npt.UploadedBy, npt.DateUploaded);
        //    }
        //    #endregion "TransferData"
        //    #region "GenerateSpreadsheet"
        //    using (XLWorkbook wb = new XLWorkbook())
        //    {
        //        wb.Worksheets.Add(dt);
        //        using (MemoryStream stream = new MemoryStream())
        //        {
        //            wb.SaveAs(stream);
        //            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "NPTList_" + DateTime.Now.ToShortDateString() + ".xlsx");
        //        }
        //    }
        //    #endregion "GenerateSpreadsheet"
        //}
        #region AutoComplete
        [HttpPost]
        public JsonResult NptCategoriesAutoComplete(string prefix)
        {
            var nptCategories = (from obj in this.nptL
                                 where obj.CategoryDesc.StartsWith(prefix)
                                 select new { label = obj.CategoryDesc, val = obj.CategoryDesc }).ToList();
            return Json(nptCategories);
        }

        [HttpPost]
        public JsonResult TeamAutoComplete(string prefix)
        {
            var teams = (from obj in this.teamL
                         where obj.TeamName.StartsWith(prefix)
                         select new { label = obj.TeamName, val = obj.TeamId }).ToList();
            return Json(teams);
        }

        [HttpPost]
        public JsonResult RepresentativeAutoComplete(string prefix)
        {

            var reps = (from obj in this.repL
                        where obj.FirstName.StartsWith(prefix) || obj.LastName.StartsWith(prefix)
                        select new { label = obj.FirstName + " " + obj.LastName, val = obj.RepId });
            return Json(reps);
        }
        #endregion AutoComplete
        #region Details
        // GET: NptTracker/Details/5
        public ActionResult Details(long? id)
        {
            #region BTSS 
            try
            {
                if (Session["role"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else if (Session["logon_user"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else if (Session["grp_id"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else
                {
                    this.a = new _Access()
                    {
                        LogonUser = Session["logon_user"].ToString(),
                        Role = Session["role"].ToString(),
                        Group = Session["grp_id"].ToString(),
                        CanView = this._af.blAccess.CanView(Session["grp_id"].ToString(), module),
                        CanEdit = this._af.blAccess.CanEdit(Session["grp_id"].ToString(), module)
                    };
                    if (!this.a.CanView) return View("~/Views/Shared/NoAccess.cshtml");
                    else ViewBag.Access = this.a;
                }
            }
            catch (Exception ex)
            {
                _ErrorMessage err = new _ErrorMessage();
                err.MessageType = "Server Error - NPTTrackerController";
                err.MessageDescription = ex.Message.ToString();
                ViewBag.Error = err;
                return View("~/Views/Shared/SomethingWentWrong.cshtml");
            }
            #endregion BTSS
            #region Method
            if (id == null)
            { 
                _ErrorMessage err = new _ErrorMessage() {
                    MessageType = "Server Error - NPTTrackerController",
                    MessageDescription = "Invalid use of null" };
                ViewBag.Error = err;
                return View("~/Views/Shared/SomethingWentWrong.cshtml");
            }
            OSC_ImportNPT oSC_ImportNPT = this._af.blNpt.Get((long)id);
            if (this._af.blNpt._err == null)
            {
                ViewBag.Error = this._af.blNpt._err;
                return View("~/Views/Shared/SomethingWentWrong.cshtml");
            }
            if (oSC_ImportNPT == null) return View("~/Views/Shared/NotFound.cshtml");
            #endregion Method
            #region Return
            return View(oSC_ImportNPT);
            #endregion Return
        }
        #endregion Details
        #region Create
        // GET: NptTracker/Create
        public ActionResult Create(long? teamId)
        {
            #region BTSS 
            try
            {
                if (Session["role"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else if (Session["logon_user"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else if (Session["grp_id"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else
                {
                    this.a = new _Access()
                    {
                        LogonUser = Session["logon_user"].ToString(),
                        Role = Session["role"].ToString(),
                        Group = Session["grp_id"].ToString(),
                        CanAdd = this._af.blAccess.CanAdd(Session["grp_id"].ToString(), module)
                    };
                    if (!this.a.CanAdd) return View("~/Views/Shared/NoAccess.cshtml");
                    else ViewBag.Access = this.a;
                }
            }
            catch (Exception ex)
            {
                _ErrorMessage err = new _ErrorMessage();
                err.MessageType = "Server Error - NPTTrackerController";
                err.MessageDescription = ex.Message.ToString();
                ViewBag.Error = err;
                return View("~/Views/Shared/SomethingWentWrong.cshtml");
            }
            #endregion BTSS
            #region Return
            if (teamId == null)
            {
                teamId = 0;
            }
            ViewBag.Teams = new SelectList(_af.blTeam.GetList(a), "TeamId", "TeamName");
            ViewBag.Representatives = new SelectList(_af.blRepresentative.GetList(a).Where(t => t.TeamId == (long)teamId), "RepId", "FullName");
            ViewBag.TeamNptCategories = new SelectList(_af.blTeamNptCategories.GetList((long)teamId), "CategoryDesc", "CategoryDesc");
            return View();
            #endregion Return
        }

        // POST: NptTracker/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NPTReportId,RepId,Activity,DateOfActivity,TimeSpent,TypeOfActivity,TeamId,IsActive")] OSC_ImportNPT oSC_ImportNPT)
        {
            #region BTSS 
            try
            {
                if (Session["role"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else if (Session["logon_user"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else if (Session["grp_id"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else
                {
                    this.a = new _Access()
                    {
                        LogonUser = Session["logon_user"].ToString(),
                        Role = Session["role"].ToString(),
                        Group = Session["grp_id"].ToString(),
                        CanAdd = this._af.blAccess.CanAdd(Session["grp_id"].ToString(), module)
                    };
                    if (!this.a.CanAdd) return View("~/Views/Shared/NoAccess.cshtml");
                    else ViewBag.Access = this.a;
                }
            }
            catch (Exception ex)
            {
                _ErrorMessage err = new _ErrorMessage();
                err.MessageType = "Server Error - NPTTrackerController";
                err.MessageDescription = ex.Message.ToString();
                ViewBag.Error = err;
                return View("~/Views/Shared/SomethingWentWrong.cshtml");
            }
            #endregion BTSS
            #region Method
            oSC_ImportNPT.Month = Convert.ToDateTime(oSC_ImportNPT.DateOfActivity).Month;
            oSC_ImportNPT.Year = Convert.ToDateTime(oSC_ImportNPT.DateOfActivity).Year;
            oSC_ImportNPT.DateUploaded = DateTime.Now;
            oSC_ImportNPT.UploadedBy = a.LogonUser;
            oSC_ImportNPT.IsActive = true;
            oSC_ImportNPT.Source = "Manual";
            bool result = this._af.blNpt.Create(oSC_ImportNPT);
            if (result == true)
            {
                ViewBag.Message = "Successfully saved!";
                return RedirectToAction("Index");
            }
            #endregion Method
            #region Return
            ViewBag.Err = this._af.blNpt._err;
            return View(oSC_ImportNPT);
            #endregion Return
        }
        #endregion Create
        #region Edit
        // GET: NptTracker/Edit/5
        public ActionResult Edit(long? id)
        {
            #region BTSS 
            try
            {
                if (Session["role"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else if (Session["logon_user"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else if (Session["grp_id"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else
                {
                    this.a = new _Access()
                    {
                        LogonUser = Session["logon_user"].ToString(),
                        Role = Session["role"].ToString(),
                        Group = Session["grp_id"].ToString(),
                        CanEdit = this._af.blAccess.CanEdit(Session["grp_id"].ToString(), module)
                    };
                    if (!this.a.CanEdit) return View("~/Views/Shared/NoAccess.cshtml");
                    else ViewBag.Access = this.a;
                }
            }
            catch (Exception ex)
            {
                _ErrorMessage err = new _ErrorMessage();
                err.MessageType = "Server Error - NPTTrackerController";
                err.MessageDescription = ex.Message.ToString();
                ViewBag.Error = err;
                return View("~/Views/Shared/SomethingWentWrong.cshtml");
            }
            #endregion BTSS
            #region Method
            if (id == null)
            {
                _ErrorMessage err = new _ErrorMessage()
                {
                    MessageType = "Server Error - NPTTrackerController",
                    MessageDescription = "Invalid use of null"
                };
                ViewBag.Error = err;
                return View("~/Views/Shared/SomethingWentWrong.cshtml");
            }
            //get
            OSC_ImportNPT oSC_ImportNPT = this._af.blNpt.Get((long)id);
            //if bl npt error exist
            if (this._af.blNpt._err == null)
            {
                ViewBag.Error = this._af.blNpt._err;
                return View("~/Views/Shared/SomethingWentWrong.cshtml");
            }
            //if record not exist
            if (oSC_ImportNPT == null) return View("~/Views/Shared/NotFound.cshtml");
            #endregion Method
            #region Return
            ViewBag.TeamNptCategories = new SelectList(_af.blTeamNptCategories.GetList((long)oSC_ImportNPT.TeamId), "CategoryDesc", "CategoryDesc");
            return View(oSC_ImportNPT);
            #endregion Return
        }

        // POST: NptTracker/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NPTReportId,RepId,Activity,DateOfActivity,TimeSpent,TypeOfActivity,CreatedBy,ItemType,Path,TeamId,Month,Year,DateUploaded,UploadedBy,Source,CategoryId,SubCategoryId,IsActive")] OSC_ImportNPT oSC_ImportNPT)
        {
            #region BTSS 
            try
            {
                if (Session["role"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else if (Session["logon_user"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else if (Session["grp_id"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else
                {
                    this.a = new _Access()
                    {
                        LogonUser = Session["logon_user"].ToString(),
                        Role = Session["role"].ToString(),
                        Group = Session["grp_id"].ToString(),
                        CanEdit = this._af.blAccess.CanEdit(Session["grp_id"].ToString(), module)
                    };
                    if (!this.a.CanEdit) return View("~/Views/Shared/NoAccess.cshtml");
                    else ViewBag.Access = this.a;
                }
            }
            catch (Exception ex)
            {
                _ErrorMessage err = new _ErrorMessage();
                err.MessageType = "Server Error - NPTTrackerController";
                err.MessageDescription = ex.Message.ToString();
                ViewBag.Error = err;
                return View("~/Views/Shared/SomethingWentWrong.cshtml");
            }
            #endregion BTSS
            #region Method
            oSC_ImportNPT.Month = Convert.ToDateTime(oSC_ImportNPT.DateOfActivity).Month;
            oSC_ImportNPT.Year = Convert.ToDateTime(oSC_ImportNPT.DateOfActivity).Year;
            oSC_ImportNPT.DateUploaded = DateTime.Now;
            oSC_ImportNPT.UploadedBy = a.LogonUser;
            oSC_ImportNPT.Source = "Manual";
            if (a.Role != "Admin") oSC_ImportNPT.IsActive = true;
            bool result = this._af.blNpt.Edit(oSC_ImportNPT);
            if (result == false && this._af.blNpt._err != null)
            {
                ViewBag.Error = this._af.blNpt._err;
                return View("~/Views/Shared/SomethingWentWrong.cshtml");
            }
            #endregion Method
            #region Return
            ViewBag.Message = "Successfully saved!";
            ViewBag.TeamNptCategories = new SelectList(_af.blTeamNptCategories.GetList((long)oSC_ImportNPT.TeamId), "CategoryDesc", "CategoryDesc");
            OSC_ImportNPT returnObj = this._af.blNpt.Get(oSC_ImportNPT.NPTReportId);
            return View(returnObj);
            #endregion Return
        }
        #endregion Edit
        #region Delete
        // GET: NptTracker/Delete/5
        public ActionResult Delete(long? id)
        {
            #region BTSS 
            try
            {
                if (Session["role"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else if (Session["logon_user"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else if (Session["grp_id"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else
                {
                    this.a = new _Access()
                    {
                        LogonUser = Session["logon_user"].ToString(),
                        Role = Session["role"].ToString(),
                        Group = Session["grp_id"].ToString(),
                        CanDelete = this._af.blAccess.CanDelete(Session["grp_id"].ToString(), module)
                    };
                    if (!this.a.CanDelete) return View("~/Views/Shared/NoAccess.cshtml");
                    else ViewBag.Access = this.a;
                }
            }
            catch (Exception ex)
            {
                _ErrorMessage err = new _ErrorMessage();
                err.MessageType = "Server Error - NPTTrackerController";
                err.MessageDescription = ex.Message.ToString();
                ViewBag.Error = err;
                return View("~/Views/Shared/SomethingWentWrong.cshtml");
            }
            #endregion BTSS
            #region Method
            if (id == null)
            {
                _ErrorMessage err = new _ErrorMessage()
                {
                    MessageType = "Server Error - NPTTrackerController",
                    MessageDescription = "Invalid use of null"
                };
                ViewBag.Error = err;
                return View("~/Views/Shared/SomethingWentWrong.cshtml");
            }
            //get
            OSC_ImportNPT oSC_ImportNPT = this._af.blNpt.Get((long)id);
            //if bl npt error exist
            if (this._af.blNpt._err == null)
            {
                ViewBag.Error = this._af.blNpt._err;
                return View("~/Views/Shared/SomethingWentWrong.cshtml");
            }
            //if record not exist
            if (oSC_ImportNPT == null) return View("~/Views/Shared/NotFound.cshtml");
            #endregion Method
            #region Return
            return View(oSC_ImportNPT);
            #endregion Return
        }

        // POST: NptTracker/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            #region BTSS 
            try
            {
                if (Session["role"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else if (Session["logon_user"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else if (Session["grp_id"] == null)
                    return View("~/Views/Shared/SessionTimeout.cshtml");
                else
                {
                    this.a = new _Access()
                    {
                        LogonUser = Session["logon_user"].ToString(),
                        Role = Session["role"].ToString(),
                        Group = Session["grp_id"].ToString(),
                        CanDelete = this._af.blAccess.CanDelete(Session["grp_id"].ToString(), module)
                    };
                    if (!this.a.CanDelete) return View("~/Views/Shared/NoAccess.cshtml");
                    else ViewBag.Access = this.a;
                }
            }
            catch (Exception ex)
            {
                _ErrorMessage err = new _ErrorMessage();
                err.MessageType = "Server Error - NPTTrackerController";
                err.MessageDescription = ex.Message.ToString();
                ViewBag.Error = err;
                return View("~/Views/Shared/SomethingWentWrong.cshtml");
            }
            #endregion BTSS
            #region Method
            OSC_ImportNPT oSC_ImportNPT = this._af.blNpt.Get(id);
            oSC_ImportNPT.DateUploaded = DateTime.Now;
            oSC_ImportNPT.UploadedBy = a.LogonUser;
            oSC_ImportNPT.IsActive = false;
            bool result = this._af.blNpt.Edit(oSC_ImportNPT);
            if (result == false && this._af.blNpt._err != null)
            {
                ViewBag.Error = this._af.blNpt._err;
                return View("~/Views/Shared/SomethingWentWrong.cshtml");
            }
            #endregion Method
            #region Return
            ViewBag.Message = "Successfully saved!";
            return RedirectToAction("Index");
            #endregion Return
        }
        #endregion Delete
        #endregion Method
    }
}
