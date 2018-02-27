﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OPSCO_Web.Models;
using BTSS_Auth;

namespace OPSCO_Web.BL
{
    public class AppFacade
    {
        private BTSS_AppFacade btssFacade;
        private DateTimeFormatting dateTimeFormatting;
        private InitializeLists initializeLists;
        private IndividualScorecardClass individualScorecardClass;
        private TeamScorecardClass teamScorecardClass;
        private BTSSExtensions btssExtensions;
        private GetReference getReference;

        public AppFacade()
        {
            btssFacade = new BTSS_AppFacade();
            dateTimeFormatting = new DateTimeFormatting();
            initializeLists = new InitializeLists();
            individualScorecardClass = new IndividualScorecardClass();
            teamScorecardClass = new TeamScorecardClass();
            btssExtensions = new BTSSExtensions();
            getReference = new GetReference();
        }

        #region "BTSSFacade"
        public BTSS_BE.set_user GetUserInfo(string user_name)
        {
            return btssFacade.GetUserInfo(user_name);
        }

        public BTSS_BE.set_user_access GetUserAccess(string user_id)
        {
            return btssFacade.GetUserAccess(user_id);
        }

        public BTSS_BE.set_module GetModule(string mod_name)
        {
            return btssFacade.GetModule(mod_name);
        }

        public bool IsAdmin(string grp_id)
        {
            return btssFacade.IsAdmin(grp_id);
        }

        public bool IsManager(string grp_id)
        {
            return btssFacade.IsManager(grp_id);
        }

        public bool IsTeamLeader(string grp_id)
        {
            return btssFacade.IsTeamLeader(grp_id);
        }

        public bool IsStaff(string grp_id)
        {
            return btssFacade.IsStaff(grp_id);
        }

        public bool IsDepAnalyst(string grp_id)
        {
            return btssFacade.IsStaff(grp_id);
        }

        public bool CanView(string grp_id, string mod_name)
        {
            return btssFacade.CanView(grp_id, mod_name);
        }

        public bool CanAdd(string grp_id, string mod_name)
        {
            return btssFacade.CanAdd(grp_id, mod_name);
        }

        public bool CanEdit(string grp_id, string mod_name)
        {
            return btssFacade.CanEdit(grp_id, mod_name);
        }

        public bool CanDelete(string grp_id, string mod_name)
        {
            return btssFacade.CanDelete(grp_id, mod_name);
        }
        #endregion "BTSSFacade"

        #region "DateTimeFormatting"
        public string GetTimeFormat(long sec)
        {
            return dateTimeFormatting.GetTimeFormat(sec);
        }

        public long GetSecondsFormat(string _hour)
        {
            return dateTimeFormatting.GetSecondsFormat(_hour);
        }
        #endregion "DateTimeFormatting"

        #region "InitializeLists"
        public bool InitializeTeams(OSCContext db)
        {
            return initializeLists.InitializeTeams(db);
        }

        public bool InitializeRepresentatives(OSCContext db)
        {
            return initializeLists.InitializeRepresentatives(db);
        }

        public bool InitializeNpts(OSCContext db)
        {
            return initializeLists.InitializeNpts(db);
        }

        public bool InitializeActivities(OSCContext db)
        {
            return initializeLists.InitializeActivities(db);
        }

        public bool InitializeManualEntries(OSCContext db)
        {
            return initializeLists.InitializeManualEntries(db);
        }

        public bool InitializeTeamNptCategories(OSCContext db)
        {
            return initializeLists.InitializeTeamNptCategories(db);
        }
        #endregion "InitializeLists"

        #region "IndividualScorecardClass"
        public List<IndividualScorecard> GetIndividualScorecardFull(long teamId, long repId, int month, int year)
        {
            return individualScorecardClass.GetIndividualScorecardFull(teamId, repId, month, year);
        }

        public IndividualScorecard GetIndividualScorecard(long teamId, long repId, int month, int year)
        {
            return individualScorecardClass.GetIndividualScorecard(teamId, repId, month, year);
        }

        public List<IndividualWorkTypes> GetIndividualWorkTypes(long teamId, long repId, int month, int year)
        {
            return individualScorecardClass.GetIndividualWorkTypes(teamId, repId, month, year);
        }

        public List<IndividualNPT> GetIndividualNPT(long teamId, long repId, int month, int year)
        {
            return individualScorecardClass.GetIndividualNPT(teamId, repId, month, year);
        }
        #endregion "IndividualScorecardClass"

        #region "TeamScorecardClass"
        
        #endregion "TeamScorecardClass"

        #region "BTSSExtensions"
        public bool IsManaged(long? teamId, string user_name, string role)
        {
            return btssExtensions.IsManaged(teamId, user_name, role);
        }
        #endregion "BTSSExtensions"

        #region "GetReference"
        public OSC_Representative GetRepresentativeByPRD(string user_name)
        {
            return getReference.GetRepresentativeByPRD(user_name);
        }

        public OSC_Manager GetManagerByUserName(string user_name)
        {
            return getReference.GetManagerByUserName(user_name);
        }

        public OSC_TeamGroupIds GetTeamIdByGroupId(string groupId, string groupType)
        {
            return getReference.GetTeamIdByGroupId(groupId, groupType);
        }

        public List<OSC_TeamGroupIds> GetGroupIds(long? teamId)
        {
            return getReference.GetGroupIds(teamId);
        }
        #endregion "GetReference"
    }

    class DateTimeFormatting
    {
        public string GetTimeFormat(long sec)
        {
            string result;
            long hours, mins, secs;
            hours = sec / 3600;
            mins = (sec % 3600) / 60;
            secs = (sec % 3600) % 60;
            result = hours.ToString("00") + ":" + mins.ToString("00") + ":" + secs.ToString("00");
            return result;
        }

