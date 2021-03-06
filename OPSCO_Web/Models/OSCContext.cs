﻿using System;
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
    #region OSCContext
    public class OSCContext : DbContext
    {
        #region Constructor
        public OSCContext() : base("name=OSCEntities")
        {
            //
        }
        #endregion Constructor

        #region DBSets
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
        public DbSet<OSC_TeamScorecardAppendix> Appendix { get; set; }
        public DbSet<OSC_CompletedItems> CompletedItems { get; set; }

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
        public DbSet<OSC_ImportBIQualDetailed> BIQD { get; set; }
        public DbSet<OSC_ImportAIQ> AIQ { get; set; }
        public DbSet<OSC_ImportTA> TA { get; set; }
        public DbSet<OSC_ImportNPT> NPT { get; set; }

        public DbSet<OSC_IndividualScorecard_Current> IndividualScorecards { get; set; }
        public DbSet<OSC_TeamScorecard_Current> TeamScorecards { get; set; }
        #endregion DBSets

        #region StaticLists
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
        public List<SelectListItem> scorecardOptions = new List<SelectListItem>()
        {
            new SelectListItem { Text = "Individual Scorecard", Value = "IndividualScorecard" },
            new SelectListItem { Text = "Team Scorecard", Value = "TeamScorecard" }
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
        #endregion StaticLists
    }
    #endregion OSCContext

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
            public bool HasPrevious { get; set; }
            public int LocationId { get; set; }
            [Display(Name = "Previous Id")]
            public Nullable<long> PreviousId { get; set; }
            [Display(Name = "Current Flag")]
            public bool IsCurrent { get; set; }
            [Display(Name = "VPN Capable")]
            public bool IsVPN { get; set; }
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
            [Display(Name = "Onshore")]
            public Nullable<bool> OnShore { get; set; }
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
            [Display(Name = "Interval Staffed Duration")]
            public Nullable<long> IntervalStaffedDuration { get; set; }
            [Display(Name = "Total Perc Service time")]
            public Nullable<double> TotalPercServiceTime { get; set; }
            [Display(Name = "Total ACD Calls")]
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
            [Display(Name = "Average Hold Time")]
            public Nullable<long> AvgHoldDur { get; set; }
            [Display(Name = "Interval Idle Duration")]
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

    #region "Scorecard"
    public class Scorecard
    {
        [Display(Name = "Available Base Hours")]
        public double HoursWorked { get; set; }
        [Display(Name = "Rate of Production")]
        public double ProductivityRating { get; set; }
        [Display(Name = "Total Utilization")]
        public double TotalUtilization { get; set; }
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
        //Activity
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
        //NPT
        [Display(Name = "NPT Hours")]
        public double NPTHours { get; set; }
        //BI Qual
        [Display(Name = "Status Count")]
        public int StatusCount { get; set; }
        [Display(Name = "Select Count")]
        public int SelectCount { get; set; }
        [Display(Name = "Review Count")]
        public int ReviewCount { get; set; }
        [Display(Name = "Fail Count")]
        public int FailCount { get; set; }
        [Display(Name = "Processing Quality")]
        public double QualityRating { get; set; }
        [Display(Name = "Quality Review Rate")]
        public double QualityReviewRate { get; set; }
        //BI Prod
        [Display(Name = "Total Transactions")]
        public int TotalTransactions { get; set; }
        [Display(Name = "Processing Time")]
        public double ProcessingHours { get; set; }
        //Manual Entries
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
        //AIQ
        [Display(Name = "Interval Staffed Duration")]
        public string IntervalStaffedDuration { get; set; } //hh:mm:ss
        public long Sec_IntervalStaffedDuration { get; set; } //ssssss
        [Display(Name = "Total Perc Service time")]
        public double TotalPercServiceTime { get; set; }
        [Display(Name = "Total ACD Calls")]
        public int TotalACDCalls { get; set; }
        /**/
        public int ExtInCalls { get; set; }
        /**/
        public string ExtInAvgActiveDur { get; set; } //hh:mm:ss
        public long Sec_ExtInAvgActiveDur { get; set; } //ssssss
        [Display(Name = "Outbound Calls")]
        public int ExtOutCalls { get; set; }
        /**/
        public string AvgExtOutActiveDur { get; set; } //hh:mm:ss
        public long Sec_AvgExtOutActiveDur { get; set; } //ssssss
        [Display(Name = "Wrap Up Duration")]
        public string ACDWrapUpTime { get; set; } //hh:mm:ss
        public long Sec_ACDWrapUpTime { get; set; } //ssssss
        [Display(Name = "ACD Talk Time")]
        public string ACDTalkTime { get; set; } //hh:mm:ss
        public long Sec_ACDTalkTime { get; set; } //ssssss
        [Display(Name = "ACD Ring Time")]
        public string ACDRingTime { get; set; } //hh:mm:ss
        public long Sec_ACDRingTime { get; set; } //ssssss
        [Display(Name = "Aux")]
        public string Aux { get; set; } //hh:mm:ss
        public long Sec_Aux { get; set; } //ssssss
        [Display(Name = "Average Hold Time")]
        public string AvgHoldDur { get; set; } //hh:mm:ss
        public long Sec_AvgHoldDur { get; set; } //ssssss
        [Display(Name = "Interval Idle Duration")]
        public string IntervalIdleDur { get; set; } //hh:mm:ss
        public long Sec_IntervalIdleDur { get; set; } //ssssss
        public int Transfers { get; set; }
        [Display(Name = "Held Contacts")]
        public int HeldContacts { get; set; }
        public int Redirects { get; set; }
    }
    #endregion "Scorecard"

    #region "TeamScorecard"
    public class TeamScorecard : Scorecard
    {
        public long TeamScorecardId { get; set; }
        public long TeamId { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public int Year { get; set; }
        public double ProductivityGoal { get; set; }
        public double QualityGoal { get; set; }
        public double EfficiencyGoal { get; set; }
        public double UtilizationGoal { get; set; }
        public string IndividualSummaryComments { get; set; }
        public string TeamSummaryComments { get; set; }
        public string WorktypeSummaryComments { get; set; }
        public string StatusSummaryComments { get; set; }
        public bool IsSignedOff { get; set; }
        public string ManagerSignOff { get; set; }
        public string SignOffBy { get; set; }
        public DateTime SignOffDate { get; set; }
        public Scorecard Scorecard { get; set; }
    }

    public class IndividualSummary
    {
        [Display(Name = "User ID")]
        public string UserId { get; set; }
        [Display(Name = "Name")]
        public string RepName { get; set; }
        [Display(Name = "Utilization Score")]
        public double UtilizationScore { get; set; }
        [Display(Name = "Productivity Score")]
        public double ProductivityScore { get; set; }
        [Display(Name = "Efficiency Score")]
        public double EfficiencyScore { get; set; }
        [Display(Name = "Quality Score")]
        public double QualityScore { get; set; }
        [Display(Name = "Attendance")]
        public double Attendance { get; set; }
        [Display(Name = "Non-Processing Time")]
        public double NonProcessingTime { get; set; }
        [Display(Name = "Quality Review Rate")]
        public double QualityReviewRate { get; set; }

    }
    #endregion "TeamScorecard"

    #region "IndividualScorecard"
    public class IndividualScorecard
    {
        public long IndividualScorecardId { get; set; }
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
    #endregion "IndividualScorecard"

    #region "IndividualVariables"
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
        [Display(Name = "Status Count")]
        public int StatusCount { get; set; }
        [Display(Name = "Select Count")]
        public int SelectCount { get; set; }
        [Display(Name = "Review Count")]
        public int ReviewCount { get; set; }
        [Display(Name = "Fail Count")]
        public int FailCount { get; set; }
        [Display(Name = "Processing Quality")]
        public double QualityRating { get; set; }
        [Display(Name = "Quality Review Rate")]
        public double QualityReviewRate { get; set; }
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
    #endregion "IndividualVariables"

    public class TeamRanking
    {
        [Display(Name = "Team")]
        public string Team { get; set; }
        [Display(Name = "Productivity Rating")]
        public double ProductivityRating { get; set; }
        [Display(Name = "Quality Rating")]
        public double QualityRating { get; set; }
        [Display(Name = "Efficiency Rating")]
        public double EfficiencyRating { get; set; }
        [Display(Name = "Utilization Rating")]
        public double UtilizationRating { get; set; }
        [Display(Name = "Combined Score")]
        public double CombinedScore { get; set; }
        [Display(Name = "Ranking")]
        public int Ranking { get; set; }
        [Display(Name = "Health")]
        public string Health { get; set; }
        public string Color { get; set; }
    }

    public class WorktypeSummary
    {
        public string Worktype { get; set; }
        public int TotalItemsProcess { get; set; }
        public int TotalItemsSelected { get; set; }
        public double PercSelectedforQC { get; set; }
        public int TotalItemsReviewed { get; set; }
        public double PercSelectedQCd { get; set; }
    }

    public class StatusSummary
    {
        public string Worktype { get; set; }
        public string Status { get; set; }
        public double QCParameter { get; set; }
        public int CompletedCount { get; set; }
        public int SelectedCount { get; set; }
        public int ReviewedCount { get; set; }
        public double ReviewedPerc { get; set; }
    }

    public class LocationSummary
    {
        [Display(Name = "Location")]
        public string Location { get; set; }
        [Display(Name = "Representative Count")]
        public int RepCount { get; set; }
        [Display(Name = "Representative Percentage")]
        public double RepPerc { get; set; }
        [Display(Name = "Onshore/Offshore")]
        public string Shore { get; set; }
    }

    public class LocationAllocation
    {
        [Display(Name = "Onshore")]
        public string OnShore { get; set; }
        [Display(Name = "Onshore Locations")]
        public string OnShoreLoc { get; set; }
        [Display(Name = "Onshore Count")]
        public double OnShoreCount { get; set; }
        [Display(Name = "Onshore Perc")]
        public double OnShorePerc { get; set; }
        [Display(Name = "Offshore")]
        public string OffShore { get; set; }
        [Display(Name = "Offshore Locations")]
        public string OffShoreLoc { get; set; }
        [Display(Name = "Offshore Count")]
        public double OffShoreCount { get; set; }
        [Display(Name = "Offshore Perc")]
        public double OffShorePerc { get; set; }
        [Display(Name = "Onshore - Offshore")]
        public string OnVsOff { get; set; }
    }


    public class OutstandingInventoryTable
    {
        public int Count { get; set; }
    }

    public class OutstandingInventoryTableDetailed {
        public string BusinessArea { get; set; }
        public string Worktype { get; set; }
        public string Status { get; set; }
        public int Count { get; set; }
    }

    public class CustomizedScorecardFields
    {
        public string FieldName { get; set; }
        public int SortOrder { get; set; }
    }

    #region "ChartModels"
    public class PieList
    {
        public string Category { get; set; }
        [Display(Name = "Time Spent")]
        public double TimeSpent { get; set; }
    }

    public class BarChart
    {
        public string Label { get; set; }
        public double Count { get; set; }
    }
    #endregion "ChartModels"

    #region "Import"
    public class Import
    {
        [Key]
        public int ImportId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
    #endregion "Import"
    #endregion "ViewModel"
}