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

        public DataSet GetData(string table)
        {
            string sqlCon = ConfigurationManager.ConnectionStrings["oscdb"].ToString();
            SqlConnection dbConnection = new SqlConnection(sqlCon);
            DataSet resultSet = null;

            try
            {

            }
            catch (SqlException sqlException)
            {
                _errorType = "SQL Error";
                _errorMessage = sqlException.Message.ToString();
            }
            catch (Exception exception)
            {
                _errorType = "Error";
                _errorMessage = exception.Message.ToString();
            }
            finally
            {

            }
        }
    }
}
