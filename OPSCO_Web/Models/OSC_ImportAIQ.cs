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
    
    public partial class OSC_ImportAIQ
    {
        public long AIQReportId { get; set; }
        public Nullable<long> RepId { get; set; }
        public string Agent { get; set; }
        public Nullable<long> IntervalStaffedDuration { get; set; }
        public Nullable<double> TotalPercServiceTime { get; set; }
        public Nullable<int> TotalACDCalls { get; set; }
        public Nullable<int> ExtInCalls { get; set; }
        public Nullable<long> ExtInAvgActiveDur { get; set; }
        public Nullable<int> ExtOutCalls { get; set; }
        public Nullable<long> AvgExtOutActiveDur { get; set; }
        public Nullable<long> ACDWrapUpTime { get; set; }
        public Nullable<long> ACDTalkTime { get; set; }
        public Nullable<long> ACDRingTime { get; set; }
        public Nullable<long> Aux { get; set; }
        public Nullable<long> AvgHoldDur { get; set; }
        public Nullable<long> IntervalIdleDur { get; set; }
        public Nullable<int> Transfers { get; set; }
        public Nullable<int> HeldContacts { get; set; }
        public Nullable<int> Redirects { get; set; }
        public Nullable<long> TeamId { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<System.DateTime> DateUploaded { get; set; }
        public string UploadedBy { get; set; }
    }
}