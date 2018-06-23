﻿using System;
using System.Collections.Generic;
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
                    var userDetails = _DbContext.DeekshaStatusTable.Find(input.Phone);
                    if (userDetails == null)
                    {
                        _DbContext.DeekshaStatusTable.Add(input);
                        _DbContext.SaveChanges();
                        bool isSmsSent = new SmsSenderController().InvokeSms(input.Phone);
                        if (isSmsSent)
                        {
                            return Json(new { Code = 0 }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { Code = 7 }, JsonRequestBehavior.AllowGet);
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
            ApplicationDbContext _DbContextForDashboard = new ApplicationDbContext();
            var viewModel =
                new DashboardViewModel { ContentItems = _DbContextForDashboard.DeekshaStatusTable.Select(m => m).ToList() };
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
            ApplicationDbContext _DbContextForUpdateStatus = new ApplicationDbContext();
            try
            {
                int updatedStatus = (int)input.DeekshaStatus;
                var userDetails = _DbContextForUpdateStatus.DeekshaStatusTable.Find(input.Phone);
                if (userDetails != null)
                {
                    if (userDetails.DeekshaStatus != updatedStatus)
                    {
                        userDetails.DeekshaStatus = updatedStatus;
                        _DbContextForUpdateStatus.SaveChanges();
                        bool isSmsSent = new SmsSenderController().InvokeSms(userDetails.Phone);
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

        public ActionResult SmsLogs()
        {
            ApplicationDbContext _DbContextForSmsLogs = new ApplicationDbContext();
            var viewModel = new SmsLogViewModel { ContentItems = _DbContextForSmsLogs.SmsLogTable.Select(m => m).ToList() };
            return View(viewModel);
        }
    }
}