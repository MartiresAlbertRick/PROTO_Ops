using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTSS_Auth
{
    public class BTSS_BE
    {
        public class set_user
        {
            public string user_id { get; set; }
            public string user_name { get; set; }
            public string user_last_name { get; set; }
            public string user_first_name { get; set; }
            public string user_middle_name { get; set; }
            public bool can_prod { get; set; }
            public bool can_uat { get; set; }
            public bool can_peer { get; set; }
            public bool can_dev { get; set; }
            public DateTime created_date { get; set; }
        }

        public class set_user_access
        {
            public string user_id { get; set; }
            public string grp_id { get; set; }
        }

        public class set_group
        {
            public string grp_id { get; set; }
            public string grp_name { get; set; }
            public string grp_desc { get; set; }
            public DateTime created_date { get; set; }
        }

        public class set_group_access
        {
            public string grp_id { get; set; }
            public string mod_id { get; set; }
            public bool can_view { get; set; }
            public bool can_add { get; set; }
            public bool can_edit { get; set; }
            public bool can_delete { get; set; }
        }

        public class set_module
        {
            public string mod_id { get; set; }
            public string mod_name { get; set; }
            public string mod_desc { get; set; }
            public DateTime created_date { get; set; }
        }

        public class AppConst
        {
            public const string TABLE_SETUSER = "SELECT * FROM SET_USER";
            public const string TABLE_SETUSERACCESS = "SELECT * FROM SET_USER_ACCESS";
            public const string TABLE_SETGROUP = "SELECT * FROM SET_GROUP";
            public const string TABLE_SETGROUPACCESS = "SELECT * FROM SET_GROUP_ACCESS";
            public const string TABLE_SETMODULE = "SELECT * FROM SET_MODULE";
        }
    }
}