        public long GetSecondsFormat(string _hour)
        {
            long returnValue, hours = 0, mins = 0, secs = 0;
            char delimiter = ':';
            int ctr = 1;
            string[] time = _hour.Split(delimiter);

            foreach (string s in time)
            {
                if (ctr == 1) { hours = Convert.ToInt64(s); }
                else if (ctr == 2) { mins = Convert.ToInt64(s); }
                else { secs = Convert.ToInt64(s); }
                ctr++;
            }

            returnValue = (hours * 3600) + (mins * 60) + secs;
            return returnValue;
        }
    }

    class InitializeLists
    {
        public bool InitializeTeams(OSCContext db)
        {
            foreach (OSC_Team team in db.Teams)
            {
                team.Department = db.Departments.Find(team.DepartmentId);
            }
            return true;
        }

        public bool InitializeRepresentatives(OSCContext db)
        {
            foreach (OSC_Representative rep in db.Representatives)
            {
                rep.FullName = rep.FirstName + " " + rep.LastName;
                rep.Team = db.Teams.Find(rep.TeamId);
                rep.Location = db.Locations.Find(rep.LocationId);
                rep.CoreRole = db.CoreRoles.Find(rep.CoreRoleId);
            }
            return true;
        }

        public bool InitializeNpts(OSCContext db)
        {
            foreach (OSC_ImportNPT npt in db.NPT)
            {
                npt.Team = db.Teams.Find(npt.TeamId);
                npt.Representative = db.Representatives.Find(npt.RepId);
                npt.Representative.Location = db.Locations.Find(npt.Representative.LocationId);
                npt.Representative.CoreRole = db.CoreRoles.Find(npt.Representative.CoreRoleId);
                npt.Representative.FullName = npt.Representative.FirstName + " " + npt.Representative.LastName;
            }
            return true;
        }

        public bool InitializeActivities(OSCContext db)
        {
            foreach (OSC_ActivityTracker act in db.ActivityTrackers)
            {
                act.Team = db.Teams.Find(act.TeamId);
                act.Representative = db.Representatives.Find(act.RepId);
                act.Representative.Location = db.Locations.Find(act.Representative.LocationId);
                act.Representative.CoreRole = db.CoreRoles.Find(act.Representative.CoreRoleId);
                act.Representative.FullName = act.Representative.FirstName + " " + act.Representative.LastName;
            }
            return true;
        }

        public bool InitializeManualEntries(OSCContext db)
        {
            foreach (OSC_ManualEntry me in db.ManualEntries)
            {
                me.MonthName = db.months.Where(m => m.Value == Convert.ToString(me.Month)).First().Text;
                me.Team = db.Teams.Find(me.TeamId);
                me.Representative = db.Representatives.Find(me.RepId);
                me.Representative.Location = db.Locations.Find(me.Representative.LocationId);
                me.Representative.CoreRole = db.CoreRoles.Find(me.Representative.CoreRoleId);
                me.Representative.FullName = me.Representative.FirstName + " " + me.Representative.LastName;
            }
            return true;
        }

        public bool InitializeTeamNptCategories(OSCContext db)
        {
            foreach (OSC_TeamNptCategory n in db.TeamNptCategories)
            {
                n.CategoryDesc = db.NptCategories.Find(n.CategoryId).CategoryDesc;
            }
            return true;
        }
    }

    class GetReference
    {
        private OSCContext db = new OSCContext();
        public OSC_Representative GetRepresentativeByPRD(string user_name)
        {
            OSC_Representative rep = db.Representatives.Where(t => t.PRDUserId == user_name && t.IsActive).FirstOrDefault();
            return rep;
        }

        public OSC_Manager GetManagerByUserName(string user_name)
        {
            OSC_Manager mgr = db.Managers.Where(t => t.PRDUserId == user_name).FirstOrDefault();
            return mgr;
        }

        public OSC_TeamGroupIds GetTeamIdByGroupId(string groupId, string groupType)
        {
            OSC_TeamGroupIds result = db.TeamGroupIds.Where(t => t.GroupId == groupId && t.GroupType == groupType).FirstOrDefault();
            return result;
        }

        public List<OSC_TeamGroupIds> GetGroupIds(long? teamId)
        {
            var result = db.TeamGroupIds.Where(t => t.TeamId == teamId);
            return result.ToList();
        }
    }

