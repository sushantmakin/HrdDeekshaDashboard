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
                "http://sms.wishsolution.com/SecureApi.aspx?usr=HITRAS&key=A8B33272-A7A1-45C9-8782-3C3F3E032668&smstype=TextSMS&to={0}&msg={1}&rout=Transactional&from=HITRAS";
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
                        "Shree Harivansh, We could not find any entry with this mobile number in our records. Kindly give a misscall with the number registered in the Deekhsa Nivedan Form.";
                    break;
                case 0:
                    smsData =
                        "Shree Harivansh {0},We have received your Diksha Nivedan Form and your file is saved succesfully. You will recieve an SMS with the update as and when it is replied upon by Maharaj Shree.";
                    break;
                case 1:
                    smsData =
                        "Shree Harivansh {0},We have received your Diksha Nivedan Form.Please read Bhagwat Rehasaya of Shree Dongre Ji Maharaj daily for a year and fill your Diksha Nivedan form again, after one year.Bhagwat Rehasaya of Shree Dongre Ji Maharaj is easily available at Spritual Book Stores.";
                    break;
                case 2:
                    smsData =
                        "Shree Harivansh {0},We have received your Diksha Nivedan Form.Please do the Vrindavan Shat leela(reading) daily, from now onwards and please reach for your diksha on 15 September, 2018, morning 10 o'clock, at Madan Taer, Parikrama Marg, Vrindavan.Location: https://goo.gl/CRBQSi .For Downloading Vrindavan Shat leela, visit https://goo.gl/BACQJh";
                    break;
                case 3:
                    smsData =
                        "Shree Harivansh {0},We have received your Diksha Nivedan Form.Please do the Vrindavan Shat leela daily, from now onwards till next year, Holi Mahotsava Katha in March 2019, and we will inform you of your Diksha Dates in February 2019.For Downloading Vrindavan Shat leela, kindly visit: https://goo.gl/BACQJh";
                    break;
                default:
                    smsData =
                        "Shree Harivansh, We could not find any entry with this mobile number in our records. Kindly give a misscall with the number registered in the Deekhsa Nivedan Form.";
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