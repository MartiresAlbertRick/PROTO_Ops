using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPSCO_Web.Models;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace OPSCO_Web.Controllers
{
    public class ImportController : Controller
    {
        private OSCContext db = new OSCContext();

        // GET: Import
        public ActionResult Index(HttpPostedFileBase bip)
        {
            ViewBag.Months = db.months;
            ViewBag.Years = db.years;
            return View();
        }

        [HttpPost]
        public ActionResult Import([Bind(Include = "ImportId,Month,Year")] Import import, HttpPostedFileBase bip)
        {
            ViewBag.Months = db.months;
            ViewBag.Years = db.years;
            #region "BIP"
            if (bip == null || bip.ContentLength == 0)
            {
                ViewBag.ErrorBIP = "Please select an excel file";
            }
            else
            {
                if (bip.FileName.EndsWith("xls") || bip.FileName.EndsWith("xlsx"))
                {
                    string path = Server.MapPath("~/ImportFile/" + Path.GetFileName(bip.FileName));
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path); 
                    bip.SaveAs(path);
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    List<OSC_ImportBIProd> list = new List<OSC_ImportBIProd>();
                    for (int row = 2; row <= range.Rows.Count; row++)
                    {
                        OSC_ImportBIProd obj = new OSC_ImportBIProd();
                        obj.Group = ((Excel.Range)range.Cells[row, 1]).Text;
                        obj.UserIdName = ((Excel.Range)range.Cells[row, 2]).Text;
                        obj.BusinessArea = ((Excel.Range)range.Cells[row, 3]).Text;
                        obj.WorkType = ((Excel.Range)range.Cells[row, 4]).Text;
                        obj.Status = ((Excel.Range)range.Cells[row, 5]).Text;
                        obj.Count = ((Excel.Range)range.Cells[row, 6]).Text;
                        obj.Date1 = ((Excel.Range)range.Cells[row, 7]).Text;
                        obj.Date2 = ((Excel.Range)range.Cells[row, 8]).Text;
                        obj.Rating = ((Excel.Range)range.Cells[row, 9]).Text;
                        obj.Month = import.Month;
                        obj.Year = import.Year;
                        obj.DateUploaded = DateTime.Now;
                        obj.UploadedBy = User.Identity.Name;
                        list.Add(obj);
                    }
                    foreach (OSC_ImportBIProd obj in list)
                    {
                        obj.RepId = db.Representatives.Where(r => (r.BIUserId == obj.UserIdName) &&
                                                                (r.TeamId == db.GetTeamIdByGroupId(obj.Group, "BI").TeamId)).FirstOrDefault().RepId;
                        obj.TeamId = db.GetTeamIdByGroupId(obj.Group, "BI").TeamId;
                        //db.BIP.Add(obj);
                    }
                    try
                    {
                        //db.SaveChanges();
                        ViewBag.MessageBIP = "Success";
                        if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorBIP = "Error: " + ex.Message.ToString();
                        if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    }
                }
                else
                {
                    ViewBag.ErrorBIP = "File type is incorrect";
                }
            }
            #endregion "BIP"
            #region "BIQ"
            #endregion "BIQ"
            #region "AIQ"
            #endregion "AIQ"
            #region "TA"
            #endregion "TA"
            #region "NPT"
            #endregion "NPT"
            return View("Index");
        }
    }
}