    class IndividualScorecardClass
    {
        private OSCContext db = new OSCContext();
        public List<IndividualScorecard> GetIndividualScorecardFull(long teamId, long repId, int month, int year)
        {
            List<IndividualScorecard> result = new List<IndividualScorecard>();
            for (int i = 1; i <= month; i++)
            {
                result.Add(GetIndividualScorecard(teamId, repId, i, year));
            }
            IndividualScorecard total = new IndividualScorecard()
            {
                Month = 13,
                MonthName = "Total",
                TeamId = teamId,
                RepId = repId,
                Year = year,
                AverageAuxTime = 0,
                AverageHandleTime = 0,
                AverageTalkTime = 0,
                AverageWrapUpDuration = 0,
                Efficiency = 0,
                Highlights = "",
                HoursWorked = 0,
                SupportLineUtilization = 0,
                TotalUtilization = 0,
                ProductivityRating = 0,
                individualActivities = GetIndividualActivities(teamId, repId, 13, year),
                individualAIQ = GetIndividualAIQ(teamId, repId, 13, year),
                individualBIProd = GetIndividualProd(teamId, repId, 13, year),
                rep = db.Representatives.Find(repId),
                individualNonProcessing = GetIndividualNonProcessing(teamId, repId, 13, year),
                individualBIQual = GetIndividualQual(teamId, repId, 13, year),
                individualManualEntries = GetIndividualManualEntries(teamId, repId, 13, year)
            };
            foreach (IndividualScorecard item in result)
            {
                total.AverageAuxTime = total.AverageAuxTime + item.AverageAuxTime; //GetAverage
                total.AverageHandleTime = total.AverageHandleTime + item.AverageHandleTime; //GetAverage
                total.AverageTalkTime = total.AverageTalkTime + item.AverageTalkTime; //GetAverage
                total.AverageWrapUpDuration = total.AverageWrapUpDuration + item.AverageWrapUpDuration; //GetAverage
                total.Efficiency = total.Efficiency + item.Efficiency; //GetAverage
                total.ProductivityRating = total.ProductivityRating + item.ProductivityRating; //GetAverage
                total.SupportLineUtilization = total.SupportLineUtilization + item.SupportLineUtilization; //GetAverage
                total.TotalUtilization = total.TotalUtilization + item.TotalUtilization; //GetAverage
                total.HoursWorked = total.HoursWorked + item.HoursWorked;
                total.individualBIProd.ProcessingHours = total.individualBIProd.ProcessingHours + item.individualBIProd.ProcessingHours;
                total.individualBIProd.Count = total.individualBIProd.Count + item.individualBIProd.Count;
                total.individualBIQual.FailCount = total.individualBIQual.FailCount + item.individualBIQual.FailCount;
                total.individualBIQual.ReviewCount = total.individualBIQual.ReviewCount + item.individualBIQual.ReviewCount;
                total.individualBIQual.QualityRating = total.individualBIQual.QualityRating + item.individualBIQual.QualityRating; //GetAverage
                total.individualActivities.Attendance_Days = total.individualActivities.Attendance_Days + item.individualActivities.Attendance_Days;
                total.individualActivities.Attendance_Hours = total.individualActivities.Attendance_Hours + item.individualActivities.Attendance_Hours;
                total.individualActivities.Overtime_Days = total.individualActivities.Overtime_Days + item.individualActivities.Overtime_Days;
                total.individualActivities.Overtime_Hours = total.individualActivities.Overtime_Hours + item.individualActivities.Overtime_Hours;
                total.individualActivities.TimeOff_Days = total.individualActivities.TimeOff_Days + item.individualActivities.TimeOff_Days;
                total.individualActivities.TimeOff_Hours = total.individualActivities.TimeOff_Hours + item.individualActivities.TimeOff_Hours;
                total.individualActivities.Holiday_Days = total.individualActivities.Holiday_Days + item.individualActivities.Holiday_Days;
                total.individualActivities.Holiday_Hours = total.individualActivities.Holiday_Hours + item.individualActivities.Holiday_Hours;
                total.individualNonProcessing.NPTHours = total.individualNonProcessing.NPTHours + item.individualNonProcessing.NPTHours;
                total.individualAIQ.ACDRingTime = total.individualAIQ.ACDRingTime + item.individualAIQ.ACDRingTime;
                total.individualAIQ.ACDTalkTime = total.individualAIQ.ACDTalkTime + item.individualAIQ.ACDTalkTime;
                total.individualAIQ.ACDWrapUpTime = total.individualAIQ.ACDWrapUpTime + item.individualAIQ.ACDWrapUpTime;
                total.individualAIQ.Aux = total.individualAIQ.Aux + item.individualAIQ.Aux;
                total.individualAIQ.AvgExtOutActiveDur = total.individualAIQ.AvgExtOutActiveDur + item.individualAIQ.AvgExtOutActiveDur;
                total.individualAIQ.AvgHoldDur = total.individualAIQ.AvgHoldDur + item.individualAIQ.AvgHoldDur;
                total.individualAIQ.ExtInAvgActiveDur = total.individualAIQ.ExtInAvgActiveDur + item.individualAIQ.ExtInAvgActiveDur;
                total.individualAIQ.ExtInCalls = total.individualAIQ.ExtInCalls + item.individualAIQ.ExtInCalls;
                total.individualAIQ.ExtOutCalls = total.individualAIQ.ExtOutCalls + item.individualAIQ.ExtOutCalls;
                total.individualAIQ.HeldContacts = total.individualAIQ.HeldContacts + item.individualAIQ.HeldContacts;
                total.individualAIQ.IntervalIdleDur = total.individualAIQ.IntervalIdleDur + item.individualAIQ.IntervalIdleDur;
                total.individualAIQ.IntervalStaffedDuration = total.individualAIQ.IntervalStaffedDuration + item.individualAIQ.IntervalStaffedDuration;
                total.individualAIQ.Redirects = total.individualAIQ.Redirects + item.individualAIQ.Redirects;
                total.individualAIQ.TotalACDCalls = total.individualAIQ.TotalACDCalls + item.individualAIQ.TotalACDCalls;
                total.individualAIQ.TotalPercServiceTime = total.individualAIQ.TotalPercServiceTime + item.individualAIQ.TotalPercServiceTime;
                total.individualAIQ.Transfers = total.individualAIQ.Transfers + item.individualAIQ.Transfers;
                total.individualManualEntries.CallManagementScore = total.individualManualEntries.CallManagementScore + item.individualManualEntries.CallManagementScore; //GetAverage
                total.individualManualEntries.AdministrativeProcedures = total.individualManualEntries.AdministrativeProcedures + item.individualManualEntries.AdministrativeProcedures; //GetAverage
                total.individualManualEntries.ActiveProjects = total.individualManualEntries.ActiveProjects + item.individualManualEntries.ActiveProjects;
                total.individualManualEntries.CallEfficiency = total.individualManualEntries.CallEfficiency + item.individualManualEntries.CallEfficiency; //GetAverage
                total.individualManualEntries.Commitment = total.individualManualEntries.Commitment + item.individualManualEntries.Commitment; //GetAverage
                total.individualManualEntries.CompletedProjects = total.individualManualEntries.CompletedProjects + item.individualManualEntries.CompletedProjects;
                total.individualManualEntries.Compliance = total.individualManualEntries.Compliance + item.individualManualEntries.Compliance; //GetAverage
                total.individualManualEntries.Engagement = total.individualManualEntries.Engagement + item.individualManualEntries.Engagement; //GetAverage
                total.individualManualEntries.GainLossAmount = total.individualManualEntries.GainLossAmount + item.individualManualEntries.GainLossAmount;
                total.individualManualEntries.GainLossOccurances = total.individualManualEntries.GainLossOccurances + item.individualManualEntries.GainLossOccurances;
                total.individualManualEntries.JHValues = total.individualManualEntries.JHValues + item.individualManualEntries.JHValues; //GetAverage
                total.individualManualEntries.ProductAccuracy = total.individualManualEntries.ProductAccuracy + item.individualManualEntries.ProductAccuracy; //GetAverage
                total.individualManualEntries.ScheduleAdherence = total.individualManualEntries.ScheduleAdherence + item.individualManualEntries.ScheduleAdherence; //GetAverage
            }
            double multiplier = Math.Pow(10, 2);
            total.AverageAuxTime = total.AverageAuxTime / month;
            total.AverageAuxTime = Math.Ceiling(total.AverageAuxTime * multiplier) / multiplier;
            total.AverageHandleTime = total.AverageHandleTime / month;
            total.AverageHandleTime = Math.Ceiling(total.AverageHandleTime * multiplier) / multiplier;
            total.AverageTalkTime = total.AverageTalkTime / month;
            total.AverageTalkTime = Math.Ceiling(total.AverageTalkTime * multiplier) / multiplier;
            total.AverageWrapUpDuration = total.AverageWrapUpDuration / month;
            total.AverageWrapUpDuration = Math.Ceiling(total.AverageWrapUpDuration * multiplier) / multiplier;
            total.Efficiency = total.Efficiency / month;
            total.Efficiency = Math.Ceiling(total.Efficiency * multiplier) / multiplier;
            total.ProductivityRating = total.ProductivityRating / month;
            total.ProductivityRating = Math.Ceiling(total.ProductivityRating * multiplier) / multiplier;
            total.SupportLineUtilization = total.SupportLineUtilization / month;
            total.SupportLineUtilization = Math.Ceiling(total.SupportLineUtilization * multiplier) / multiplier;
            total.TotalUtilization = total.TotalUtilization / month;
            total.TotalUtilization = Math.Ceiling(total.TotalUtilization * multiplier) / multiplier;
            total.individualBIQual.QualityRating = total.individualBIQual.QualityRating / month;
            total.individualBIQual.QualityRating = Math.Ceiling(total.individualBIQual.QualityRating * multiplier) / multiplier;
            total.individualManualEntries.CallManagementScore = total.individualManualEntries.CallManagementScore / month;
            total.individualManualEntries.CallManagementScore = Math.Ceiling((double)total.individualManualEntries.CallManagementScore * multiplier) / multiplier;
            total.individualManualEntries.AdministrativeProcedures = total.individualManualEntries.AdministrativeProcedures / month;
            total.individualManualEntries.AdministrativeProcedures = Math.Ceiling((double)total.individualManualEntries.AdministrativeProcedures * multiplier) / multiplier;
            total.individualManualEntries.CallEfficiency = total.individualManualEntries.CallEfficiency / month;
            total.individualManualEntries.CallEfficiency = Math.Ceiling((double)total.individualManualEntries.CallEfficiency * multiplier) / multiplier;
            total.individualManualEntries.Commitment = total.individualManualEntries.Commitment / month;
            total.individualManualEntries.Commitment = Math.Ceiling((double)total.individualManualEntries.Commitment * multiplier) / multiplier;
            total.individualManualEntries.Compliance = total.individualManualEntries.Compliance / month;
            total.individualManualEntries.Compliance = Math.Ceiling((double)total.individualManualEntries.Compliance * multiplier) / multiplier;
            total.individualManualEntries.Engagement = total.individualManualEntries.Engagement / month;
            total.individualManualEntries.Engagement = Math.Ceiling((double)total.individualManualEntries.Engagement * multiplier) / multiplier;
            total.individualManualEntries.JHValues = total.individualManualEntries.JHValues / month;
            total.individualManualEntries.JHValues = Math.Ceiling((double)total.individualManualEntries.JHValues * multiplier) / multiplier;
            total.individualManualEntries.ProductAccuracy = total.individualManualEntries.ProductAccuracy / month;
            total.individualManualEntries.ProductAccuracy = Math.Ceiling((double)total.individualManualEntries.ProductAccuracy * multiplier) / multiplier;
            total.individualManualEntries.ScheduleAdherence = total.individualManualEntries.ScheduleAdherence / month;
            total.individualManualEntries.ScheduleAdherence = Math.Ceiling((double)total.individualManualEntries.ScheduleAdherence * multiplier) / multiplier;
            result.Add(total);
            return result.OrderBy(t => t.Month).ToList();
        }

