using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HitaRasDharaDeekshaMissCallDashboard.Models;

namespace HitaRasDharaDeekshaMissCallDashboard.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public static readonly ApplicationDbContext _DbContext = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(HomeViewModel input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userDetails = _DbContext.DeekshaStatusTable.Find(input.Phone);
                    if (userDetails == null)
                    {
                        _DbContext.DeekshaStatusTable.Add(input);
                        _DbContext.SaveChanges();
                        return Json(new { Code = 0 }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { Code = 1 }, JsonRequestBehavior.AllowGet);
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
            var viewModel =
                new DashboardViewModel { ContentItems = _DbContext.DeekshaStatusTable.Select(m => m).ToList() };
            return View(viewModel);
        }

        public ActionResult UpdateStatus()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(UpdateStatusViewModel input)
        {
            try
            {
                int updatedStatus = (int)input.DeekshaStatus;
                var userDetails = _DbContext.DeekshaStatusTable.Find(input.Phone);
                if (userDetails != null)
                {
                    if (userDetails.DeekshaStatus != updatedStatus)
                    {
                        userDetails.DeekshaStatus = updatedStatus;
                        _DbContext.SaveChanges();
                        return Json(new { Code = 5 }, JsonRequestBehavior.AllowGet);
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

        public ActionResult SmsLogs()
        {
            var viewModel = new SmsLogViewModel();
            var smsLogData = new SmsLogData { Phone = "123", Name = "Abc", Status = "Pending", SmsSentStatus = true, Timestamp = new DateTime(2017, 1, 18, 11, 07, 07) };
            var smsLogData2 = new SmsLogData { Phone = "456", Name = "Def", Status = "Passed", SmsSentStatus = false, Timestamp = new DateTime(2017, 1, 18, 11, 07, 07) };
            viewModel.ContentItems = new List<SmsLogData>();
            viewModel.ContentItems.Add(smsLogData);
            viewModel.ContentItems.Add(smsLogData2);
            return View(viewModel);
        }
    }
}