using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPSCO_Web.Models
{
    public class _Access
    {
        public string LogonUser { get; set; }
        public string Role { get; set; }
        public string Group { get; set; }
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }
}