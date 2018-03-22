using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using OPSCO_Web.Models;
using BTSS_Auth;



namespace OPSCO_Web.Controllers
{
    public class _AppFacade
    {
        public BL_Team blTeam;
        public BL_Representative blRepresentative;
        public BL_NptTracker blNpt;
        public BL_NptCategory blNptCategory;
        public BL_TeamNptCategories blTeamNptCategories;
        public BTSS_AppFacade blAccess;

        public _AppFacade()
        {
            blTeam = new BL_Team();
            blRepresentative = new BL_Representative();
            blNpt = new BL_NptTracker();
            blNptCategory = new BL_NptCategory();
            blTeamNptCategories = new BL_TeamNptCategories();
            blAccess = new BTSS_AppFacade();
        }
    }

    #region BL_Department
    public class BL_Department
    {

    }
    #endregion BL_Department
    #region BL_Team
    public class BL_Team : Controller
    {
        #region Variables
        private OSCContext db;
        private ManagerAccess access;
        public _ErrorMessage _err;
        public List<OSC_Team> idb;
        #endregion Variables
        #region Constructor
        public BL_Team()
        {
            db = new OSCContext();
            access = new ManagerAccess();
            _err = new _ErrorMessage();
            idb = this.GetList();
        }
        #endregion Constructor
        #region Methods
        //Get all
        public List<OSC_Team> GetList()
        {
            List<OSC_Team> result = new List<OSC_Team>();

            try
            {
                result = (from list in this.db.Teams select list).ToList();
                foreach (OSC_Team item in result)
                {
                    item.Department = db.Departments.Find(item.DepartmentId);
                }
            }
            catch (Exception ex)
            {
                _err.MessageType = "Server Error";
                _err.MessageDescription = ex.Message.ToString();
                return result;
            }
            return result.OrderBy(t => t.TeamName).ToList();
        }

        //Filter by role and group access
        public List<OSC_Team> GetList(_Access paramAccess)
        {
            List<OSC_Team> result = new List<OSC_Team>();
            if (paramAccess.LogonUser == "" || paramAccess.LogonUser == null || paramAccess.Role == "" || paramAccess.Role == null) return result;
            try
            {
                switch (paramAccess.Role)
                {
                    case "Admin":
                        result = this.idb;
                        break;
                    case "Staff":
                        OSC_Representative rep = this.db.Representatives.Where(t => t.PRDUserId == paramAccess.LogonUser && t.IsActive && t.IsCurrent).FirstOrDefault();
                        if (rep != null) result = this.GetList().Where(t => t.TeamId == rep.TeamId && t.IsActive).ToList();
                        break;
                    case "Manager":
                    case "Department Analyst":
                    case "Team Leader":
                        List<long> list = this.access.GetList(paramAccess.LogonUser, paramAccess.Role, "TEAM");
                        result = this.GetList().Where(t => list.Contains((long)t.TeamId) && t.IsActive).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                _err.MessageType = "Server Error";
                _err.MessageDescription = ex.Message.ToString();
                return result;
            }
            return result.OrderBy(t => t.TeamName).ToList();
        }
        #endregion Methods
        #region Destructor
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion Destructor
    }
    #endregion BT_Team
    #region BL_Representative
    public class BL_Representative : Controller
    {
        #region Variables
        private OSCContext db;
        private ManagerAccess access;
        public _ErrorMessage _err;
        public List<OSC_Representative> idb;
        #endregion Variables
        #region Constructor
        public BL_Representative()
        {
            db = new OSCContext();
            access = new ManagerAccess();
            _err = new _ErrorMessage();
            idb = this.GetList();
        }
        #endregion Constructor
        #region Methods
        //Get all
        public List<OSC_Representative> GetList()
        {
            List<OSC_Representative> result = new List<OSC_Representative>();
                    
            try
            {
                result = (from list in this.db.Representatives select list).ToList();
                foreach (OSC_Representative item in result)
                {
                    item.Team = db.Teams.Find(item.TeamId);
                    item.Location = db.Locations.Find(item.LocationId);
                    item.CoreRole = db.CoreRoles.Find(item.CoreRoleId);
                    item.FullName = item.FirstName + " " + item.LastName;
                }
            }
            catch (Exception ex)
            {
                _err.MessageType = "Server Error";
                _err.MessageDescription = ex.Message.ToString();
                return result;
            }
            return result.OrderBy(t => t.TeamId).ThenBy(t => t.FullName).ToList();
        }

