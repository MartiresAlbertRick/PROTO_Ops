using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace OSC_DataAccessLayer
{
    public abstract class _AbsComDAL
    {

        #region "Private Member Variables"

        private string _serverInfo;
        private SqlConnection _dbConnection;
        private string _errorMessage;

        #endregion
        
        #region "Public Member Variables"
        public string _errorType;
        public Exception _errorContentExc;
        public SqlException _errorContentSqlExc;
        #endregion

        #region "Private Methods"

        private void GetDBConnection()
        {
            //for testing only comment after use
            //this._serverInfo = "Data Source=JHJHST59\\JHS1D;Initial Catalog=dbbtOSCp1;Trusted_Connection=YES;";
            this._serverInfo = ConfigurationManager.ConnectionStrings["oscdb"].ToString();
            this._dbConnection = new SqlConnection(this._serverInfo);
        }

        private void CloseConnection()
        {

            this._dbConnection.Close();
            this._serverInfo = "";

        }

        #endregion

        #region "Public Methods"


        //For stored procedure that return DataSet value
        public DataSet GetDataStoredProcedureDataSet(string storedProcedure)
        {

            DataSet resultSet = null;

            try
            {

                this.GetDBConnection();
                SqlCommand query = new SqlCommand(storedProcedure, this._dbConnection);
                query.CommandType = CommandType.StoredProcedure;
                this._dbConnection.Open();

                SqlDataAdapter dbAdapter = new SqlDataAdapter(query);
                resultSet = new DataSet();
                dbAdapter.Fill(resultSet);

                dbAdapter.Dispose();
                dbAdapter = null;
                query.Dispose();
                query = null;

            }
            catch (SqlException sqlException)
            {

                _errorMessage = "SQL Error: Number - " + sqlException.Number + ", " + sqlException.Message;
                _errorType = "SQL Error";
                _errorContentSqlExc = sqlException;

            }
            catch (Exception exception)
            {

                _errorMessage = "Runtime Error - " + exception.Message;
                _errorType = "Runtime Error";
                _errorContentExc = exception;

            }
            finally
            {

                this.CloseConnection();

            }

            return resultSet;

        }


        //For stored procedure (w/ Parameter) that return a DataSet value
        public DataSet GetDataStoredProcedureDataSet(string storedProcedure, SqlParameterCollection storedProcedureParameter)
        {

            DataSet resultSet = null;

            try
            {

                this.GetDBConnection();
                SqlCommand query = new SqlCommand(storedProcedure, this._dbConnection);
                query.CommandType = CommandType.StoredProcedure;

                foreach (SqlParameter parameter in storedProcedureParameter)
                {

                    query.Parameters.Add(parameter.ParameterName, parameter.SqlDbType).Value = parameter.Value;

                }

                this._dbConnection.Open();

                SqlDataAdapter dbAdapter = new SqlDataAdapter(query);
                resultSet = new DataSet();
                dbAdapter.Fill(resultSet);

                dbAdapter.Dispose();
                dbAdapter = null;
                query.Dispose();
                query = null;

            }
            catch (SqlException sqlException)
            {

                _errorMessage = "SQL Error: Number - " + sqlException.Number + ", " + sqlException.Message;
                _errorType = "SQL Error";
                _errorContentSqlExc = sqlException;

            }
            catch (Exception exception)
            {

                _errorMessage = "Runtime Error - " + exception.Message;
                _errorType = "Runtime Error";
                _errorContentExc = exception;

            }
            finally
            {

                this.CloseConnection();

            }

            return resultSet;
        }


        //For stored procedure that return DataTable value
        public DataTable GetDataStoredProcedureDataTable(string storedProcedure)
        {

            DataTable resultSet = null;

            try
            {

                this.GetDBConnection();
                SqlCommand query = new SqlCommand(storedProcedure, this._dbConnection);
                query.CommandType = CommandType.StoredProcedure;
                this._dbConnection.Open();

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

                _errorMessage = "SQL Error: Number - " + sqlException.Number + ", " + sqlException.Message;
                _errorType = "SQL Error";
                _errorContentSqlExc = sqlException;

            }
            catch (Exception exception)
            {

                _errorMessage = "Runtime Error - " + exception.Message;
                _errorType = "Runtime Error";
                _errorContentExc = exception;

            }
            finally
            {

                this.CloseConnection();

            }

            return resultSet;

        }


        //For stored procedure (w/ Parameter) that return a DataTable value
        public DataTable GetDataStoredProcedureDataTable(string storedProcedure, SqlParameterCollection storedProcedureParameter)
        {

            DataTable resultSet = null;

            try
            {

                this.GetDBConnection();
                SqlCommand query = new SqlCommand(storedProcedure, this._dbConnection);
                query.CommandType = CommandType.StoredProcedure;

                foreach (SqlParameter parameter in storedProcedureParameter)
                {

                    query.Parameters.Add(parameter.ParameterName, parameter.SqlDbType).Value = parameter.Value;

                }

                this._dbConnection.Open();

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

                _errorMessage = "SQL Error: Number - " + sqlException.Number + ", " + sqlException.Message;
                _errorType = "SQL Error";
                _errorContentSqlExc = sqlException;

            }
            catch (Exception exception)
            {

                _errorMessage = "Runtime Error - " + exception.Message;
                _errorType = "Runtime Error";
                _errorContentExc = exception;

            }
            finally
            {

                this.CloseConnection();

            }

            return resultSet;
        }


        //For stored procedure does not return resultset value only integer
        //return value will only signify if store procedure is succesfull (1) or not (0)
        public int ModifyDataStoredProcedure(string storedProcedure)
        {

            int status = 0;

            try
            {

                this.GetDBConnection();
                this._dbConnection.Open();
                SqlTransaction transaction;
                transaction = this._dbConnection.BeginTransaction();
                SqlCommand query = new SqlCommand(storedProcedure, this._dbConnection, transaction);
                query.CommandType = CommandType.StoredProcedure;
                query.ExecuteNonQuery();
                transaction.Commit();

                transaction.Dispose();
                transaction = null;
                query.Dispose();
                query = null;
                status = 1;
            }
            catch (SqlException sqlException)
            {

                _errorMessage = "SQL Error: Number - " + sqlException.Number + ", " + sqlException.Message;
                _errorType = "SQL Error";
                _errorContentSqlExc = sqlException;

            }
            catch (Exception exception)
            {

                _errorMessage = "Runtime Error - " + exception.Message;
                _errorType = "Runtime Error";
                _errorContentExc = exception;

            }
            finally
            {

                this.CloseConnection();

            }

            return status;

        }


        //For stored procedure (w/ Parameter) does not return resultset value only integer
        //return value will only signify if store procedure is succesfull (1) or not (0)
        public int ModifyDataStoredProcedure(string storedProcedure, SqlParameterCollection storedProcedureParameter)
        {

            int status = 0;

            try
            {

                this.GetDBConnection();
                this._dbConnection.Open();
                SqlTransaction transaction;
                transaction = this._dbConnection.BeginTransaction();
                SqlCommand query = new SqlCommand(storedProcedure, this._dbConnection, transaction);
                query.CommandType = CommandType.StoredProcedure;
                query.CommandTimeout = 1000;
                foreach (SqlParameter parameter in storedProcedureParameter)
                {

                    query.Parameters.Add(parameter.ParameterName, parameter.SqlDbType).Value = parameter.Value;

                }

                query.ExecuteNonQuery();
                transaction.Commit();

                transaction.Dispose();
                transaction = null;
                query.Dispose();
                query = null;
                status = 1;
            }
            catch (SqlException sqlException)
            {

                _errorMessage = "SQL Error: Number - " + sqlException.Number + ", " + sqlException.Message;
                _errorType = "SQL Error";
                _errorContentSqlExc = sqlException;

            }
            catch (Exception exception)
            {

                _errorMessage = "Runtime Error - " + exception.Message;
                _errorType = "Runtime Error";
                _errorContentExc = exception;

            }
            finally
            {

                this.CloseConnection();

            }

            return status;

        }

        //For stored procedure does not return resultset value only integer 
        //(stored procedure must use "SELECT @integer" for return value)
        public int ModifyDataStoredProcedureWithReturn(string storedProcedure)
        {

            int status = 0;

            try
            {

                this.GetDBConnection();
                SqlCommand query = new SqlCommand(storedProcedure, this._dbConnection);
                query.CommandType = CommandType.StoredProcedure;
                this._dbConnection.Open();

                SqlDataReader dbReader = query.ExecuteReader();
                dbReader.Read();
                status = dbReader.GetInt32(0);

                dbReader.Dispose();
                dbReader = null;
                query.Dispose();
                query = null;

            }
            catch (SqlException sqlException)
            {

                _errorMessage = "SQL Error: Number - " + sqlException.Number + ", " + sqlException.Message;
                _errorType = "SQL Error";
                _errorContentSqlExc = sqlException;

            }
            catch (Exception exception)
            {

                _errorMessage = "Runtime Error - " + exception.Message;
                _errorType = "Runtime Error";
                _errorContentExc = exception;

            }
            finally
            {

                this.CloseConnection();

            }

            return status;

        }


        //For stored procedure (w/ Parameter) does not return resultset value only integer 
        //(stored procedure must use "SELECT @integer" for return value)
        public long ModifyDataStoredProcedureWithReturn(string storedProcedure, SqlParameterCollection storedProcedureParameter)
        {

            long status = 0;

            try
            {

                this.GetDBConnection();
                SqlCommand query = new SqlCommand(storedProcedure, this._dbConnection);
                query.CommandType = CommandType.StoredProcedure;
                this._dbConnection.Open();

                foreach (SqlParameter parameter in storedProcedureParameter)
                {

                    query.Parameters.Add(parameter.ParameterName, parameter.SqlDbType).Value = parameter.Value;

                }

                SqlDataReader dbReader = query.ExecuteReader();
                dbReader.Read();
                status = dbReader.GetInt64(0);

                dbReader.Dispose();
                dbReader = null;
                query.Dispose();
                query = null;

            }
            catch (SqlException sqlException)
            {

                _errorMessage = "SQL Error: Number - " + sqlException.Number + ", " + sqlException.Message;
                _errorType = "SQL Error";
                _errorContentSqlExc = sqlException;

            }
            catch (Exception exception)
            {

                _errorMessage = "Runtime Error - " + exception.Message;
                _errorType = "Runtime Error";
                _errorContentExc = exception;

            }
            finally
            {

                this.CloseConnection();

            }

            return status;

        }

        #endregion

    }
}
