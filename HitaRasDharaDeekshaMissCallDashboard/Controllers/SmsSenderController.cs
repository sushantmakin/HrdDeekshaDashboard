using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Mvc;
using HitaRasDharaDeekshaMissCallDashboard.Models;
using Microsoft.Ajax.Utilities;

namespace HitaRasDharaDeekshaMissCallDashboard.Controllers
{
    public interface ISmsSenderService
    {
        bool InvokeSms(string phone);
    }
    public class SmsSenderController : Controller
    {

        public bool InvokeSystemSms(int DeekshaId)
        {
            ApplicationDbContext _DbContext2 = new ApplicationDbContext();
            string smsContent;
            int deekshaStatus;
            string URL = _DbContext2.CMSDataTable.Find("SmsApiUrl").Value;
            var userDetails = _DbContext2.DeekshaStatusTable.FirstOrDefault(t => t.DeekshaId == DeekshaId);
            deekshaStatus = userDetails?.DeekshaStatus ?? -1;
            smsContent = userDetails != null ? string.Format(_DbContext2.StatusMappingTable.Find(deekshaStatus).SmsMessage, userDetails.Name) : _DbContext2.StatusMappingTable.Find(deekshaStatus).SmsMessage;
            var urlToHit = string.Format(URL, userDetails.Phone, smsContent);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(urlToHit).Result;  // Blocking call!  
                SmsLogData logData = new SmsLogData
                {
                    Name = userDetails.Name,
                    Phone = userDetails.Phone,
                    Status = userDetails.DeekshaStatus,
                    Timestamp = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                        TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
                    SmsSentStatus = true,
                    Id = userDetails.Phone + "#" + userDetails.DeekshaStatus + "_" + userDetails.Name + TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                             TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"))
                };
                if (response.IsSuccessStatusCode)
                {
                    _DbContext2.SmsLogTable.Add(logData);
                    _DbContext2.SaveChanges();
                    return true;
                }
                logData.SmsSentStatus = false;
                _DbContext2.SmsLogTable.Add(logData);
                _DbContext2.SaveChanges();
                return false;
        }

        public bool InvokeMissCallSms(string mobileNumber)
        {
            ApplicationDbContext _DbContext2 = new ApplicationDbContext();
            string smsContent;
            string URL = _DbContext2.CMSDataTable.Find("SmsApiUrl").Value;
            var usersWithProvidedMobileNumber = _DbContext2.DeekshaStatusTable.Select(x => x).Where(t => t.Phone.Equals(mobileNumber)).ToList();
            if (usersWithProvidedMobileNumber.Count == 0)
            {
                smsContent = _DbContext2.CMSDataTable.Find("UnknownUser").Value;
            }
            else if (usersWithProvidedMobileNumber.Count == 1)
            {
                HomeViewModel userDetails = usersWithProvidedMobileNumber[0];
                smsContent = string.Format(_DbContext2.StatusMappingTable.Find(userDetails.DeekshaStatus).SmsMessage,
                        userDetails.Name);
            }
            else
            {
                StringBuilder sb = new StringBuilder(string.Format(_DbContext2.CMSDataTable.Find("MultiSmsHeader").Value, usersWithProvidedMobileNumber.Count) + Environment.NewLine + Environment.NewLine);
                int index = 1;
                foreach (var item in usersWithProvidedMobileNumber)
                {
                    var specificSms = index+". "+ string.Format(_DbContext2.StatusMappingTable.Find(item.DeekshaStatus).SmsMessage,
                        item.Name);
                    sb.Append(specificSms);
                    if (index != usersWithProvidedMobileNumber.Count)
                    {
                        sb.Append(Environment.NewLine);
                        sb.Append(Environment.NewLine);
                    }
                    index++;
                }

                smsContent = sb.ToString();
            }

            var urlToHit = string.Format(URL, mobileNumber, smsContent);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(urlToHit).Result;  // Blocking call!  
            //SmsLogData logData = new SmsLogData
            //{
            //    Name = userDetails.Name,
            //    Phone = userDetails.Phone,
            //    Status = userDetails.DeekshaStatus,
            //    Timestamp = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
            //        TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
            //    SmsSentStatus = true,
            //    Id = userDetails.Phone + "#" + userDetails.DeekshaStatus + "_" + userDetails.Name + TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
            //             TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"))
            //};
            if (response.IsSuccessStatusCode)
            {
                //_DbContext2.SmsLogTable.Add(logData);
                //_DbContext2.SaveChanges();
                return true;
            }
            //logData.SmsSentStatus = false;
            //_DbContext2.SmsLogTable.Add(logData);
            //_DbContext2.SaveChanges();
            return false;
        }

        public bool MissCall()
        {
            ApplicationDbContext _dbContext = new ApplicationDbContext();
            string queryParameter = _dbContext.CMSDataTable.Find("SmsQueryParameter").Value;
            string mobile = Request.QueryString[queryParameter];
            string reducedMobile = mobile.Substring(mobile.Length - 10);
            if (!mobile.IsNullOrWhiteSpace())
            {
                return InvokeMissCallSms(reducedMobile);
            }
            return false;
        }
    }
}