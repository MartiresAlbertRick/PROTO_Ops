using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;

namespace OPSCO_Web.Models
{
    public interface IOSCContext
    {
        IQueryable<OSC_Department> Departments { get; }
        IQueryable<OSC_Team> Teams { get; }
        OSC_Department FindDepartmentById(long ID);
        int SaveChanges();
        T Add<T>(T entity) where T : class;
        T Delete<T>(T entity) where T : class;
        
    }
}