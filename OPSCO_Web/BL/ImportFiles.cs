using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Configuration;
using System.IO;
using OPSCO_Web.Models;

namespace OPSCO_Web
{
    public class ImportFiles : _AbsComDAL
    { 
        #region "Methods"
        public int ImportBIProd(string filePath, Import import, DateTime dateUploaded, string user)
        {
            int result = 0;
            OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=6'");
            con.Open();
            var obj = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            OleDbCommand cmd = new OleDbCommand("select * from [" + obj.Rows[0].Field<string>("TABLE_NAME") + "]", con);
            OleDbDataAdapter adp = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            string sqlCon = ConfigurationManager.ConnectionStrings["oscdb"].ToString();
            
            OleDbDataReader dr = cmd.ExecuteReader();
            SqlBulkCopy bulkInsert = new SqlBulkCopy(sqlCon);
            bulkInsert.DestinationTableName = "OSC_ImportBIProd_temp";
            if (dr.HasRows)
            {
                dt.Load(dr);
                dt.Columns.Add("Month");
                dt.Columns.Add("Year");
                dt.Columns.Add("DateUploaded");
                dt.Columns.Add("UploadedBy");
                string sRow="";
                foreach (DataRow row in dt.Rows)
                {
                    if (row[0].ToString() == "")
                    { row[0] = sRow; }
                    else
                    { sRow = row[0].ToString(); }

                    row["Month"] = import.Month;
                    row["Year"] = import.Year;
                    row["DateUploaded"] = dateUploaded;
                    row["UploadedBy"] = user;
                }
                bulkInsert.WriteToServer(dt);
                result = 1;
            }
            con.Close();

            #region "DeleteFile"
            //File.Delete(filePath);
            #endregion "DeleteFile"
            #region "PushImport"
            if (result == 1)
            {
                var ArrayOfParameters = new SqlCommand().Parameters;
                ArrayOfParameters.AddWithValue("@Month", import.Month);
                ArrayOfParameters.AddWithValue("@Year", import.Year);
                ArrayOfParameters.AddWithValue("@DateUploaded", dateUploaded);
                ArrayOfParameters.AddWithValue("@UploadedBy", user);

                result = this.ModifyDataStoredProcedure("OSC_ImportBIProd_Transfer", ArrayOfParameters);
            }
            #endregion "PushImport"
            return result;
        }

        public int ImportBIQual(string filePath, Import import, DateTime dateUploaded, string user)
        {
            int result = 0;
            OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=6'");
            con.Open();
            var obj = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            OleDbCommand cmd = new OleDbCommand("select * from [" + obj.Rows[0].Field<string>("TABLE_NAME") + "]", con);
            OleDbDataAdapter adp = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            string sqlCon = ConfigurationManager.ConnectionStrings["oscdb"].ToString();

            OleDbDataReader dr = cmd.ExecuteReader();
            SqlBulkCopy bulkInsert = new SqlBulkCopy(sqlCon);
            bulkInsert.DestinationTableName = "OSC_ImportBIQual_temp";
            if (dr.HasRows)
            {
                dt.Load(dr);
                dt.Columns.Add("Month");
                dt.Columns.Add("Year");
                dt.Columns.Add("DateUploaded");
                dt.Columns.Add("UploadedBy");
                string sRow = "";
                foreach (DataRow row in dt.Rows)
                {
                    if (row[0].ToString() == "")
                    { row[0] = sRow; }
                    else
                    { sRow = row[0].ToString(); }

                    row["Month"] = import.Month;
                    row["Year"] = import.Year;
                    row["DateUploaded"] = dateUploaded;
                    row["UploadedBy"] = user;
                }
                bulkInsert.WriteToServer(dt);
                result = 1;
            }
            con.Close();

            #region "DeleteFile"
            //File.Delete(filePath);
            #endregion "DeleteFile"
            #region "PushImport"
            if (result == 1)
            {
                var ArrayOfParameters = new SqlCommand().Parameters;
                ArrayOfParameters.AddWithValue("@Month", import.Month);
                ArrayOfParameters.AddWithValue("@Year", import.Year);
                ArrayOfParameters.AddWithValue("@DateUploaded", dateUploaded);
                ArrayOfParameters.AddWithValue("@UploadedBy", user);

                result = this.ModifyDataStoredProcedure("OSC_ImportBIQual_Transfer", ArrayOfParameters);
            }
            #endregion "PushImport"
            return result;
        }
        #endregion "Methods"
    }
} 
