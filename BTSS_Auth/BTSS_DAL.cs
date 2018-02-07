using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace BTSS_Auth
{
    public class BTSS_DAL
    {
        public string _errorType;
        public string _errorMessage;

        public DataTable GetData(string table)
        {
            string sqlCon = ConfigurationManager.ConnectionStrings["oscdb"].ToString();
            SqlConnection dbConnection = new SqlConnection(sqlCon);
            DataTable resultSet = null;

            try
            {
                SqlCommand query = new SqlCommand(table, dbConnection);
                query.CommandType = CommandType.Text;
                dbConnection.Open();

                SqlDataAdapter dbAdapter = new SqlDataAdapter(query);
                resultSet = new DataTable();
                dbAdapter.Fill(resultSet);

                dbAdapter.Dispose();
                dbAdapter = null;
                query.Dispose();
                query = null;
            }
            catch (SqlException sqlException)
            {
                _errorType = "SQL Error";
                _errorMessage = sqlException.Message.ToString();
            }
            catch (Exception exception)
            {
                _errorType = "Runtime Error";
                _errorMessage = exception.Message.ToString();
            }
            finally
            {
                dbConnection.Close();
            }

            return resultSet;
        }

        public List<BTSS_BE.set_user> SetUser()
        {
            List<BTSS_BE.set_user> resultSet = new List<BTSS_BE.set_user>();
            DataTable dt = new DataTable();
            dt = this.GetData(BTSS_BE.AppConst.TABLE_SETUSER);
            foreach (DataRow dr in dt.Rows)
            {
                BTSS_BE.set_user set_user = new BTSS_BE.set_user();
                set_user.user_id = Convert.ToString(dr["user_id"]);
                set_user.user_name = Convert.ToString(dr["user_name"]);
                set_user.user_last_name = Convert.ToString(dr["user_last_name"]);
                set_user.user_first_name = Convert.ToString(dr["user_first_name"]);
                set_user.user_middle_name = Convert.ToString(dr["user_middle_name"]);
                set_user.can_prod = Convert.ToBoolean(dr["can_prod"]);
                set_user.can_uat = Convert.ToBoolean(dr["can_uat"]);
                set_user.can_peer = Convert.ToBoolean(dr["can_peer"]);
                set_user.can_dev = Convert.ToBoolean(dr["can_dev"]);
                set_user.created_date = Convert.ToDateTime(dr["created_date"]);
                resultSet.Add(set_user);
            }
            return resultSet;
        }

        public List<BTSS_BE.set_user_access> SetUserAccess()
        {
            List<BTSS_BE.set_user_access> resultSet = new List<BTSS_BE.set_user_access>();
            DataTable dt = new DataTable();
            dt = this.GetData(BTSS_BE.AppConst.TABLE_SETUSERACCESS);
            foreach (DataRow dr in dt.Rows)
            {
                BTSS_BE.set_user_access set_user_access = new BTSS_BE.set_user_access();
                set_user_access.user_id = Convert.ToString(dr["user_id"]);
                set_user_access.grp_id = Convert.ToString(dr["grp_id"]);
                resultSet.Add(set_user_access);
            }
            return resultSet;
        }

        public List<BTSS_BE.set_group> SetGroup()
        {
            List<BTSS_BE.set_group> resultSet = new List<BTSS_BE.set_group>();
            DataTable dt = new DataTable();
            dt = this.GetData(BTSS_BE.AppConst.TABLE_SETGROUP);
            foreach (DataRow dr in dt.Rows)
            {
                BTSS_BE.set_group set_group = new BTSS_BE.set_group();
                set_group.grp_id = Convert.ToString(dr["grp_id"]);
                set_group.grp_name = Convert.ToString(dr["grp_name"]);
                set_group.grp_desc = Convert.ToString(dr["grp_desc"]);
                set_group.created_date = Convert.ToDateTime(dr["created_date"]);
                resultSet.Add(set_group);
            }
            return resultSet;
        }

        public List<BTSS_BE.set_group_access> SetGroupAccess()
        {
            List<BTSS_BE.set_group_access> resultSet = new List<BTSS_BE.set_group_access>();
            DataTable dt = new DataTable();
            dt = this.GetData(BTSS_BE.AppConst.TABLE_SETGROUPACCESS);
            foreach (DataRow dr in dt.Rows)
            {
                BTSS_BE.set_group_access set_group_access = new BTSS_BE.set_group_access();
                set_group_access.grp_id = Convert.ToString(dr["grp_id"]);
                set_group_access.mod_id = Convert.ToString(dr["mod_id"]);
                set_group_access.can_view = Convert.ToBoolean(dr["can_view"]);
                set_group_access.can_add = Convert.ToBoolean(dr["can_add"]);
                set_group_access.can_edit = Convert.ToBoolean(dr["can_edit"]);
                set_group_access.can_delete = Convert.ToBoolean(dr["can_delete"]);
                resultSet.Add(set_group_access);
            }
            return resultSet;
        }

        public List<BTSS_BE.set_module> SetModule()
        {
            List<BTSS_BE.set_module> resultSet = new List<BTSS_BE.set_module>();
            DataTable dt = new DataTable();
            dt = this.GetData(BTSS_BE.AppConst.TABLE_SETMODULE);
            foreach (DataRow dr in dt.Rows)
            {
                BTSS_BE.set_module set_module = new BTSS_BE.set_module();
                set_module.mod_id = Convert.ToString(dr["mod_id"]);
                set_module.mod_name = Convert.ToString(dr["mod_name"]);
                set_module.mod_desc = Convert.ToString(dr["mod_desc"]);
                set_module.created_date = Convert.ToDateTime(dr["created_date"]);
                resultSet.Add(set_module);
            }
            return resultSet;
        }
    }
}
