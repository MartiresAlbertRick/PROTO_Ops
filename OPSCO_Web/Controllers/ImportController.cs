using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPSCO_Web.Models;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using OPSCO_Web.BL;

namespace OPSCO_Web.Controllers
{
    public class ImportController : Controller
    {
        private OSCContext db = new OSCContext();
        private AppFacade af = new AppFacade();

        // GET: Import
        public ActionResult Index()
        {
            ViewBag.Months = db.months;
            ViewBag.Years = db.years;
            ViewBag.ErrorBIP = null;
            ViewBag.MessageBIP = null;
            ViewBag.ErrorBIQ = null;
            ViewBag.MessageBIQ = null;
            ViewBag.ErrorBIQD = null;
            ViewBag.MessageBIQD = null;
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
                                          HttpPostedFileBase biqd,
                                          HttpPostedFileBase aiq,
                                          HttpPostedFileBase ta,
                                          HttpPostedFileBase npt)
        {
            string logon_user = Session["logon_user"].ToString();
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
                    ImportFiles i = new ImportFiles();
                    int result1 = i.ImportBIProd(path, import, DateTime.Now, logon_user);
                    if (result1 == 1)
                    {
                        ViewBag.MessageBIP = "Success";
                    }
                    else
                    {
                        ViewBag.ErrorBIP = "Failed";
                    }
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
                    ImportFiles i = new ImportFiles();
                    int result2 = i.ImportBIQual(path, import, DateTime.Now, logon_user);
                    if (result2 == 1)
                    {
                        ViewBag.MessageBIQ = "Success";
                    }
                    else
                    {
                        ViewBag.ErrorBIQ = "Failed";
                    }
                }
                else
                {
                    ViewBag.ErrorBIQ = "File type is incorrect";
                }
            }
            #endregion "BIQ"
            #region "BIQD"
            if (biqd == null || biqd.ContentLength == 0)
            {
                ViewBag.ErrorBIQD = "No file selected";
            }
            else
            {
                if (biqd.FileName.EndsWith("xls") || biqd.FileName.EndsWith("xlsx"))
                {
                    string fname = Path.GetFileName(biqd.FileName);
                    string path = Server.MapPath("~/ImportFile/" + fname);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    biqd.SaveAs(path);
                    ImportFiles i = new ImportFiles();
                    int result2 = i.ImportBIQualD(path, import, DateTime.Now, logon_user);
                    if (result2 == 1)
                    {
                        ViewBag.MessageBIQD = "Success";
                    }
                    else
                    {
                        ViewBag.ErrorBIQD = "Failed";
                    }
                }
                else
                {
                    ViewBag.ErrorBIQD = "File type is incorrect";
                }
            }
            #endregion "BIQD"
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
                    List<OSC_ImportAIQ> list = new List<OSC_ImportAIQ>();
                    for (int row = 2; row <= range.Rows.Count; row++)
                    {
                        OSC_ImportAIQ obj = new OSC_ImportAIQ();
                        obj.Agent = ((Excel.Range)range.Cells[row, 1]).Text;
                        obj.IntervalStaffedDuration = af.GetSecondsFormat(((Excel.Range)range.Cells[row, 2]).Text);
                        obj.TotalPercServiceTime = Convert.ToDouble(((Excel.Range)range.Cells[row, 3]).Text);
                        obj.TotalACDCalls = Convert.ToInt32(((Excel.Range)range.Cells[row, 4]).Text);
                        obj.ExtInCalls = Convert.ToInt32(((Excel.Range)range.Cells[row, 5]).Text);
                        obj.ExtInAvgActiveDur = af.GetSecondsFormat(((Excel.Range)range.Cells[row, 6]).Text);
                        obj.ExtOutCalls = Convert.ToInt32(((Excel.Range)range.Cells[row, 7]).Text);
                        obj.AvgExtOutActiveDur = af.GetSecondsFormat(((Excel.Range)range.Cells[row, 8]).Text);
                        obj.ACDWrapUpTime = af.GetSecondsFormat(((Excel.Range)range.Cells[row, 9]).Text);
                        obj.ACDTalkTime = af.GetSecondsFormat(((Excel.Range)range.Cells[row, 10]).Text);
                        obj.ACDRingTime = af.GetSecondsFormat(((Excel.Range)range.Cells[row, 11]).Text);
                        obj.Aux = af.GetSecondsFormat(((Excel.Range)range.Cells[row, 12]).Text);
                        obj.AvgHoldDur = af.GetSecondsFormat(((Excel.Range)range.Cells[row, 13]).Text);
                        obj.IntervalIdleDur = af.GetSecondsFormat(((Excel.Range)range.Cells[row, 14]).Text);
                        obj.Transfers = Convert.ToInt32(((Excel.Range)range.Cells[row, 15]).Text);
                        obj.HeldContacts = Convert.ToInt32(((Excel.Range)range.Cells[row, 16]).Text);
                        obj.Redirects = Convert.ToInt32(((Excel.Range)range.Cells[row, 17]).Text);
                        obj.Month = import.Month;
                        obj.Year = import.Year;
                        obj.DateUploaded = DateTime.Now;
                        obj.UploadedBy = User.Identity.Name;
                        list.Add(obj);
                    }
                    foreach (OSC_ImportAIQ obj in list)
                    {
                        obj.RepId = db.Representatives.Where(r => (r.AIQUserId == obj.Agent) && ((bool)r.IsCurrent)).FirstOrDefault().RepId;
                        obj.TeamId = db.Representatives.Where(r => (r.AIQUserId == obj.Agent) && ((bool)r.IsCurrent)).FirstOrDefault().TeamId;
                        db.AIQ.Add(obj);
                    }
                    try
                    {
                        db.SaveChanges();
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
                    #region "EXCEL"
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    List<OSC_ImportTA> list = new List<OSC_ImportTA>();
                    for (int row = 2; row <= range.Rows.Count; row++)
                    {
                        OSC_ImportTA obj = new OSC_ImportTA();
                        obj.AssignedId = ((Excel.Range)range.Cells[row, 1]).Text;
                        obj.Group = ((Excel.Range)range.Cells[row, 2]).Text;
                        obj.FirstName = ((Excel.Range)range.Cells[row, 3]).Text;
                        obj.MiddleInt = ((Excel.Range)range.Cells[row, 4]).Text;
                        obj.LastName = ((Excel.Range)range.Cells[row, 5]).Text;
                        obj.CreateDateTime = ((Excel.Range)range.Cells[row, 6]).Text;
                        obj.BusinessArea = ((Excel.Range)range.Cells[row, 7]).Text;
                        obj.WorkType = ((Excel.Range)range.Cells[row, 8]).Text;
                        obj.Status = ((Excel.Range)range.Cells[row, 9]).Text;
                        obj.Queue = ((Excel.Range)range.Cells[row, 10]).Text;
                        obj.Suspended = ((Excel.Range)range.Cells[row, 11]).Text;
                        obj.SuspendDate = ((Excel.Range)range.Cells[row, 12]).Text;
                        obj.UnsuspendDate = ((Excel.Range)range.Cells[row, 13]).Text;
                        obj.LastStatusUpdate = ((Excel.Range)range.Cells[row, 14]).Text;
                        obj.Account = ((Excel.Range)range.Cells[row, 15]).Text;
                        obj.GAC = ((Excel.Range)range.Cells[row, 16]).Text;
                        obj.Assoc = ((Excel.Range)range.Cells[row, 17]).Text;
                        obj.Certificate = ((Excel.Range)range.Cells[row, 18]).Text;
                        obj.CheckAmount = ((Excel.Range)range.Cells[row, 19]).Text;
                        obj.First_Name = ((Excel.Range)range.Cells[row, 20]).Text;
                        obj.Last_Name = ((Excel.Range)range.Cells[row, 21]).Text;
                        obj.CustomerNo = ((Excel.Range)range.Cells[row, 22]).Text;
                        obj.ProductType = ((Excel.Range)range.Cells[row, 23]).Text;
                        obj.AdminSystem = ((Excel.Range)range.Cells[row, 24]).Text;
                        obj.CheckAmountTotal = ((Excel.Range)range.Cells[row, 25]).Text;
                        obj.UCIVendorMatchDate = ((Excel.Range)range.Cells[row, 26]).Text;
                        obj.ReasonCodeForAdv = ((Excel.Range)range.Cells[row, 27]).Text;
                        obj.ReasonDescription = ((Excel.Range)range.Cells[row, 28]).Text;
                        obj.TinSourceType = ((Excel.Range)range.Cells[row, 29]).Text;
                        obj.SSBusinessUnit = ((Excel.Range)range.Cells[row, 30]).Text;
                        obj.Month = import.Month;
                        obj.Year = import.Year;
                        obj.DateUploaded = DateTime.Now;
                        obj.UploadedBy = User.Identity.Name;
                        list.Add(obj);
                    }
                    foreach (OSC_ImportTA obj in list)
                    {
                        if (Convert.ToDateTime(obj.CreateDateTime).Month == obj.Month)
                        {
                            obj.RepId = db.Representatives.Where(r => (r.PRDUserId == obj.AssignedId) && ((bool)r.IsCurrent)).FirstOrDefault().RepId;
                            obj.TeamId = af.GetTeamIdByGroupId(obj.Group, "BI").TeamId;
                            db.TA.Add(obj);
                        }
                    }
                    try
                    {
                        db.SaveChanges();
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
                    #region "EXCEL"
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    List<OSC_ImportNPT> list = new List<OSC_ImportNPT>();
                    for (int row = 2; row <= range.Rows.Count; row++)
                    {
                        OSC_ImportNPT obj = new OSC_ImportNPT();
                        obj.Activity = ((Excel.Range)range.Cells[row, 1]).Text;
                        obj.DateOfActivity = Convert.ToDateTime(((Excel.Range)range.Cells[row, 2]).Text);
                        obj.TimeSpent = Convert.ToDouble(((Excel.Range)range.Cells[row, 3]).Text) * 60;
                        obj.TypeOfActivity = ((Excel.Range)range.Cells[row, 4]).Text;
                        obj.CreatedBy = ((Excel.Range)range.Cells[row, 5]).Text;
                        obj.ItemType = ((Excel.Range)range.Cells[row, 6]).Text;
                        obj.Path = ((Excel.Range)range.Cells[row, 7]).Text;
                        obj.Month = import.Month;
                        obj.Year = import.Year;
                        obj.DateUploaded = DateTime.Now;
                        obj.UploadedBy = User.Identity.Name;
                        obj.Source = "Import";
                        list.Add(obj);
                    }
                    foreach (OSC_ImportNPT obj in list)
                    {
                        if (Convert.ToDateTime(obj.DateOfActivity).Month == obj.Month)
                        { 
                            obj.RepId = db.Representatives.Where(r => (r.LastName + ", " + r.FirstName == obj.CreatedBy) &&
                                                                    ((bool)r.IsCurrent)).FirstOrDefault().RepId;
                            obj.TeamId = db.Representatives.Where(r => (r.LastName + ", " + r.FirstName == obj.CreatedBy) && 
                                                                    ((bool)r.IsCurrent)).FirstOrDefault().TeamId;
                            db.NPT.Add(obj);
                        }
                    }
                    try
                    {
                        db.SaveChanges();
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
                    ViewBag.ErrorNPT = "File type is incorrect";
                }
            }
            #endregion "NPT"
            return View("Index");
        }
    }
}