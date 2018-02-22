using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BTSS_Auth;

namespace OPSCO_Web.Models
{
    public class OSCContext : DbContext
    {
        public OSCContext() : base("name=OSCEntities")
        { }

        #region "DBSets"
        public DbSet<OSC_Department> Departments { get; set; }
        public DbSet<OSC_Team> Teams { get; set; }
        public DbSet<OSC_Representative> Representatives { get; set; }
        public DbSet<OSC_Manager> Managers { get; set; }
        public DbSet<OSC_ActivityTracker> ActivityTrackers { get; set; }
        public DbSet<OSC_ManualEntry> ManualEntries { get; set; }
        public DbSet<OSC_CustomizeScorecard> CustomizeScorecards { get; set; }
        public DbSet<OSC_WorkType> WorkTypes { get; set; }
        public DbSet<OSC_WorkStatus> Statuses { get; set; }
        public DbSet<OSC_BusinessArea> BusinessAreas { get; set; }

        public DbSet<OSC_Location> Locations { get; set; }
        public DbSet<OSC_CoreRole> CoreRoles { get; set; }
        public DbSet<OSC_NptCategory> NptCategories { get; set; }
        public DbSet<OSC_TeamGroupIds> TeamGroupIds { get; set; }
        public DbSet<OSC_TeamNptCategory> TeamNptCategories { get; set; }
        public DbSet<OSC_ManageGroup> ManageGroups { get; set; }
        public DbSet<OSC_TeamWorkItem> TeamWorkItems { get; set; }
        public DbSet<OSC_ScorecardField> ScorecardFields { get; set; }

        public DbSet<OSC_ImportBIProd> BIP { get; set; }
        public DbSet<OSC_ImportBIQual> BIQ { get; set; }
        public DbSet<OSC_ImportAIQ> AIQ { get; set; }
        public DbSet<OSC_ImportTA> TA { get; set; }
        public DbSet<OSC_ImportNPT> NPT { get; set; }

        public DbSet<OSC_IndividualScorecard_Current> IndividualScorecards { get; set; }
        #endregion "DBSets"

        #region "StaticLists"
        public List<SelectListItem> months = new List<SelectListItem>()
        {
            new SelectListItem { Text = "January", Value = "1" },
            new SelectListItem { Text = "February", Value = "2" },
            new SelectListItem { Text = "March", Value = "3" },
            new SelectListItem { Text = "April", Value = "4" },
            new SelectListItem { Text = "May", Value = "5" },
            new SelectListItem { Text = "June", Value = "6" },
            new SelectListItem { Text = "July", Value = "7" },
            new SelectListItem { Text = "August", Value = "8" },
            new SelectListItem { Text = "September", Value = "9" },
            new SelectListItem { Text = "October", Value = "10" },
            new SelectListItem { Text = "November", Value = "11" },
            new SelectListItem { Text = "December", Value = "12" },
        };
        public List<SelectListItem> years = new List<SelectListItem>()
        {
            new SelectListItem { Text = "2018", Value = "2018" },
            new SelectListItem { Text = "2017", Value = "2017" }
        };
        public List<SelectListItem> activities = new List<SelectListItem>()
        {
            new SelectListItem { Text = "Time-off", Value = "Time-off" },
            new SelectListItem { Text = "Overtime", Value = "Overtime" },
            new SelectListItem { Text = "Holiday", Value = "Holiday" }
        };
        public List<SelectListItem> projectResponsibilities = new List<SelectListItem>()
        {
            new SelectListItem { Text = "Below", Value = "Below" },
            new SelectListItem { Text = "Meets", Value = "Meets" },
            new SelectListItem { Text = "Exceeds", Value = "Exceeds" }
        };
        public List<SelectListItem> userGroupType = new List<SelectListItem>()
        {
            new SelectListItem { Text = "BI", Value = "BI" },
            new SelectListItem { Text = "AIQ", Value = "AIQ" }
        };
        #endregion "StaticLists"

        #region "GetReference"
        public OSC_Representative GetRepresentativeByPRD(string user_name)
        {
            OSC_Representative rep = Representatives.Where(t => t.PRDUserId == user_name && t.IsActive).FirstOrDefault();
            return rep;
        }

        public OSC_Manager GetManagerByUserName(string user_name)
        {
            OSC_Manager mgr = Managers.Where(t => t.PRDUserId == user_name).FirstOrDefault();
            return mgr;
        }

