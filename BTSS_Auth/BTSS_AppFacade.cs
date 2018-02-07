using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTSS_Auth
{
    public class BTSS_AppFacade
    {
        private BTSS_BL _bl;
        private List<BTSS_BE.set_user> _SetUser;
        private List<BTSS_BE.set_user_access> _SetUserAccess;
        private List<BTSS_BE.set_group> _SetGroup;
        private List<BTSS_BE.set_group_access> _SetGroupAccess;
        private List<BTSS_BE.set_module> _SetModule;

        public BTSS_AppFacade()
        {
            _bl = new BTSS_BL();
            _SetUser = new List<BTSS_BE.set_user>();
            _SetUser = _bl.SetUser();
            _SetUserAccess = new List<BTSS_BE.set_user_access>();
            _SetUserAccess = _bl.SetUserAccess();
            _SetGroup = new List<BTSS_BE.set_group>();
            _SetGroup = _bl.SetGroup();
            _SetGroupAccess = new List<BTSS_BE.set_group_access>();
            _SetGroupAccess = _bl.SetGroupAccess();
            _SetModule = new List<BTSS_BE.set_module>();
            _SetModule = _bl.SetModule();
        }

        public BTSS_BE.set_user GetUserInfo(string user_name)
        {
            BTSS_BE.set_user user = new BTSS_BE.set_user();
            user = this._SetUser.Where(t => t.user_name == user_name).FirstOrDefault();
            return user;
        }

        public BTSS_BE.set_user_access GetUserAccess(string user_id)
        {
            BTSS_BE.set_user_access user_access = new BTSS_BE.set_user_access();
            user_access = this._SetUserAccess.Where(t => t.user_id == user_id).FirstOrDefault();
            return user_access;
        }

        public bool IsAdmin(string grp_id)
        {
            BTSS_BE.set_group group = new BTSS_BE.set_group();
            group = this._SetGroup.Where(t => t.grp_id == grp_id).FirstOrDefault();
            if (group.grp_name == "Admin")
            {
                return true;
            }
            return false;
        }

        public bool IsManager(string grp_id)
        {
            BTSS_BE.set_group group = new BTSS_BE.set_group();
            group = this._SetGroup.Where(t => t.grp_id == grp_id).FirstOrDefault();
            if (group.grp_name == "Manager")
            {
                return true;
            }
            return false;
        }

        public bool IsTeamLeader(string grp_id)
        {
            BTSS_BE.set_group group = new BTSS_BE.set_group();
            group = this._SetGroup.Where(t => t.grp_id == grp_id).FirstOrDefault();
            if (group.grp_name == "Team Leader")
            {
                return true;
            }
            return false;
        }

        public bool IsStaff(string grp_id)
        {
            BTSS_BE.set_group group = new BTSS_BE.set_group();
            group = this._SetGroup.Where(t => t.grp_id == grp_id).FirstOrDefault();
            if (group.grp_name == "Staff")
            {
                return true;
            }
            return false;
        }

        public bool IsDepAnalyst(string grp_id)
        {
            BTSS_BE.set_group group = new BTSS_BE.set_group();
            group = this._SetGroup.Where(t => t.grp_id == grp_id).FirstOrDefault();
            if (group.grp_name == "Department Analyst")
            {
                return true;
            }
            return false;
        }

        public bool CanView(string grp_id, string mod_name)
        {
            BTSS_BE.set_group_access group_access = new BTSS_BE.set_group_access();
            group_access = this._SetGroupAccess.Where(t => t.grp_id == grp_id && t.mod_id == this.GetModule(mod_name).mod_id).FirstOrDefault();
            if (group_access != null)
            {
                return group_access.can_view;
            }
            return false;
        }

        public bool CanAdd(string grp_id, string mod_name)
        {
            BTSS_BE.set_group_access group_access = new BTSS_BE.set_group_access();
            group_access = this._SetGroupAccess.Where(t => t.grp_id == grp_id && t.mod_id == this.GetModule(mod_name).mod_id).FirstOrDefault();
            if (group_access != null)
            {
                return group_access.can_add;
            }
            return false;
        }

        public bool CanEdit(string grp_id, string mod_name)
        {
            BTSS_BE.set_group_access group_access = new BTSS_BE.set_group_access();
            group_access = this._SetGroupAccess.Where(t => t.grp_id == grp_id && t.mod_id == this.GetModule(mod_name).mod_id).FirstOrDefault();
            if (group_access != null)
            {
                return group_access.can_edit;
            }
            return false;
        }

        public bool CanDelete(string grp_id, string mod_name)
        {
            BTSS_BE.set_group_access group_access = new BTSS_BE.set_group_access();
            group_access = this._SetGroupAccess.Where(t => t.grp_id == grp_id && t.mod_id == this.GetModule(mod_name).mod_id).FirstOrDefault();
            if (group_access != null)
            { 
                return group_access.can_delete;
            }
            return false;
        }

        public BTSS_BE.set_module GetModule(string mod_name)
        {
            BTSS_BE.set_module module = new BTSS_BE.set_module();
            module = this._SetModule.Where(t => t.mod_name == mod_name).FirstOrDefault();
            return module;
        }
    }
}