        public IndividualScorecard GetIndividualScorecard(long teamId, long repId, int month, int year)
        {
            IndividualScorecard result = new IndividualScorecard();
            result = (from list in db.IndividualScorecards
                      where list.TeamId == teamId &&
                             list.RepId == repId &&
                             list.Month == month &&
                             list.Year == year
                      select new IndividualScorecard
                      {
                          TeamId = (long)list.TeamId,
                          RepId = (long)list.RepId,
                          Month = (int)list.Month,
                          Year = (int)list.Year,
                          Highlights = (string)list.Comments
                      }).FirstOrDefault();
            if (result != null)
            {
                result.rep = db.Representatives.Find(repId);
                result.rep.Location = db.Locations.Find(result.rep.LocationId);
                result.rep.CoreRole = db.CoreRoles.Find(result.rep.CoreRoleId);
                result.MonthName = db.months.Where(m => m.Value == Convert.ToString(result.Month)).First().Text;
                result.individualBIProd = GetIndividualProd(teamId, repId, month, year);
                result.individualBIQual = GetIndividualQual(teamId, repId, month, year);
                result.individualAIQ = GetIndividualAIQ(teamId, repId, month, year);
                result.individualNonProcessing = GetIndividualNonProcessing(teamId, repId, month, year);
                result.individualActivities = GetIndividualActivities(teamId, repId, month, year);
                result.individualManualEntries = GetIndividualManualEntries(teamId, repId, month, year);
                result.HoursWorked = GetHoursWorked(result.individualActivities.Attendance_Hours, result.individualActivities.Overtime_Hours, result.individualActivities.TimeOff_Hours, result.individualActivities.Holiday_Hours);
                result.ProductivityRating = GetProductivityRating(result.individualBIProd.ProcessingHours, result.HoursWorked);
                result.AverageTalkTime = GetAverageTalkTime((long)result.individualAIQ.ACDTalkTime, (int)result.individualAIQ.TotalACDCalls);
                result.AverageWrapUpDuration = GetAverageWrapUpDuration((long)result.individualAIQ.ACDWrapUpTime, (int)result.individualAIQ.TotalACDCalls);
                result.AverageAuxTime = GetAverageAuxTime((long)result.individualAIQ.Aux, (int)result.individualAIQ.TotalACDCalls);
                result.TotalUtilization = GetTotalUtilization(result.individualBIProd.ProcessingHours, (long)result.individualAIQ.ACDTalkTime, result.individualNonProcessing.NPTHours, result.HoursWorked);
                result.Efficiency = GetEfficiency(result.individualBIProd.ProcessingHours, result.HoursWorked, result.individualNonProcessing.NPTHours, (long)result.individualAIQ.ACDTalkTime);
                result.AverageHandleTime = GetAverageHandleTime((long)result.individualAIQ.ACDTalkTime, (long)result.individualAIQ.ACDWrapUpTime, (long)result.individualAIQ.AvgHoldDur, (int)result.individualAIQ.HeldContacts, (int)result.individualAIQ.TotalACDCalls);
                result.SupportLineUtilization = GetSupportLineUtilization((long)result.individualAIQ.ACDTalkTime, (long)result.individualAIQ.IntervalIdleDur, (long)result.individualAIQ.ACDWrapUpTime, (double)result.HoursWorked, (double)result.individualBIProd.ProcessingHours, (double)result.individualNonProcessing.NPTHours);
            }
            else
            {

                result = new IndividualScorecard()
                {
                    Month = month,
                    MonthName = db.months.Where(m => m.Value == Convert.ToString(month)).First().Text,
                    TeamId = teamId,
                    RepId = repId,
                    Year = year,
                    AverageAuxTime = 0,
                    AverageHandleTime = 0,
                    AverageTalkTime = 0,
                    AverageWrapUpDuration = 0,
                    Efficiency = 0,
                    Highlights = "",
                    HoursWorked = 0,
                    SupportLineUtilization = 0,
                    TotalUtilization = 0,
                    ProductivityRating = 0,
                    individualActivities = GetIndividualActivities(teamId, repId, month, year),
                    individualAIQ = GetIndividualAIQ(teamId, repId, month, year),
                    individualBIProd = GetIndividualProd(teamId, repId, month, year),
                    rep = db.Representatives.Find(repId),
                    individualNonProcessing = GetIndividualNonProcessing(teamId, repId, month, year),
                    individualBIQual = GetIndividualQual(teamId, repId, month, year)
                };
            }
            return result;
        }

