using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using OPSCO_Web.Models;
using BTSS_Auth;

namespace OPSCO_Web.Controllers
{
    public class _AppFacade
    {
        public BL_NPTTracker blNpt;
        public _AppFacade ()
        {
            blNpt = new BL_NPTTracker();
        }
        
    }

    public class BL_Department
    {

    }

    public class BL_NPTTracker
    {
        #region Variables
        private OSCContext db = new OSCContext();
        private ManagerAccess access = new ManagerAccess();
        public _ErrorMessage _err = new _ErrorMessage();
        #endregion Variables

        #region Methods
        //Get all
        public List<OSC_ImportNPT> GetList()
        {
            List<OSC_ImportNPT> result = new List<OSC_ImportNPT>();
            try
            {
                result = (from list in this.db.NPT where list.Source == "Manual" select list).ToList();
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
                _err.MessageDescription = ex.Message.ToString();
            }
            return result.OrderBy(t => t.NPTReportId).ToList();
        }

        //Filter by role and group access
        public List<OSC_ImportNPT> GetList(string user_name, string role)
        {
            List<OSC_ImportNPT> result = new List<OSC_ImportNPT>();
            if (user_name == "" || user_name == null || role == "" || role == null)
            {
                return result;
            }
            try
            {
                switch (role)
                {
                    case "Staff":
                        OSC_Representative rep = this.db.Representatives.Where(t => t.PRDUserId == user_name && t.IsActive && t.IsCurrent).FirstOrDefault();
                        if (rep != null)
                        { 
                            result = this.GetList().Where(t => t.RepId == rep.RepId && t.IsActive).ToList();
                        }
                        break;
                    case "Manager":
                    case "Department Analyst":
                    case "Team Leader":
                        List<long> list = this.access.GetList(user_name, role);
                        result = this.GetList().Where(t => list.Contains((long)t.TeamId) && t.IsActive).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                _err.MessageDescription = ex.Message.ToString();
            }
            return result.OrderBy(t => t.NPTReportId).ToList();
        }
        #endregion Methods
    }

    public class ManagerAccess
    {
        private OSCContext db = new OSCContext();

        public List<long> GetList(string user_name, string role)
        {
            List<long> result = new List<long>();
            OSC_Manager manager = db.Managers.Where(t => t.PRDUserId == user_name && t.IsActive).FirstOrDefault();
            if (manager != null)
            { 
                switch (role)
                {
                    case "Manager":
                    case "Department Analyst":
                        var dep = (from list in db.ManageGroups where list.Type == "DEPT" && list.ManagerId == manager.ManagerId select list.EntityId).ToList();
                        result = (from list in db.Teams where dep.Contains((long)list.DepartmentId) select list.TeamId).ToList();
                        break;
                    case "Team Leader":
                        result = (from list in db.ManageGroups where list.Type == "TEAM" && list.ManagerId == manager.ManagerId select list.EntityId).ToList();
                        break;
                }
            }
            return result;
        }
    }
}