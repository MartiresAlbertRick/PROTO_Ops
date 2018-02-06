using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using OSC_BusinessEntity;

namespace OSC_DataAccessLayer
{
    public class TeamDAL : _AbsComDAL
    {
        #region "ClassVariables"
        public Team team;
        public _ResultMessage resultMessage;
        #endregion "ClassVariables"

        #region "Constructor"
        public TeamDAL()
        {
            team = new Team();
            resultMessage = new _ResultMessage();
        }
        #endregion "Constructor"

        #region "Methods"
        public virtual long Create()
        {
            long status = 0;
            var ArrayOfParameters = new SqlCommand().Parameters;
            ArrayOfParameters.AddWithValue(_AppConstants.PAR02_TEAM, team.TeamName);
            ArrayOfParameters.AddWithValue(_AppConstants.PAR03_TEAM, team.DepartmentId);
            status = this.ModifyDataStoredProcedureWithReturn(_AppConstants.SP_TEAM_CREATE, ArrayOfParameters);
            if (status == 0)
            {
                this.resultMessage.MessageType = this._errorType;
                switch (this._errorType)
                {
                    case "Runtime Error":
                        this.resultMessage.MessageContentExc = this._errorContentExc;
                        break;
                    case "SQL Error":
                        this.resultMessage.MessageContentSqlExc = this._errorContentSqlExc;
                        break;
                }
            }
            return status;
        }

        public virtual int Update()
        {
            int status = 0;
            var ArrayOfParameters = new SqlCommand().Parameters;
            ArrayOfParameters.AddWithValue(_AppConstants.PAR01_TEAM, team.TeamId);
            ArrayOfParameters.AddWithValue(_AppConstants.PAR02_TEAM, team.TeamName);
            ArrayOfParameters.AddWithValue(_AppConstants.PAR03_TEAM, team.DepartmentId);
            ArrayOfParameters.AddWithValue(_AppConstants.PAR04_TEAM, team.IsActive);
            status = this.ModifyDataStoredProcedure(_AppConstants.SP_TEAM_UPDATE, ArrayOfParameters);
            if (status == 0)
            {
                this.resultMessage.MessageType = this._errorType;
                switch (this._errorType)
                {
                    case "Runtime Error":
                        this.resultMessage.MessageContentExc = this._errorContentExc;
                        break;
                    case "SQL Error":
                        this.resultMessage.MessageContentSqlExc = this._errorContentSqlExc;
                        break;
                } 
            } 
            return status;
        } 

        public virtual int Delete()
        {
            int status = 0;
            var ArrayOfParameters = new SqlCommand().Parameters;
            ArrayOfParameters.AddWithValue(_AppConstants.PAR01_TEAM, team.TeamId);
            status = this.ModifyDataStoredProcedure(_AppConstants.SP_TEAM_DELETE, ArrayOfParameters);
            if (status == 0)
            {
                this.resultMessage.MessageType = this._errorType;
                switch (this._errorType)
                {
                    case "Runtime Error":
                        this.resultMessage.MessageContentExc = this._errorContentExc;
                        break;
                    case "SQL Error":
                        this.resultMessage.MessageContentSqlExc = this._errorContentSqlExc;
                        break;
                } 
            } 
            return status;
        } 

        public virtual List<Team> Get()
        {
            List<Team> result = new List<Team>();
            DataTable dt = new DataTable();
            dt = this.GetDataStoredProcedureDataTable(_AppConstants.SP_TEAM_GET);
            foreach (DataRow dr in dt.Rows)
            {
                Team team = new Team();
                team.TeamId = Convert.ToInt64(dr[0]);
                team.TeamName = Convert.ToString(dr[1]);
                team.DepartmentId = Convert.ToInt64(dr[2]);
                team.IsActive = Convert.ToBoolean(dr[5]);
                result.Add(team);
            } 
            return result;
        } 
        #endregion "Methods"
    } 
} 