        #region "Formulas"
        public double GetProductivityRating(double ProcessingHours, double HoursWorked)
        {
            double ProductivityRating = 0;
            ProductivityRating = ProcessingHours / HoursWorked;
            if (ProductivityRating > 0)
            {
                ProductivityRating = ProductivityRating * 100;
                double multiplier = Math.Pow(10, 2);
                ProductivityRating = Math.Ceiling(ProductivityRating * multiplier) / multiplier;
            }
            else { ProductivityRating = 0; }
            return ProductivityRating;
        }

        public double GetTotalUtilization(double ProcessingHours, long ACDTalkTime, double NPTHours, double HoursWorked)
        {
            double TotalUtilization = 0;
            TotalUtilization = ACDTalkTime / 3600;
            TotalUtilization = ProcessingHours + TotalUtilization + NPTHours;
            if (HoursWorked > 0)
            {
                TotalUtilization = TotalUtilization / HoursWorked;
                TotalUtilization = TotalUtilization * 100;
                double multiplier = Math.Pow(10, 2);
                TotalUtilization = Math.Ceiling(TotalUtilization * multiplier) / multiplier;
            }
            else { TotalUtilization = 0; }
            return TotalUtilization;
        }

        public double GetEfficiency(double ProcessingHours, double HoursWorked, double NPTHours, long ACDTalkTime)
        {
            double Efficiency = 0;
            Efficiency = ACDTalkTime / 3600;
            Efficiency = (HoursWorked - NPTHours) - Efficiency;
            if (Efficiency > 0)
            {
                Efficiency = ProcessingHours / Efficiency;
                Efficiency = Efficiency * 100;
                double multiplier = Math.Pow(10, 2);
                Efficiency = Math.Ceiling(Efficiency * multiplier) / multiplier;
            }
            else { Efficiency = 0; }
            return Efficiency;
        }

        public double GetAverageHandleTime(long ACDTalkTime, long ACDWrapUpTime, long AvgHoldDur, int HeldContacts, int TotalACDCalls)
        {
            double AverageHandleTime = 0;
            AverageHandleTime = AvgHoldDur * HeldContacts;
            AverageHandleTime = ACDTalkTime + ACDWrapUpTime + AverageHandleTime;
            AverageHandleTime = AverageHandleTime / TotalACDCalls;
            return AverageHandleTime;
        }