        public OSC_TeamGroupIds GetTeamIdByGroupId(string groupId, string groupType)
        {
            OSC_TeamGroupIds result = TeamGroupIds.Where(t => t.GroupId == groupId && t.GroupType == groupType).FirstOrDefault();
            return result;
        }

        public List<OSC_TeamGroupIds> GetGroupIds(long? teamId)
        {
            var result = TeamGroupIds.Where(t => t.TeamId == teamId);
            return result.ToList();
        }

        public bool IsManaged(long? teamId, string user_name, string role)
        {
            if (role == "Admin") return true;
            if (role == "Staff")
            {
                var rep = (from r in Representatives where r.PRDUserId == user_name && r.TeamId == teamId select r);
                if (rep != null) return true;
                else return false;
            }
            //Team Leader, Manager and Department Analyst
            long? managerId = GetManagerByUserName(user_name).ManagerId;
            if (managerId == 0 || managerId == null) return false;
            OSC_ManageGroup mg = ManageGroups.Where(m => m.Type == "TEAM" && m.EntityId == (long)teamId && m.ManagerId == (long)managerId).FirstOrDefault();
            if (mg != null) return true;
            else
            {
                if (role == "Manager" || role == "Department Analyst")
                {
                    long depId = (long)Teams.Find((long)teamId).DepartmentId;
                    mg = ManageGroups.Where(m => m.Type == "DEPT" && m.EntityId == depId && m.ManagerId == managerId).FirstOrDefault();
                    if (mg != null) return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        #endregion "GetReference"

        #region "InitializeLists"

        public bool InitializeTeams()
        {
            foreach (OSC_Team team in Teams)
            {
                team.Department = Departments.Find(team.DepartmentId);
            }
            return true;
        }

        public bool InitializeRepresentatives()
        {
            foreach (OSC_Representative rep in Representatives)
            {
                rep.FullName = rep.FirstName + " " + rep.LastName;
                rep.Team = Teams.Find(rep.TeamId);
                rep.Location = Locations.Find(rep.LocationId);
                rep.CoreRole = CoreRoles.Find(rep.CoreRoleId);
            }
            return true;
        }

        public bool InitializeNpts()
        {
            foreach (OSC_ImportNPT npt in NPT)
            {
                npt.Team = Teams.Find(npt.TeamId);
                npt.Representative = Representatives.Find(npt.RepId);
                npt.Representative.Location = Locations.Find(npt.Representative.LocationId);
                npt.Representative.CoreRole = CoreRoles.Find(npt.Representative.CoreRoleId);
                npt.Representative.FullName = npt.Representative.FirstName + " " + npt.Representative.LastName;
            }
            return true;
        }

        public bool InitializeActivities()
        {
            foreach (OSC_ActivityTracker act in ActivityTrackers)
            {
                act.Team = Teams.Find(act.TeamId);
                act.Representative = Representatives.Find(act.RepId);
                act.Representative.Location = Locations.Find(act.Representative.LocationId);
                act.Representative.CoreRole = CoreRoles.Find(act.Representative.CoreRoleId);
                act.Representative.FullName = act.Representative.FirstName + " " + act.Representative.LastName;
            }
            return true;
        }

        public bool InitializeManualEntries()
        {
            foreach (OSC_ManualEntry me in ManualEntries)
            {
                me.MonthName = months.Where(m => m.Value == Convert.ToString(me.Month)).First().Text;
                me.Team = Teams.Find(me.TeamId);
                me.Representative = Representatives.Find(me.RepId);
                me.Representative.Location = Locations.Find(me.Representative.LocationId);
                me.Representative.CoreRole = CoreRoles.Find(me.Representative.CoreRoleId);
                me.Representative.FullName = me.Representative.FirstName + " " + me.Representative.LastName;
            }
            return true;
        }

        public bool InitializeTeamNptCategories()
        {
            foreach (OSC_TeamNptCategory n in TeamNptCategories)
            {
                n.CategoryDesc = NptCategories.Find(n.CategoryId).CategoryDesc;
            }
            return true;
        }

        public List<IndividualScorecard> GetIndividualScorecardFull(long teamId, long repId, int month, int year)
        {
            List<IndividualScorecard> result = new List<IndividualScorecard>();
            for (int i = 1; i <= month; i++)
            {
                result.Add(GetIndividualScorecard(teamId, repId, i, year));
            }
            IndividualScorecard total = new IndividualScorecard()
            {
                Month = 13, MonthName = "Total", TeamId = teamId, RepId = repId, Year = year, AverageAuxTime = 0, AverageHandleTime = 0, AverageTalkTime = 0, AverageWrapUpDuration = 0,
                Efficiency = 0, Highlights = "", HoursWorked = 0, SupportLineUtilization = 0, TotalUtilization=0, ProductivityRating=0, individualActivities = GetIndividualActivities(teamId, repId, 13, year),
                individualAIQ = GetIndividualAIQ(teamId, repId, 13, year), individualBIProd = GetIndividualProd(teamId, repId, 13, year), rep = Representatives.Find(repId),
                individualNonProcessing = GetIndividualNonProcessing(teamId, repId, 13, year), individualBIQual = GetIndividualQual(teamId, repId, 13, year)
            };
            //foreach (IndividualScorecard item in result)
            //{
                
            ///}
            result.Add(total);
            return result.OrderBy(t => t.Month).ToList();
        }

        public IndividualScorecard GetIndividualScorecard(long teamId, long repId, int month, int year)
        {
            IndividualScorecard result = new IndividualScorecard();
            result = (from list in IndividualScorecards
                     where list.TeamId == teamId &&
                            list.RepId == repId &&
                            list.Month == month &&
                            list.Year == year
                     select new IndividualScorecard
                     {
                         TeamId = (long)list.TeamId,
                         RepId = (long)list.RepId,
                         Month = (int) list.Month,
                         Year = (int)list.Year,
                         Highlights = (string)list.Comments
                     }).FirstOrDefault();
            if (result != null)
            {
                result.rep = Representatives.Find(repId);
                result.rep.Location = Locations.Find(result.rep.LocationId);
                result.rep.CoreRole = CoreRoles.Find(result.rep.CoreRoleId);
                result.MonthName = months.Where(m => m.Value == Convert.ToString(result.Month)).First().Text;
                result.individualBIProd = GetIndividualProd(teamId, repId, month, year);
                result.individualBIQual = GetIndividualQual(teamId, repId, month, year);
                result.individualAIQ = GetIndividualAIQ(teamId, repId, month, year);
                result.individualNonProcessing = GetIndividualNonProcessing(teamId, repId, month, year);
                result.individualActivities = GetIndividualActivities(teamId, repId, month, year);
                //individualManualEntry
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
                    MonthName = months.Where(m => m.Value == Convert.ToString(month)).First().Text,
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
                    rep = Representatives.Find(repId),
                    individualNonProcessing = GetIndividualNonProcessing(teamId, repId, month, year),
                    individualBIQual = GetIndividualQual(teamId, repId, month, year)
                };
            }
            return result;
        }

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

        

        public IndividualActivities GetIndividualActivities(long teamId, long repId, int month, int year)
        {
            IndividualActivities result = new IndividualActivities() { Attendance_Days = 0, Attendance_Hours=0, Overtime_Days = 0, Overtime_Hours = 0, Holiday_Days = 0, Holiday_Hours = 0, TimeOff_Days = 0, TimeOff_Hours = 0 };
            var x = (from list in ActivityTrackers
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
            var y = (from list in ActivityTrackers
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
            var z = (from list in ActivityTrackers
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
            if (z!=null)
            { 
                foreach (IndividualActivities item in z)
                {
                    result.Overtime_Days += item.Overtime_Days;
                    result.Overtime_Hours += item.Overtime_Hours;
                }
            }
            var a = (from list in ActivityTrackers
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
            if (a != null) { 
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
            OSC_ManualEntry result = (from list in ManualEntries
                     where list.TeamId == teamId && list.RepId == repId && list.Month == month && list.Year == year
                     select list).FirstOrDefault();
            return result;
        }

        public IndividualBIProd GetIndividualProd(long teamId, long repId, int month, int year)
        {
            IndividualBIProd result = new IndividualBIProd();
            var x = (from list in BIP
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
            var x = (from list in BIQ
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
                result = new IndividualBIQual() {
                    TeamId = teamId, RepId = repId, Month = month, Year = year, FailCount = 0, ReviewCount = 0, QualityRating = 0
                };
            }
            return result;
        }

        public OSC_ImportAIQ GetIndividualAIQ(long teamId, long repId, int month, int year)
        {
            OSC_ImportAIQ result = new OSC_ImportAIQ();
            var x = (from list in AIQ
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

        public IndividualNonProcessing GetIndividualNonProcessing(long teamId, long repId, int month, int year)
        {
            IndividualNonProcessing result = new IndividualNonProcessing();
            var x = (from list in NPT
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
                result = new IndividualNonProcessing() {
                    TeamId = teamId, RepId = repId, Month = month, Year = year, NPTHours = 0
                };
            }
            return result;
        }

        public List<IndividualWorkTypes> GetIndividualWorkTypes(long teamId, long repId, int month, int year)
        {
            List<IndividualWorkTypes> result = new List<IndividualWorkTypes>();
            var s = (from list in BIP where list.TeamId == teamId && list.RepId == repId && list.Month == month && list.Year == year
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
            var s = (from list in NPT where list.TeamId == teamId && list.RepId == repId && list.Month == month && list.Year == year
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
            var x = (from list in BIP
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
            return result.OrderBy(t=>t.BusinessArea).ThenBy(t=>t.WorkType).ThenBy(t=>t.Status).ToList();
        }

        public List<OSC_TeamWorkItem> GetTeamWorkItems(long teamId, int year)
        {
            List<OSC_TeamWorkItem> result = (from list in TeamWorkItems
                                             where list.TeamId == teamId &&
                                                    list.Year == year
                                             select list).ToList();
            return result;
        }
        #endregion "InitializeLists"

        #region "DateTimeFormatting"
        //return time string format hh:mm:ss
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

        //return seconds long format sssssss
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
        #endregion "DateTimeFormatting"C:\Users\martiab\Documents\Projects\OperationsScorecard\OPSCO_Web\ImportFile\OSC_Scripts_0124.sql

        #region "BTSS"
        public BTSS_AppFacade appFacade = new BTSS_AppFacade();
        #endregion "BTSS"
    }

    #region "OSCdbEntities"
    [MetadataType(typeof(OSC_Department.Metadata))]
    public partial class OSC_Department
    {
        sealed class Metadata {
            [Key]
            public long DepartmentId { get; set; }
            [Required]
            [Display(Name = "Department")]
            public string DepartmentName { get; set; }
            [Display(Name = "Active Flag")]
            public bool IsActive { get; set; }
        }
    }

    [MetadataType(typeof(OSC_Team.Metadata))]
    public partial class OSC_Team
    {
        sealed class Metadata
        {
            [Key]
            public long TeamId { get; set; }
            [Required]
            [Display(Name = "Team")]
            public string TeamName { get; set; }
            public Nullable<long> DepartmentId { get; set; }
            [Display(Name = "Active Flag")]
            public bool IsActive { get; set; }
            public string BIUserGroup { get; set; }
            public string AIQUserGroup { get; set; }
        }
        public virtual OSC_Department Department { get; set; }
        public virtual ICollection<OSC_TeamGroupIds> GroupIds { get; set; }
    }

    [MetadataType(typeof(OSC_Representative.Metadata))]
    public partial class OSC_Representative
    {
        sealed class Metadata
        {
            [Key]
            public string RepId { get; set; }
            [Display(Name = "PRD Id")]
            public string PRDUserId { get; set; }
            [Display(Name = "AIQ Id")]
            public string AIQUserId { get; set; }
            [Display(Name = "AWD Id")]
            public string BIUserId { get; set; }
            [Display(Name = "Workday Id")]
            public Nullable<long> WorkdayId { get; set; }
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Display(Name = "Middle Name")]
            public string MiddleName { get; set; }
            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            public long TeamId { get; set; }
            public int CoreRoleId { get; set; }
            [Display(Name = "Start Date")]
            [DataType(DataType.Date)]
            public DateTime StartDate { get; set; }
            [Display(Name = "End Date")]
            [DataType(DataType.Date)]
            public DateTime EndDate { get; set; }
            public string Comments { get; set; }
            [Display(Name = "On Shore")]
            public bool OnShoreRep { get; set; }
            [Display(Name = "Phone Representative")]
            public bool PhoneRep { get; set; }
            [Display(Name = "Work Hours")]
            public double WorkHours { get; set; }
            [Display(Name = "Has Previous")]
            public Nullable<bool> HasPrevious { get; set; }
            public int LocationId { get; set; }
            [Display(Name = "Previous Id")]
            public Nullable<long> PreviousId { get; set; }
            [Display(Name = "Current Flag")]
            public Nullable<bool> IsCurrent { get; set; }
            [Display(Name = "Active Flag")]
            public bool IsActive { get; set; }
        }
        public virtual OSC_Team Team { get; set; }
        public virtual OSC_CoreRole CoreRole { get; set; }
        public virtual OSC_Location Location { get; set; }
        [Display(Name = "Representative")]
        public string FullName { get; set; }
    }

    [MetadataType(typeof(OSC_Location.Metadata))]
    public partial class OSC_Location
    {
        sealed class Metadata
        {
            [Key]
            public int LocationId { get; set; }
            [Required]
            public string Location { get; set; }
            [Display(Name = "Active Flag")]
            public Nullable<bool> IsActive { get; set; }
        }
    }

    [MetadataType(typeof(OSC_CoreRole.Metadata))]
    public partial class OSC_CoreRole
    {
        sealed class Metadata
        {
            [Key]
            public int CoreRoleId { get; set; }
            [Required]
            [Display(Name = "Role")]
            public string CoreRole { get; set; }
            public Nullable<int> Tier { get; set; }
            [Display(Name = "Monthly Hours")]
            public Nullable<double> MonthlyHours { get; set; }
            public Nullable<double> Percentage { get; set; }
            [Display(Name = "Active Flag")]
            public Nullable<bool> IsActive { get; set; }
        }
    }

    [MetadataType(typeof(OSC_Manager.Metadata))]
    public partial class OSC_Manager
    {
        sealed class Metadata
        {
            [Key]
            public long ManagerId { get; set; }
            [Display(Name = "PRD Id")]
            public string PRDUserId { get; set; }
            public string Name { get; set; }
            [Display(Name = "Active Flag")]
            public bool IsActive { get; set; }
        }
        public virtual ICollection<OSC_ManageGroup> Groups { get; set; }
    }

    [MetadataType(typeof(OSC_ActivityTracker.Metadata))]
    public partial class OSC_ActivityTracker
    {
        sealed class Metadata
        {
            [Key]
            public long ActivityId { get; set; }
            public long RepId { get; set; }
            public int Month { get; set; }
            public int Year { get; set; }
            [Display(Name = "Date From")]
            [DataType(DataType.Date)]
            public DateTime DateFrom { get; set; }
            [Display(Name = "Date To")]
            [DataType(DataType.Date)]
            public DateTime DateTo { get; set; }
            [Display(Name = "Activity")]
            public string Activity { get; set; }
            [Display(Name = "No of Hours")]
            public double NoOfHours { get; set; }
            [Display(Name = "Date Modified")]
            public DateTime DateModified { get; set; }
            [Display(Name = "Modified By")]
            public string ModifiedBy { get; set; }
            [Display(Name = "No of Days")]
            public double NoOfDays { get; set; }
            public long TeamId { get; set; }
            [Display(Name = "Active Flag")]
            public bool IsActive { get; set; }
        }
        public virtual OSC_Team Team { get; set; }
        public virtual OSC_Representative Representative { get; set; }
    }

    [MetadataType(typeof(OSC_ManualEntry.Metadata))]
    public partial class OSC_ManualEntry
    {
        sealed class Metadata
        {
            [Key]
            public long EntryId { get; set; }
            public long TeamId { get; set; }
            public long RepId { get; set; }
            [Display(Name = "Gain/Loss Occurances")]
            public decimal GainLossOccurances { get; set; }
            [Display(Name = "Gain/Loss Amount")]
            public double GainLossAmount { get; set; }
            [Display(Name = "Call Management Score")]
            public double CallManagementScore { get; set; }
            [Display(Name = "Project Responsibility")]
            public string ProjectResponsibility { get; set; }
            [Display(Name = "Schedule Adherence")]
            public double ScheduleAdherence { get; set; }
            public double Compliance { get; set; }
            [Display(Name = "Product Accuracy")]
            public double ProductAccuracy { get; set; }
            public double Commitment { get; set; }
            [Display(Name = "JH Values")]
            public double JHValues { get; set; }
            [Display(Name = "Call Efficiency")]
            public double CallEfficiency { get; set; }
            public double Engagement { get; set; }
            [Display(Name = "Administrative Procedures")]
            public double AdministrativeProcedures { get; set; }
            [Display(Name = "Active Projects")]
            public int ActiveProjects { get; set; }
            [Display(Name = "Completed Projects")]
            public int CompletedProjects { get; set; }
            public int Month { get; set; }
            public int Year { get; set; }
            [Display(Name = "Date Modified")]
            public DateTime DateUploaded { get; set; }
            [Display(Name = "Modified By")]
            public string UploadedBy { get; set; }
            [Display(Name = "Active Flag")]
            public bool IsActive { get; set; }
        }
        public virtual OSC_Team Team { get; set; }
        public virtual OSC_Representative Representative { get; set; }
        public string MonthName { get; set; }
        [Display(Name = "Period Coverage")]
        [DataType(DataType.Date)]
        public DateTime PeriodCoverage { get; set; }
    }

    [MetadataType(typeof(OSC_ImportNPT.Metadata))]
    public partial class OSC_ImportNPT
    {
        sealed class Metadata
        {
            [Key]
            public long NPTReportId { get; set; }
            public Nullable<long> RepId { get; set; }
            [Display(Name = "Activity Description")]
            public string Activity { get; set; }
            [Display(Name = "Date of Activity")]
            [DataType(DataType.Date)]
            public Nullable<System.DateTime> DateOfActivity { get; set; }
            [Display(Name = "Time Spent")]
            public Nullable<double> TimeSpent { get; set; }
            [Display(Name = "Category")]
            public string TypeOfActivity { get; set; }
            [Display(Name = "Created By")]
            public string CreatedBy { get; set; }
            [Display(Name = "Item Type")]
            public string ItemType { get; set; }
            public string Path { get; set; }
            public Nullable<long> TeamId { get; set; }
            public Nullable<int> Month { get; set; }
            public Nullable<int> Year { get; set; }
            [Display(Name = "Date Modified")]
            public Nullable<System.DateTime> DateUploaded { get; set; }
            [Display(Name = "Modified By")]
            public string UploadedBy { get; set; }
            public string Source { get; set; }
            public Nullable<long> CategoryId { get; set; }
            public Nullable<long> SubCategoryId { get; set; }
            [Display(Name = "Active Flag")]
            public bool IsActive { get; set; }
        }
        public virtual OSC_Team Team { get; set; }
        public virtual OSC_Representative Representative { get; set; }
    }

    [MetadataType(typeof(OSC_TeamNptCategory.Metadata))]
    public partial class OSC_TeamNptCategory
    {
        sealed class Metadata
        {
            public long TeamId { get; set; }
            public long CategoryId { get; set; }
        }
        [Display(Name = "Category")]
        public string CategoryDesc { get; set; }
    }

    [MetadataType(typeof(OSC_TeamGroupIds.Metadata))]
    public partial class OSC_TeamGroupIds
    {
        sealed class Metadata
        {
            public long TGIId { get; set; }
            public long TeamId { get; set; }
            [Display(Name = "Group Id")]
            public string GroupId { get; set; }
            [Display(Name = "Group Type")]
            public string GroupType { get; set; }
        }
    }

    [MetadataType(typeof(OSC_ImportAIQ.Metadata))]
    public partial class OSC_ImportAIQ
    {
        sealed class Metadata
        {
            [Key]
            public long AIQReportId { get; set; }
            public Nullable<long> RepId { get; set; }
            public string Agent { get; set; }
            [Display(Name="Interval Staffed Duration")]
            public Nullable<long> IntervalStaffedDuration { get; set; }
            [Display(Name="Total Perc Service time")]
            public Nullable<double> TotalPercServiceTime { get; set; }
            [Display(Name ="Total ACD Calls")]
            public Nullable<int> TotalACDCalls { get; set; }
            /**/
            public Nullable<int> ExtInCalls { get; set; }
            /**/
            public Nullable<long> ExtInAvgActiveDur { get; set; }
            [Display(Name = "Outbound Calls")]
            public Nullable<int> ExtOutCalls { get; set; }
            /**/
            public Nullable<long> AvgExtOutActiveDur { get; set; }
            [Display(Name = "Wrap Up Duration")]
            public Nullable<long> ACDWrapUpTime { get; set; }
            [Display(Name = "ACD Talk Time")]
            public Nullable<long> ACDTalkTime { get; set; }
            [Display(Name = "ACD Ring Time")]
            public Nullable<long> ACDRingTime { get; set; }
            [Display(Name = "Aux")]
            public Nullable<long> Aux { get; set; }
            [Display(Name ="Average Hold Time")]
            public Nullable<long> AvgHoldDur { get; set; }
            [Display(Name ="Interval Idle Duration")]
            public Nullable<long> IntervalIdleDur { get; set; }
            public Nullable<int> Transfers { get; set; }
            [Display(Name = "Held Contacts")]
            public Nullable<int> HeldContacts { get; set; }
            public Nullable<int> Redirects { get; set; }
            public Nullable<long> TeamId { get; set; }
            public Nullable<int> Month { get; set; }
            public Nullable<int> Year { get; set; }
            public Nullable<System.DateTime> DateUploaded { get; set; }
            public string UploadedBy { get; set; }
        }
    }
    #endregion "OSCdbEntities"

    #region "ViewModel"
    public class IndividualScorecard
    {
        public long TeamId { get; set; }
        public long RepId { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public int Year { get; set; }
        public string Highlights { get; set; }
        [Display(Name = "Rate of Production")]
        public double ProductivityRating { get; set; }
        [Display(Name = "Total Utilization")]
        public double TotalUtilization { get; set; }
        [Display(Name = "Available Base Hours")]
        public double HoursWorked { get; set; }
        public double Efficiency { get; set; }
        [Display(Name = "Average Talk Time")]
        public double AverageTalkTime { get; set; }
        [Display(Name = "Average Wrap Up Duration")]
        public double AverageWrapUpDuration { get; set; }
        [Display(Name = "Average Aux Time")]
        public double AverageAuxTime { get; set; }
        [Display(Name = "Average Handle Time")]
        public double AverageHandleTime { get; set; }
        [Display(Name = "Support Line Utilization")]
        public double SupportLineUtilization { get; set; }
        public virtual IndividualActivities individualActivities { get; set; }
        public virtual OSC_ManualEntry individualManualEntries { get; set; }
        public virtual IndividualBIProd individualBIProd { get; set; }
        public virtual IndividualBIQual individualBIQual { get; set; }
        public virtual OSC_ImportAIQ individualAIQ { get; set; }
        public virtual IndividualNonProcessing individualNonProcessing { get; set; }
        public virtual OSC_Representative rep { get; set; }

    }

    public class IndividualBIProd
    {
        public long TeamId { get; set; }
        public long RepId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        [Display(Name = "Total Transactions")]
        public int Count { get; set; }
        [Display(Name = "Processing Time")]
        public double ProcessingHours { get; set; }
    }

    public class IndividualBIProdTimings
    {
        public long TeamId { get; set; }
        public long RepId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string BusinessArea { get; set; }
        public string WorkType { get; set; }
        public string Status { get; set; }
        public int Count { get; set; }
        public double ProcessingMinutes { get; set; }
    }

    public class IndividualBIQual
    {
        public long TeamId { get; set; }
        public long RepId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        [Display(Name = "Review Count")]
        public int ReviewCount { get; set; }
        [Display(Name = "Fail Count")]
        public int FailCount { get; set; }
        [Display(Name = "Processing Quality")]
        public double QualityRating { get; set; }
    }

    public class IndividualNonProcessing
    {
        public long TeamId { get; set; }
        public long RepId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        [Display(Name = "NPT Hours")]
        public double NPTHours { get; set; }
    }

    public class IndividualActivities
    {
        public long TeamId { get; set; }
        public long RepId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        [Display(Name = "Attendance")]
        public double Attendance_Days { get; set; }
        public double Attendance_Hours { get; set; }
        [Display(Name = "Holidays")]
        public double Holiday_Days { get; set; }
        public double Holiday_Hours { get; set; }
        public double Overtime_Days { get; set; }
        [Display(Name = "Overtime")]
        public double Overtime_Hours { get; set; }
        [Display(Name = "Time-Off")]
        public double TimeOff_Days { get; set; }
        public double TimeOff_Hours { get; set; }
    }

    public class IndividualWorkTypes
    {
        [Display(Name = "Worktype")]
        public string WorkType { get; set; }
        public int Count { get; set; }
    }

    public class IndividualNPT
    {
        public string Category { get; set; }
        [Display(Name = "Time Spent")]
        public double TimeSpent { get; set; }
    }

    public class Import
    {
        [Key]
        public int ImportId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
    #endregion "ViewModel"
}