        //Filter by role and group access
        public List<OSC_Representative> GetList(_Access paramAccess)
        {
            List<OSC_Representative> result = new List<OSC_Representative>();
            if (paramAccess.LogonUser == "" || paramAccess.LogonUser == null || paramAccess.Role == "" || paramAccess.Role == null) return result;
            try
            {
                switch (paramAccess.Role)
                {
                    case "Admin":
                        result = this.idb;
                        break;
                    case "Staff":
                        OSC_Representative rep = this.db.Representatives.Where(t => t.PRDUserId == paramAccess.LogonUser && t.IsActive && t.IsCurrent).FirstOrDefault();
                        if (rep != null) result = this.GetList().Where(t => t.RepId == rep.RepId && t.IsActive).ToList();
                        break;
                    case "Manager":
                    case "Department Analyst":
                    case "Team Leader":
                        List<long> list = this.access.GetList(paramAccess.LogonUser, paramAccess.Role, "TEAM");
                        result = this.GetList().Where(t => list.Contains((long)t.TeamId) && t.IsActive).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                _err.MessageType = "Server Error";
                _err.MessageDescription = ex.Message.ToString();
                return result;
            }
            return result.OrderBy(t => t.TeamId).ThenBy(t => t.FullName).ToList();
        }
        #endregion Methods
        #region Destructor
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion Destructor
    }
    #endregion BL_Representative
    #region BL_NPTTracker
    public class BL_NptTracker : Controller
    {
        #region Variables
        private OSCContext db;
        private ManagerAccess access;
        public _ErrorMessage _err;
        public List<OSC_ImportNPT> idb;
        #endregion Variables
        #region Constructor
        public BL_NptTracker()
        {
            db = new OSCContext();
            access = new ManagerAccess();
            _err = new _ErrorMessage();
            idb = this.GetList();
        }
        #endregion Constructor
        #region Methods
        //Get all
        public List<OSC_ImportNPT> GetList()
        {
            List<OSC_ImportNPT> result = new List<OSC_ImportNPT>();
            try
            {
                result = (from list in this.db.NPT where list.Source == "Manual" select list).AsNoTracking().ToList();
                foreach (OSC_ImportNPT item in result)
                {
                    item.Team = db.Teams.Find(item.TeamId);
                    item.Representative = db.Representatives.Find(item.RepId);
                    item.Representative.Location = db.Locations.Find(item.Representative.LocationId);
                    item.Representative.CoreRole = db.CoreRoles.Find(item.Representative.CoreRoleId);
                    item.Representative.FullName = item.Representative.FirstName + " " + item.Representative.LastName;
                }
            }
            catch (Exception ex)
            {
                _err.MessageType = "Server Error";
                _err.MessageDescription = ex.Message.ToString();
                return result;
            }
            return result.OrderBy(t => t.NPTReportId).ToList();
        }