        public double GetSupportLineUtilization(long ACDTalkTime, long IntervalIdleDur, long ACDWrapUpTime, double HoursWorked, double ProcessingHours, double NPTHours)
        {
            double SupportLineUtilization = 0;
            SupportLineUtilization = (ACDTalkTime / 3600) + (IntervalIdleDur / 3600) + (ACDWrapUpTime / 3600);
            SupportLineUtilization = SupportLineUtilization / (HoursWorked - ProcessingHours) - NPTHours;
            SupportLineUtilization = SupportLineUtilization * 100;
            return SupportLineUtilization;
        }

        public double GetHoursWorked(double Attendance, double Overtime, double TimeOff, double Holiday)
        { return ((Attendance + Overtime) - TimeOff) - Holiday; }

        public double GetAverageTalkTime(long ACDTalkTime, int TotalACDCalls)
        {
            double result = 0;
            if (TotalACDCalls != 0)
            {
                result = ACDTalkTime / TotalACDCalls;
            }
            return result;
        }

        public double GetAverageWrapUpDuration(long ACDWrapUpTime, int TotalACDCalls)
        {
            double result = 0;
            if (TotalACDCalls != 0)
            {
                result = ACDWrapUpTime / TotalACDCalls;
            }
            return result;
        }

        public double GetAverageAuxTime(long Aux, int TotalACDCalls)
        {
            double result = 0;
            if (TotalACDCalls != 0)
            {
                result = Aux / TotalACDCalls;
            }
            return result;
        }
        #endregion "Formulas"

        #region "IndividualScorecardReference"
        public IndividualActivities GetIndividualActivities(long teamId, long repId, int month, int year)
        {
            IndividualActivities result = new IndividualActivities() { Attendance_Days = 0, Attendance_Hours = 0, Overtime_Days = 0, Overtime_Hours = 0, Holiday_Days = 0, Holiday_Hours = 0, TimeOff_Days = 0, TimeOff_Hours = 0 };
            var x = (from list in db.ActivityTrackers
                     where list.Activity == "Attendance" &&
                            list.TeamId == teamId &&
                            list.RepId == repId &&
                            list.Month == month &&
                            list.Year == year
                     select new IndividualActivities
                     {
                         TeamId = list.TeamId,
                         RepId = list.RepId,
                         Month = list.Month,
                         Year = list.Year,
                         Attendance_Days = (double)list.NoOfDays,
                         Attendance_Hours = (double)list.NoOfHours
                     }).FirstOrDefault();
            if (x != null)
            {
                result = x;
            }
            else
            {
                result.TeamId = teamId;
                result.RepId = repId;
                result.Month = month;
                result.Year = year;
                result.Attendance_Days = 0;
                result.Attendance_Hours = 0;
            }
            var y = (from list in db.ActivityTrackers
                     where list.Activity == "Holiday" &&
                            list.TeamId == teamId &&
                            list.RepId == repId &&
                            list.Month == month &&
                            list.Year == year
                     select new IndividualActivities
                     {
                         TeamId = list.TeamId,
                         RepId = list.RepId,
                         Month = list.Month,
                         Year = list.Year,
                         Holiday_Days = (double)list.NoOfDays,
                         Holiday_Hours = (double)list.NoOfHours
                     });
            result.Holiday_Days = 0;
            result.Holiday_Hours = 0;
            if (y != null)
            {
                foreach (IndividualActivities item in y)
                {
                    result.Holiday_Days += item.Holiday_Days;
                    result.Holiday_Hours += item.Holiday_Hours;
                }
            }
            var z = (from list in db.ActivityTrackers
                     where list.Activity == "Overtime" &&
                            list.TeamId == teamId &&
                            list.RepId == repId &&
                            list.Month == month &&
                            list.Year == year
                     select new IndividualActivities
                     {
                         TeamId = list.TeamId,
                         RepId = list.RepId,
                         Month = list.Month,
                         Year = list.Year,
                         Overtime_Days = (double)list.NoOfDays,
                         Overtime_Hours = (double)list.NoOfHours
                     });
            result.Overtime_Days = 0;
            result.Overtime_Hours = 0;
            if (z != null)
            {
                foreach (IndividualActivities item in z)
                {
                    result.Overtime_Days += item.Overtime_Days;
                    result.Overtime_Hours += item.Overtime_Hours;
                }
            }
            var a = (from list in db.ActivityTrackers
                     where list.Activity == "Time-off" &&
                            list.TeamId == teamId &&
                            list.RepId == repId &&
                            list.Month == month &&
                            list.Year == year
                     select new IndividualActivities
                     {
                         TeamId = list.TeamId,
                         RepId = list.RepId,
                         Month = list.Month,
                         Year = list.Year,
                         TimeOff_Days = (double)list.NoOfDays,
                         TimeOff_Hours = (double)list.NoOfHours
                     });
            result.TimeOff_Days = 0;
            result.TimeOff_Hours = 0;
            if (a != null)
            {
                foreach (IndividualActivities item in z)
                {
                    result.TimeOff_Days += item.TimeOff_Days;
                    result.TimeOff_Hours += item.TimeOff_Hours;
                }
            }
            return result;
        }

        public OSC_ManualEntry GetIndividualManualEntry(long teamId, long repId, int month, int year)
        {
            OSC_ManualEntry result = (from list in db.ManualEntries
                                      where list.TeamId == teamId && list.RepId == repId && list.Month == month && list.Year == year
                                      select list).FirstOrDefault();
            return result;
        }

