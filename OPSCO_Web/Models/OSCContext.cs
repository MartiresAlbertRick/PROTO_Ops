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
        public System.Data.Entity.DbSet<OSC_Department> Departments { get; set; }
        public System.Data.Entity.DbSet<OSC_Team> Teams { get; set; }
        public System.Data.Entity.DbSet<OSC_Representative> Representatives { get; set; }
        public System.Data.Entity.DbSet<OSC_Manager> Managers { get; set; }
        public System.Data.Entity.DbSet<OSC_ActivityTracker> ActivityTrackers { get; set; }
        public System.Data.Entity.DbSet<OSC_ManualEntry> ManualEntries { get; set; }
        public System.Data.Entity.DbSet<OSC_CustomizeScorecard> CustomizeScorecards {get; set;}
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

    public class DepartmentViewModel
    {
        [Display(Name = "Department")]
        [Required(ErrorMessage = "Please select a department")]
        public long SelectedDepartment { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
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
}