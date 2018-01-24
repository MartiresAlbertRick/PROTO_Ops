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
        public ActionResult Index()
        {
            ViewBag.Months = db.months;
            ViewBag.Years = db.years;
            ViewBag.ErrorBIP = null;
            ViewBag.MessageBIP = null;
            ViewBag.ErrorBIQ = null;
            ViewBag.MessageBIQ = null;
            ViewBag.ErrorAIQ = null;
            ViewBag.MessageAIQ = null;
            ViewBag.ErrorTA = null;
            ViewBag.MessageTA = null;
            ViewBag.ErrorNPT = null;
            ViewBag.MessageNPT = null;
            return View();
        }

        [HttpPost]
        public ActionResult Import([Bind(Include = "ImportId,Month,Year")] Import import,
                                          HttpPostedFileBase bip,
                                          HttpPostedFileBase biq,
                                          HttpPostedFileBase aiq,
                                          HttpPostedFileBase ta,
                                          HttpPostedFileBase npt)
        {
            ViewBag.Months = db.months;
            ViewBag.Years = db.years;
            #region "BIP"
            if (bip == null || bip.ContentLength == 0)
            {
                ViewBag.ErrorBIP = "No file selected";
            }
            else
            {
                if (bip.FileName.EndsWith("xls") || bip.FileName.EndsWith("xlsx"))
                {
                    string fname = Path.GetFileName(bip.FileName);
                    string path = Server.MapPath("~/ImportFile/" + fname);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path); 
                    bip.SaveAs(path);
                    #region "EXCEL"
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
                    #endregion "EXCEL"
                }
                else
                {
                    ViewBag.ErrorBIP = "File type is incorrect";
                }
            }
            #endregion "BIP"
            #region "BIQ"
            if (biq == null || biq.ContentLength == 0)
            {
                ViewBag.ErrorBIQ = "No file selected";
            }
            else
            {
                if (biq.FileName.EndsWith("xls") || biq.FileName.EndsWith("xlsx"))
                {
                    string fname = Path.GetFileName(biq.FileName);
                    string path = Server.MapPath("~/ImportFile/" + fname);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    biq.SaveAs(path);
                    #region "EXCEL"
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    List<OSC_ImportBIQual> list = new List<OSC_ImportBIQual>();
                    for (int row = 2; row <= range.Rows.Count; row++)
                    {
                        OSC_ImportBIQual obj = new OSC_ImportBIQual();
                        obj.Group = ((Excel.Range)range.Cells[row, 1]).Text;
                        obj.UserIdName = ((Excel.Range)range.Cells[row, 2]).Text;
                        obj.BusinessArea = ((Excel.Range)range.Cells[row, 3]).Text;
                        obj.Field1 = ((Excel.Range)range.Cells[row, 4]).Text;
                        obj.Field2 = ((Excel.Range)range.Cells[row, 5]).Text;
                        obj.Count1 = ((Excel.Range)range.Cells[row, 6]).Text;
                        obj.Count2 = ((Excel.Range)range.Cells[row, 7]).Text;
                        obj.Count3 = ((Excel.Range)range.Cells[row, 8]).Text;
                        obj.Count4 = ((Excel.Range)range.Cells[row, 9]).Text;
                        obj.ErrorPoints = ((Excel.Range)range.Cells[row, 10]).Text;
                        obj.Rating = ((Excel.Range)range.Cells[row, 11]).Text;
                        obj.Month = import.Month;
                        obj.Year = import.Year;
                        obj.DateUploaded = DateTime.Now;
                        obj.UploadedBy = User.Identity.Name;
                        list.Add(obj);
                    }
                    foreach (OSC_ImportBIQual obj in list)
                    {
                        obj.Repid = db.Representatives.Where(r => (r.BIUserId == obj.UserIdName) &&
                                                                (r.TeamId == db.GetTeamIdByGroupId(obj.Group, "BI").TeamId)).FirstOrDefault().RepId;
                        obj.TeamId = db.GetTeamIdByGroupId(obj.Group, "BI").TeamId;
                        //db.BIQ.Add(obj);
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
                    #endregion "EXCEL"
                }
                else
                {
                    ViewBag.ErrorBIQ = "File type is incorrect";
                }
            }
            #endregion "BIQ"
            #region "AIQ"
            if (aiq == null || aiq.ContentLength == 0)
            {
                ViewBag.ErrorAIQ = "No file selected";
            }
            else
            {
                if (aiq.FileName.EndsWith("xls") || aiq.FileName.EndsWith("xlsx"))
                {
                    string fname = Path.GetFileName(aiq.FileName);
                    string path = Server.MapPath("~/ImportFile/" + fname);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    aiq.SaveAs(path);
                    #region "EXCEL"
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    List<OSC_ImportBIQual> list = new List<OSC_ImportBIQual>();
                    for (int row = 2; row <= range.Rows.Count; row++)
                    {
                        OSC_ImportBIQual obj = new OSC_ImportBIQual();
                        obj.Group = ((Excel.Range)range.Cells[row, 1]).Text;
                        obj.UserIdName = ((Excel.Range)range.Cells[row, 2]).Text;
                        obj.BusinessArea = ((Excel.Range)range.Cells[row, 3]).Text;
                        obj.Field1 = ((Excel.Range)range.Cells[row, 4]).Text;
                        obj.Field2 = ((Excel.Range)range.Cells[row, 5]).Text;
                        obj.Count1 = ((Excel.Range)range.Cells[row, 6]).Text;
                        obj.Count2 = ((Excel.Range)range.Cells[row, 7]).Text;
                        obj.Count3 = ((Excel.Range)range.Cells[row, 8]).Text;
                        obj.Count4 = ((Excel.Range)range.Cells[row, 9]).Text;
                        obj.ErrorPoints = ((Excel.Range)range.Cells[row, 10]).Text;
                        obj.Rating = ((Excel.Range)range.Cells[row, 11]).Text;
                        obj.Month = import.Month;
                        obj.Year = import.Year;
                        obj.DateUploaded = DateTime.Now;
                        obj.UploadedBy = User.Identity.Name;
                        list.Add(obj);
                    }
                    foreach (OSC_ImportBIQual obj in list)
                    {
                        obj.Repid = db.Representatives.Where(r => (r.BIUserId == obj.UserIdName) &&
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
                    #endregion "EXCEL"
                }
                else
                {
                    ViewBag.ErrorAIQ = "File type is incorrect";
                }
            }
            #endregion "AIQ"
            #region "TA"
            if (ta == null || ta.ContentLength == 0)
            {
                ViewBag.ErrorTA = "No file selected";
            }
            else
            {
                if (ta.FileName.EndsWith("xls") || ta.FileName.EndsWith("xlsx"))
                {
                    string fname = Path.GetFileName(ta.FileName);
                    string path = Server.MapPath("~/ImportFile/" + fname);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    ta.SaveAs(path);
                }
                else
                {
                    ViewBag.ErrorTA = "File type is incorrect";
                }
            }
            #endregion "TA"
            #region "NPT"
            if (npt == null || npt.ContentLength == 0)
            {
                ViewBag.ErrorNPT = "No file selected";
            }
            else
            {
                if (npt.FileName.EndsWith("xls") || npt.FileName.EndsWith("xlsx"))
                {
                    string fname = Path.GetFileName(npt.FileName);
                    string path = Server.MapPath("~/ImportFile/" + fname);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    npt.SaveAs(path);
                }
                else
                {
                    ViewBag.ErrorNPT = "File type is incorrect";
                }
            }
            #endregion "NPT"
            return View("Index");
        }
    }
}