        public IndividualBIProd GetIndividualProd(long teamId, long repId, int month, int year)
        {
            IndividualBIProd result = new IndividualBIProd();
            var x = (from list in db.BIP
                     where list.TeamId == teamId && list.RepId == repId && list.Month == month && list.Year == year
                     select new IndividualBIProd { TeamId = (long)list.TeamId, RepId = (long)list.RepId, Count = (int)list.Count, Month = (int)list.Month, Year = (int)list.Year });
            foreach (IndividualBIProd item in x) { if (result != null) result.Count += item.Count; else result = item; }
            List<IndividualBIProdTimings> individualBiProdTimings = GetIndividualBIProdTimings(teamId, repId, month, year);
            foreach (IndividualBIProdTimings item in individualBiProdTimings)
            {
                result.ProcessingHours += item.ProcessingMinutes;
            }
            result.ProcessingHours = result.ProcessingHours / 60;
            double multiplier = Math.Pow(10, 2);
            result.ProcessingHours = Math.Ceiling(result.ProcessingHours * multiplier) / multiplier;
            return result;
        }

        public IndividualBIQual GetIndividualQual(long teamId, long repId, int month, int year)
        {
            IndividualBIQual result = new IndividualBIQual();
            var x = (from list in db.BIQ
                     where list.TeamId == teamId && list.Repid == repId && list.Month == month && list.Year == year
                     select new IndividualBIQual { TeamId = (long)list.TeamId, RepId = (long)list.Repid, FailCount = (int)list.Count4, ReviewCount = (int)list.Count3, Month = (int)list.Month, Year = (int)list.Year });
            if (x != null)
            {
                foreach (IndividualBIQual item in x)
                {
                    if (result != null)
                    {
                        result.FailCount += item.FailCount;
                        result.ReviewCount += item.ReviewCount;
                    }
                    else result = item;
                }
                if (result.ReviewCount > 0)
                {
                    if (result.FailCount > 0) result.QualityRating = (((double)result.ReviewCount - (double)result.FailCount) / (double)result.ReviewCount) * 100;
                    else result.QualityRating = ((result.ReviewCount - result.FailCount) / (result.ReviewCount - result.FailCount)) * 100;
                    double multiplier = Math.Pow(10, 2);
                    result.QualityRating = Math.Ceiling(result.QualityRating * multiplier) / multiplier;
                }
                else
                {
                    result.QualityRating = 0;
                }
            }
            else
            {
                result = new IndividualBIQual()
                {
                    TeamId = teamId,
                    RepId = repId,
                    Month = month,
                    Year = year,
                    FailCount = 0,
                    ReviewCount = 0,
                    QualityRating = 0
                };
            }
            return result;
        }

        public OSC_ImportAIQ GetIndividualAIQ(long teamId, long repId, int month, int year)
        {
            OSC_ImportAIQ result = new OSC_ImportAIQ();
            var x = (from list in db.AIQ
                     where list.TeamId == teamId && list.RepId == repId && list.Month == month && list.Year == year
                     select list).FirstOrDefault();
            if (x != null)
            {
                result = x;
            }
            else
            {
                result = new OSC_ImportAIQ()
                {
                    TeamId = teamId,
                    RepId = repId,
                    Month = month,
                    Year = year,
                    IntervalStaffedDuration = 0,
                    TotalPercServiceTime = 0,
                    TotalACDCalls = 0,
                    ExtInCalls = 0,
                    ExtInAvgActiveDur = 0,
                    ExtOutCalls = 0,
                    AvgExtOutActiveDur = 0,
                    ACDWrapUpTime = 0,
                    ACDTalkTime = 0,
                    ACDRingTime = 0,
                    Aux = 0,
                    AvgHoldDur = 0,
                    IntervalIdleDur = 0,
                    Transfers = 0,
                    HeldContacts = 0,
                    Redirects = 0
                };
            }

            return result;
        }

        public OSC_ManualEntry GetIndividualManualEntries(long teamId, long repId, int month, int year)
        {
            OSC_ManualEntry result = new OSC_ManualEntry();
            var x = (from list in db.ManualEntries
                     where list.TeamId == teamId && list.RepId == repId && list.Month == month && list.Year == year && list.IsActive
                     select list).FirstOrDefault();
            if (x != null)
            {
                result = x;
            }
            else
            {
                result = new OSC_ManualEntry()
                {
                    TeamId = teamId,
                    RepId = repId,
                    Month = month,
                    Year = year,
                    ActiveProjects = 0,
                    AdministrativeProcedures = 0,
                    CallEfficiency = 0,
                    CallManagementScore = 0,
                    Commitment = 0,
                    CompletedProjects = 0,
                    Compliance = 0,
                    Engagement = 0,
                    GainLossAmount = 0,
                    GainLossOccurances = 0,
                    JHValues = 0,
                    ProductAccuracy = 0,
                    ProjectResponsibility = "",
                    ScheduleAdherence = 0
                };
            }
            return result;
        }

        public IndividualNonProcessing GetIndividualNonProcessing(long teamId, long repId, int month, int year)
        {
            IndividualNonProcessing result = new IndividualNonProcessing();
            var x = (from list in db.NPT
                     where list.TeamId == teamId && list.RepId == repId && list.Month == month && list.Year == year
                     select new IndividualNonProcessing { TeamId = (long)list.TeamId, RepId = (long)list.RepId, NPTHours = (double)list.TimeSpent, Month = (int)list.Month, Year = (int)list.Year });
            if (x != null)
            {
                foreach (IndividualNonProcessing item in x)
                {
                    if (result != null)
                    {
                        result.NPTHours += (item.NPTHours / 60);
                    }
                    else result = item;
                }
                double multiplier = Math.Pow(10, 2);
                result.NPTHours = Math.Ceiling(result.NPTHours * multiplier) / multiplier;
            }
            else
            {
                result = new IndividualNonProcessing()
                {
                    TeamId = teamId,
                    RepId = repId,
                    Month = month,
                    Year = year,
                    NPTHours = 0
                };
            }
            return result;
        }
        #endregion "IndividualScorecardReference"

