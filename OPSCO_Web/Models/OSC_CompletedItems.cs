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
    
    public partial class OSC_CompletedItems
    {
        public long CompletedUnitId { get; set; }
        public string CompletedUnitName { get; set; }
        public Nullable<int> CompletedUnitCount { get; set; }
        public Nullable<int> CompletedUnitStatusChanges { get; set; }
        public long TeamId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
