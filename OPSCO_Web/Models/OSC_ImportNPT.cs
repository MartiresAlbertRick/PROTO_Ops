//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OPSCO_Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OSC_ImportNPT
    {
        public long NPTReportId { get; set; }
        public Nullable<long> RepId { get; set; }
        public string Activity { get; set; }
        public Nullable<System.DateTime> DateOfActivity { get; set; }
        public Nullable<double> TimeSpent { get; set; }
        public string TypeOfActivity { get; set; }
        public string CreatedBy { get; set; }
        public string ItemType { get; set; }
        public string Path { get; set; }
        public Nullable<long> TeamId { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<System.DateTime> DateUploaded { get; set; }
        public string UploadedBy { get; set; }
        public string Source { get; set; }
        public Nullable<long> CategoryId { get; set; }
        public Nullable<long> SubCategoryId { get; set; }
        public bool IsActive { get; set; }
    }
}
