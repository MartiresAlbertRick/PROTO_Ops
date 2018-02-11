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
        public DbSet<OSC_CustomizeScorecard> CustomizeScorecards {get; set;}
        public DbSet<OSC_WorkType> WorkTypes { get; set; }
        public DbSet<OSC_WorkStatus> Statuses { get; set; }
        public DbSet<OSC_BusinessArea> BusinessAreas { get; set; }

        public DbSet<OSC_Location> Locations { get; set; }
        public DbSet<OSC_CoreRole> CoreRoles { get; set; }
        public DbSet<OSC_NptCategory> NptCategories { get; set; }
        public DbSet<OSC_TeamGroupIds> TeamGroupIds { get; set; }
        public DbSet<OSC_TeamNptCategory> TeamNptCategories { get; set; }
        public DbSet<OSC_ManageGroup> ManageGroups { get; set; }

        public DbSet<OSC_ImportBIProd> BIP { get; set; }
        public DbSet<OSC_ImportBIQual> BIQ { get; set; }
        public DbSet<OSC_ImportAIQ> AIQ { get; set; }
        public DbSet<OSC_ImportTA> TA { get; set; }
        public DbSet<OSC_ImportNPT> NPT { get; set; }
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

    public class IndividualScorecard
    {
        [Key]
        public int IndividualScorecardId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public long TeamId { get; set; }
        public long RepId { get; set; }
    }

    public class Import
    {
        [Key]
        public int ImportId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }

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
            [Display(Name = " Gain/Loss Occurances")]
            public decimal GainLossOccurances { get; set; }
            [Display(Name = " Gain/Loss Amount")]
            public double GainLossAmount { get; set; }
            [Display(Name = " Call Management Score")]
            public double CallManagementScore { get; set; }
            [Display(Name = " Project Responsibility")]
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
}