        public List<IndividualWorkTypes> GetIndividualWorkTypes(long teamId, long repId, int month, int year)
        {
            List<IndividualWorkTypes> result = new List<IndividualWorkTypes>();
            var s = (from list in db.BIP
                     where list.TeamId == teamId && list.RepId == repId && list.Month == month && list.Year == year
                     select new IndividualWorkTypes { WorkType = list.WorkType, Count = (int)list.Count });
            foreach (IndividualWorkTypes item in s)
            {
                if (result.Any(t => t.WorkType == item.WorkType))
                {
                    IndividualWorkTypes obj = result.Where(t => t.WorkType == item.WorkType).FirstOrDefault();
                    result.Remove(obj);
                    obj.Count += item.Count;
                    result.Add(obj);
                }
                else result.Add(item);
            }
            return result.OrderBy(t => t.WorkType).ToList();
        }

        public List<IndividualNPT> GetIndividualNPT(long teamId, long repId, int month, int year)
        {
            List<IndividualNPT> result = new List<IndividualNPT>();
            var s = (from list in db.NPT
                     where list.TeamId == teamId && list.RepId == repId && list.Month == month && list.Year == year
                     select new IndividualNPT { Category = list.TypeOfActivity, TimeSpent = (double)list.TimeSpent });

            foreach (IndividualNPT item in s)
            {
                if (result.Any(t => t.Category == item.Category))
                {
                    IndividualNPT obj = result.Where(t => t.Category == item.Category).FirstOrDefault();
                    result.Remove(obj);
                    obj.TimeSpent += item.TimeSpent;
                    result.Add(obj);
                }
                else result.Add(item);
            }
            return result.OrderBy(t => t.Category).ToList();
        }

        public List<IndividualBIProdTimings> GetIndividualBIProdTimings(long teamId, long repId, int month, int year)
        {
            List<IndividualBIProdTimings> result = new List<IndividualBIProdTimings>();
            var x = (from list in db.BIP
                     where list.TeamId == teamId && list.RepId == repId && list.Month == month && list.Year == year
                     select new IndividualBIProdTimings
                     {
                         TeamId = (long)list.TeamId,
                         RepId = (long)list.RepId,
                         Month = (int)list.Month,
                         Year = (int)list.Year,
                         BusinessArea = (string)list.BusinessArea,
                         WorkType = (string)list.WorkType,
                         Status = (string)list.Status,
                         Count = (int)list.Count
                     });
            foreach (IndividualBIProdTimings item in x)
            {
                if (result.Any(t => t.BusinessArea == item.BusinessArea && t.WorkType == item.WorkType && t.Status == item.Status))
                {
                    IndividualBIProdTimings obj = result.Where(t => t.BusinessArea == item.BusinessArea && t.WorkType == item.WorkType && t.Status == item.Status).FirstOrDefault();
                    result.Remove(obj);
                    obj.Count += item.Count;
                    result.Add(obj);
                }
                else result.Add(item);
            }
            List<OSC_TeamWorkItem> twiList = GetTeamWorkItems(teamId, year);
            foreach (IndividualBIProdTimings item in result)
            {
                OSC_TeamWorkItem teamWorkItem = twiList.Where(t => t.BusinessAreaCode == item.BusinessArea && t.WorkTypeCode == item.WorkType && t.StatusCode == item.Status).FirstOrDefault();
                double minutes = 0;
                if (teamWorkItem != null)
                {
                    switch (item.Month)
                    {
                        case 1: minutes = (double)teamWorkItem.January; break;
                        case 2: minutes = (double)teamWorkItem.February; break;
                        case 3: minutes = (double)teamWorkItem.March; break;
                        case 4: minutes = (double)teamWorkItem.April; break;
                        case 5: minutes = (double)teamWorkItem.May; break;
                        case 6: minutes = (double)teamWorkItem.June; break;
                        case 7: minutes = (double)teamWorkItem.July; break;
                        case 8: minutes = (double)teamWorkItem.August; break;
                        case 9: minutes = (double)teamWorkItem.September; break;
                        case 10: minutes = (double)teamWorkItem.October; break;
                        case 11: minutes = (double)teamWorkItem.November; break;
                        case 12: minutes = (double)teamWorkItem.December; break;
                    }
                }
                item.ProcessingMinutes = item.Count * minutes;
                double multiplier = Math.Pow(10, 2);
                item.ProcessingMinutes = Math.Ceiling(item.ProcessingMinutes * multiplier) / multiplier;
            }
            return result.OrderBy(t => t.BusinessArea).ThenBy(t => t.WorkType).ThenBy(t => t.Status).ToList();
        }

        public List<OSC_TeamWorkItem> GetTeamWorkItems(long teamId, int year)
        {
            List<OSC_TeamWorkItem> result = (from list in db.TeamWorkItems
                                             where list.TeamId == teamId &&
                                                    list.Year == year
                                             select list).ToList();
            return result;
        }
    }

    class TeamScorecardClass
    {

    }

    class BTSSExtensions
    {
        private OSCContext db = new OSCContext();
        private GetReference getReference = new GetReference();

        public bool IsManaged(long? teamId, string user_name, string role)
        {
            if (role == "Admin") return true;
            if (role == "Staff")
            {
                var rep = (from r in db.Representatives where r.PRDUserId == user_name && r.TeamId == teamId select r);
                if (rep != null) return true;
                else return false;
            }
            //Team Leader, Manager and Department Analyst
            long? managerId = getReference.GetManagerByUserName(user_name).ManagerId;
            if (managerId == 0 || managerId == null) return false;
            OSC_ManageGroup mg = db.ManageGroups.Where(m => m.Type == "TEAM" && m.EntityId == (long)teamId && m.ManagerId == (long)managerId).FirstOrDefault();
            if (mg != null) return true;
            else
            {
                if (role == "Manager" || role == "Department Analyst")
                {
                    long depId = (long)db.Teams.Find((long)teamId).DepartmentId;
                    mg = db.ManageGroups.Where(m => m.Type == "DEPT" && m.EntityId == depId && m.ManagerId == managerId).FirstOrDefault();
                    if (mg != null) return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}