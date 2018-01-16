using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OPSCO_Web.Models
{
    public class OSCContext : DbContext, IOSCContext
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

        public DbSet<OSC_Location> Locations { get; set; }
        public DbSet<OSC_CoreRole> CoreRoles { get; set; }
        #endregion "DBSets"

        #region "IOSCContext"
        IQueryable<OSC_Department> IOSCContext.Departments
        {
            get { return Departments; }
        }

        IQueryable<OSC_Team> IOSCContext.Teams
        {
            get { return Teams; }
        }

        OSC_Department IOSCContext.FindDepartmentById(long ID)
        {
            return Set<OSC_Department>().Find(ID);
        }

        int IOSCContext.SaveChanges()
        {
            return SaveChanges();
        }

        T IOSCContext.Add<T>(T entity)
        {
            return Set<T>().Add(entity);
        }

        T IOSCContext.Delete<T>(T entity)
        {
            return Set<T>().Remove(entity);
        }

        #endregion "IOSCContext"        
    }

    [MetadataType(typeof(OSC_Department.Metadata))]
    public partial class OSC_Department
    {
        sealed class Metadata {
            [Key]
            public long DepartmentId { get; set; }
            [Required]
            [Display(Name = "Department Name")]
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
            [Display(Name = "Team Name")]
            public string TeamName { get; set; }
            public Nullable<long> DepartmentId { get; set; }
            [Display(Name = "Active Flag")]
            public bool IsActive { get; set; }
            public string BIUserGroup { get; set; }
            public string AIQUserGroup { get; set; }
        }
        public virtual OSC_Department Department { get; set; }
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
            public Nullable<long> TeamId { get; set; }
            public Nullable<int> CoreRoleId { get; set; }
            [Display(Name = "Start Date")]
            public Nullable<System.DateTime> StartDate { get; set; }
            [Display(Name = "End Date")]
            public Nullable<System.DateTime> EndDate { get; set; }
            public string Comments { get; set; }
            [Display(Name = "On Shore")]
            public Nullable<bool> OnShoreRep { get; set; }
            [Display(Name = "Phone Representative")]
            public Nullable<bool> PhoneRep { get; set; }
            [Display(Name = "Work Hours")]
            public Nullable<double> WorkHours { get; set; }
            [Display(Name = "Has Previous")]
            public Nullable<bool> HasPrevious { get; set; }
            public Nullable<int> LocationId { get; set; }
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

}