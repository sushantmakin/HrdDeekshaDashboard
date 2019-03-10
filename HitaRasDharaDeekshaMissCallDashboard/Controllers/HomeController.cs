using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using HitaRasDharaDeekshaMissCallDashboard.Models;

namespace HitaRasDharaDeekshaMissCallDashboard.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ApplicationDbContext _dbContext = new ApplicationDbContext();
            ViewBag.StatusMapping = _dbContext.StatusMappingTable.Select(m => m).Where(x => x.Visible);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(HomeViewModel input)
        {
            ApplicationDbContext _DbContext = new ApplicationDbContext();
            try
            {
                if (ModelState.IsValid)
                {
                        _DbContext.DeekshaStatusTable.Add(input);
                        _DbContext.SaveChanges();
                        bool isSmsSent = new SmsSenderController().InvokeSystemSms(input.DeekshaId);
                        if (isSmsSent)
                        {
                            return Json(new { Code = 0, UniqueNumber = input.DeekshaId }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { Code = 7 }, JsonRequestBehavior.AllowGet);
                    }
            }
            catch (Exception ex)
            {
                return Json(new { Code = 2 }, JsonRequestBehavior.AllowGet);
            }
            return View();
        }

        public ActionResult Dashboard()
        {
            ApplicationDbContext _DbContextForDashboard = new ApplicationDbContext();
            var viewModel =
                new DashboardViewModel { ContentItems = _DbContextForDashboard.DeekshaStatusTable.Select(m => m).ToList() };
            ViewBag.statusData = _DbContextForDashboard.StatusMappingTable.ToList();
            return View(viewModel);
        }

        public ActionResult UpdateStatus()
        {
            ApplicationDbContext _dbContext = new ApplicationDbContext();
            ViewBag.StatusMapping = _dbContext.StatusMappingTable.Select(m => m).Where(x => x.Visible);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(UpdateStatusViewModel input)
        {
            ApplicationDbContext _DbContextForUpdateStatus = new ApplicationDbContext();
            try
            {
                int updatedStatus = input.DeekshaStatus;
                var userDetails = _DbContextForUpdateStatus.DeekshaStatusTable.Find(input.DeekshaId);
                if (userDetails != null)
                {
                    if (userDetails.DeekshaStatus != updatedStatus)
                    {
                        userDetails.DeekshaStatus = updatedStatus;
                        _DbContextForUpdateStatus.SaveChanges();
                        bool isSmsSent = new SmsSenderController().InvokeSystemSms(userDetails.DeekshaId);
                        if (isSmsSent)
                        {
                            return Json(new { Code = 5 }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { Code = 6 }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Code = 3 }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Code = 4 }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { Code = 2 }, JsonRequestBehavior.AllowGet);
            }
        }

        public bool Reseed()
        {
            try
            {
                ApplicationDbContext dbContext = new ApplicationDbContext();
                dbContext.Database.ExecuteSqlCommand("DBCC CHECKIDENT('HomeViewModels', RESEED, 0)");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public ActionResult SmsLogs()
        {
            ApplicationDbContext _DbContextForSmsLogs = new ApplicationDbContext();
            var viewModel = new SmsLogViewModel { ContentItems = _DbContextForSmsLogs.SmsLogTable.Select(m => m).ToList() };
            ViewBag.statusData = _DbContextForSmsLogs.StatusMappingTable.ToList();

            return View(viewModel);
        }


        public void WriteTsv<T>(IEnumerable<T> data, TextWriter output)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in props)
            {
                output.Write(prop.DisplayName); // header
                output.Write("\t");
            }
            output.WriteLine();
            foreach (T item in data)
            {
                foreach (PropertyDescriptor prop in props)
                {
                    output.Write(prop.Converter.ConvertToString(
                        prop.GetValue(item)));
                    output.Write("\t");
                }
                output.WriteLine();
            }
        }


        public void ExportListFromTsvDashboard()
        {
            try
            {
                ApplicationDbContext _DbContextForDownloadExcel = new ApplicationDbContext();
                var data = _DbContextForDownloadExcel.DeekshaStatusTable.Select(m => m).ToList();
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=DeekshaDashboard.xls");
                Response.AddHeader("Content-Type", "application/vnd.ms-excel");
                WriteTsv(data, Response.Output);
                Response.End();
            }
            catch 
            {
                //Ignore
            }
        }

        public void ExportListFromTsvSmsLog()
        {
            try
            {
                ApplicationDbContext _DbContextForDownloadExcel = new ApplicationDbContext();
                var data = _DbContextForDownloadExcel.SmsLogTable.Select(m => m).ToList();
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=DeekshaSmsLogs.xls");
                Response.AddHeader("Content-Type", "application/vnd.ms-excel");
                WriteTsv(data, Response.Output);
                Response.End();

            }
            catch
            {
                //ignore
            }
        }
    }
}