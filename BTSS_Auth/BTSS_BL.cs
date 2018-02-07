using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTSS_Auth
{
    public class BTSS_BL
    {
        private BTSS_DAL _dal;

        public BTSS_BL()
        {
            _dal = new BTSS_DAL();
        }

        public List<BTSS_BE.set_user> SetUser()
        {
            return this._dal.SetUser();
        }

        public List<BTSS_BE.set_user_access> SetUserAccess()
        {
            return this._dal.SetUserAccess();
        }

        public List<BTSS_BE.set_group> SetGroup()
        {
            return this._dal.SetGroup();
        }

        public List<BTSS_BE.set_group_access> SetGroupAccess()
        {
            return this._dal.SetGroupAccess();
        }

        public List<BTSS_BE.set_module> SetModule()
        {
            return this._dal.SetModule();
        }

    }
}