        //Filter by role and group access
        public List<OSC_ImportNPT> GetList(_Access paramAccess)
        {
            List<OSC_ImportNPT> result = new List<OSC_ImportNPT>();
            if (paramAccess.LogonUser == "" || paramAccess.LogonUser == null || paramAccess.Role == "" || paramAccess.Role == null) return result;
            try
            {
                switch (paramAccess.Role)
                {
                    case "Admin":
                        result = this.idb;
                        break;
                    case "Staff":
                        OSC_Representative rep = this.db.Representatives.Where(t => t.PRDUserId == paramAccess.LogonUser && t.IsActive && t.IsCurrent).FirstOrDefault();
                        if (rep != null) result = this.GetList().Where(t => t.RepId == rep.RepId && t.IsActive).ToList();
                        break;
                    case "Manager":
                    case "Department Analyst":
                    case "Team Leader":
                        List<long> list = this.access.GetList(paramAccess.LogonUser, paramAccess.Role, "TEAM");
                        result = this.GetList().Where(t => list.Contains((long)t.TeamId) && t.IsActive).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                _err.MessageType = "Server Error";
                _err.MessageDescription = ex.Message.ToString();
                return result;
            }
            return result.OrderBy(t => t.NPTReportId).ToList();
        }

        //Filter list
        public List<OSC_ImportNPT> FilterList(List<OSC_ImportNPT> list, List<_Filter> filters)
        {
            List<OSC_ImportNPT> result = new List<OSC_ImportNPT>();
            try
            {
                if (filters.Count == 0) return list;

                
            }
            catch (Exception ex)
            {
                _err.MessageType = "Server Error";
                _err.MessageDescription = ex.Message.ToString();
                return list;
            }
            return result.OrderBy(t => t.NPTReportId).ToList();
        }

        //Get single record
        public OSC_ImportNPT Get(long id)
        {
            OSC_ImportNPT result = new OSC_ImportNPT();
            try
            {
                result = this.idb.Where(t => t.NPTReportId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _err.MessageType = "Server Error";
                _err.MessageDescription = ex.Message.ToString();
                return result;
            }
            return result;
        }

        //Create
        public bool Create(OSC_ImportNPT paramObj)
        {
            bool result = false;
            try
            {
                if (ModelState.IsValid)
                {
                    db.NPT.Add(paramObj);
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                _err.MessageType = "Server Error";
                _err.MessageDescription = ex.Message.ToString();
                return result;
            }
            return result;
        }

        //Edit
        public bool Edit(OSC_ImportNPT paramObj)
        {
            bool result = false;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(paramObj).State = EntityState.Modified;
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                _err.MessageType = "Server Error";
                _err.MessageDescription = ex.Message.ToString();
                return result;
            }
            return result;
        }

        //Delete
        public bool Delete(long id)
        {
            OSC_ImportNPT obj = new OSC_ImportNPT();
            bool result = false;
            try
            { 
                obj = this.Get(id);
                if (obj != null)
                {
                    db.NPT.Remove(obj);
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                _err.MessageType = "Server Error";
                _err.MessageDescription = ex.Message.ToString();
                return result;
            }
            return result;
        }
        #endregion Methods
        #region Destructor
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion Destructor
    }
    #endregion BL_NPTTracker
    #region BL_NptCategory
    public class BL_NptCategory : Controller
    {
        #region Variables
        private OSCContext db;
        public _ErrorMessage _err;
        public List<OSC_NptCategory> idb;
        #endregion Variables
        #region Constructor
        public BL_NptCategory()
        {
            db = new OSCContext();
            _err = new _ErrorMessage();
            idb = this.GetList();
        }
        #endregion Constructor
        #region Methods
        //Get all
        public List<OSC_NptCategory> GetList()
        {
            List<OSC_NptCategory> result = new List<OSC_NptCategory>();

            try
            {
                result = (from list in this.db.NptCategories select list).ToList();
            }
            catch (Exception ex)
            {
                _err.MessageType = "Server Error";
                _err.MessageDescription = ex.Message.ToString();
                return result;
            }
            return result.OrderBy(t => t.CategoryDesc).ToList();
        }

        //Filter by teamId
        public List<OSC_NptCategory> GetList(_Access paramAccess)
        {
            List<OSC_NptCategory> result = new List<OSC_NptCategory>();
            try
            {
                switch (paramAccess.Role)
                {
                    case "Admin":
                        result = this.idb;
                        break;
                    case "Manager":
                    case "Team Leader":
                    case "Department Analyst":
                    case "Staff":
                        result = (from list in this.idb where list.IsActive select list).ToList();
                        break;
                }
                
            }
            catch (Exception ex)
            {
                _err.MessageType = "Server Error";
                _err.MessageDescription = ex.Message.ToString();
                return result;
            }
            return result.OrderBy(t => t.CategoryDesc).ToList();
        }
        #endregion Methods
        #region Destructor
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion Destructor
    }
    #endregion BT_NptCategory
    #region BL_TeamNptCategories
    public class BL_TeamNptCategories : Controller
    {
        #region Variables
        private OSCContext db;
        public _ErrorMessage _err;
        public List<OSC_TeamNptCategory> idb;
        #endregion Variables
        #region Constructor
        public BL_TeamNptCategories()
        {
            db = new OSCContext();
            _err = new _ErrorMessage();
            idb = this.GetList();
        }
        #endregion Constructor
        #region Methods
        //Get all
        public List<OSC_TeamNptCategory> GetList()
        {
            List<OSC_TeamNptCategory> result = new List<OSC_TeamNptCategory>();

            try
            {
                result = (from list in this.db.TeamNptCategories select list).ToList();
                foreach (OSC_TeamNptCategory item in result)
                {
                    item.CategoryDesc = db.NptCategories.Find(item.CategoryId).CategoryDesc;
                }
            }
            catch (Exception ex)
            {
                _err.MessageType = "Server Error";
                _err.MessageDescription = ex.Message.ToString();
                return result;
            }
            return result.OrderBy(t => t.TeamId).ThenBy(t => t.CategoryDesc).ToList();
        }

        //Filter by teamId
        public List<OSC_TeamNptCategory> GetList(long teamId)
        {
            List<OSC_TeamNptCategory> result = new List<OSC_TeamNptCategory>();
            try
            {
                result = (from list in this.idb where list.TeamId == teamId select list).ToList();
            }
            catch (Exception ex)
            {
                _err.MessageType = "Server Error";
                _err.MessageDescription = ex.Message.ToString();
                return result;
            }
            return result.OrderBy(t => t.TeamId).ThenBy(t => t.CategoryDesc).ToList();
        }
        #endregion Methods
        #region Destructor
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion Destructor
    }
    #endregion BT_TeamNptCategories
    #region BL_Manager
    public class ManagerAccess : Controller
    {
        #region Variables
        private OSCContext db;
        #endregion Variables
        #region Contructor
        public ManagerAccess()
        {
             db = new OSCContext();
        }
        #endregion Constructor
        #region Methods
        public List<long> GetList(string user_name, string role, string lookup = "")
        {
            List<long> result = new List<long>();
            if (lookup == "") return result;
            if (user_name == "" || user_name == null || role == "" || role == null) return result;
            OSC_Manager manager = db.Managers.Where(t => t.PRDUserId == user_name && t.IsActive).FirstOrDefault();
            if (manager != null)
            { 
                switch (role)
                {
                    case "Manager":
                    case "Department Analyst":
                        var dep = (from list in db.ManageGroups where list.Type == "DEPT" && list.ManagerId == manager.ManagerId select list.EntityId).ToList();
                        if (lookup == "TEAM")
                        {
                            result = (from list in db.ManageGroups where list.Type == "TEAM" && list.ManagerId == manager.ManagerId select list.EntityId).ToList();
                            if (dep.Count > 0)
                            {
                                var s = (from list in db.Teams where dep.Contains((long)list.DepartmentId) select list.TeamId).ToList();
                                foreach (long x in s)
                                {
                                    result.Add(x);
                                }
                            }
                            
                        }
                        else
                        {
                            result = dep;
                        }
                        break;
                    case "Team Leader":
                        if (lookup == "TEAM")
                        { 
                            result = (from list in db.ManageGroups where list.Type == "TEAM" && list.ManagerId == manager.ManagerId select list.EntityId).ToList();
                        }
                        else
                        {
                            result = null;
                        }
                        break;
                }
            }
            return result;
        }
        #endregion Methods
        #region Destructor
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion Destructor
    }
    #endregion BL_Manager

}