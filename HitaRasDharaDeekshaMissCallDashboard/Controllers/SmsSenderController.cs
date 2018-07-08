using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Helpers;
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

        public bool InvokeSms(string phone)
        {
            ApplicationDbContext _DbContext2 = new ApplicationDbContext();
            string smsContent = "";
            int deekshaStatus = 1;
            string URL =
                "http://sms.wishsolution.com/SecureApi.aspx?usr=HITRAS&pwd=india123&smstype=TextSMS&to={0}&msg={1}&rout=Transactional&from=HITRAS";
            var userDetails = _DbContext2.DeekshaStatusTable.FirstOrDefault(t => t.Phone == phone);
            deekshaStatus = userDetails != null ? userDetails.DeekshaStatus : -1;
            smsContent = userDetails != null ? string.Format(GetMessageFromStatus(deekshaStatus), userDetails.Name) : GetMessageFromStatus(deekshaStatus);
            var urlToHit = string.Format(URL, phone, smsContent);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(urlToHit).Result;  // Blocking call!  
            if (userDetails != null)
            {
                SmsLogData logData = new SmsLogData
                {
                    Name = userDetails.Name,
                    Phone = userDetails.Phone,
                    Status = userDetails.DeekshaStatus,
                    Timestamp = DateTime.Now,
                    SmsSentStatus = true,
                    Id = userDetails.Phone + "#" + userDetails.DeekshaStatus + "_" + userDetails.Name + DateTime.Now,
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
            else
            {
                SmsLogData logData = new SmsLogData
                {
                    Name = "Unknown",
                    Phone = phone,
                    Status = -1,
                    Timestamp = DateTime.Now,
                    SmsSentStatus = true,
                    Id = phone+ "#" + -1 + "_" +DateTime.Now
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
        }

        public string GetMessageFromStatus(int statusId)
        {
            string smsData = "";
            switch (statusId)
            {
                case -1:
                    smsData =
                        "Shree Harivansh, We have not found any entry of this mobile number in our records. Please give a missed call from the number mentioned on your Dikhsa Nivedan form.";
                    break;
                case 0:
                    smsData =
                        "Shree Harivansh {0},We have received your Diksha Nivedan form and you will receive an update on the status of your diksha nivedan via SMS.";
                    break;
                case 1:
                    smsData =
                        "Shree Harivansh {0},We have received your Diksha Nivedan form. Please read Bhagwat Rehasaya of Shree Dongre Ji Maharaj daily for a year and fill a new Diksha Nivedan form , after one year.Bhagwat Rehasaya of Shree Dongre Ji Maharaj is easily available online and at spiritual book stores.";
                    break;
                case 2:
                    smsData =
                        "Shree Harivansh {0},We have received your Diksha Nivedan form.Please come for your diksha on 15th September, 2018, at 10am to Madan Taer, Parikrama Marg, Vrindavan.Location: https://goo.gl/CRBQSi .It is compulsory to read Vrindavan Shat Leela daily from today onwards.To Download Vrindavan Shat Leela click here https://goo.gl/BACQJh";
                    break;
                case 3:
                    smsData =
                        "Shree Harivansh {0},We have received your Diksha Nivedan form. We will inform you of the date of Diksha in Feb 2019. It is compulsory to read Vrindavan Shat leela daily from today onwards.For Downloading Vrindavan Shat leela, click here https://goo.gl/BACQJh";
                    break;
                default:
                    smsData =
                        "Shree Harivansh, We have not found any entry of this mobile number in our records. Please give a missed call from the number mentioned on your Dikhsa Nivedan form.";
                    break;
            }
            return smsData;
        }

        public bool MissCall()
        {
            string mobile = Request.QueryString["from"];
            if (!mobile.IsNullOrWhiteSpace())
            {
                return InvokeSms(mobile);
            }
            return false;
        }
    }
}