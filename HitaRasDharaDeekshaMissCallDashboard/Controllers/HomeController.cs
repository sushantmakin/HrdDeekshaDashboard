using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
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
                    }
                    else
                    {
                        //return new ContentResult { Content = "<script> swal ('User already exists','You can update the status if required from Update Status page!','error')</script>" };
                        return Json(new { Code = 1},JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
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
        public ActionResult UpdateStatus(UpdateStatusViewModel input)
        {
            var viewModel = new UpdateStatusViewModel();
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
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Users current status is same as new status. Updation not required.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Error","User doesnot exist, Please verify the entered Mobile Number");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return View(viewModel);
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