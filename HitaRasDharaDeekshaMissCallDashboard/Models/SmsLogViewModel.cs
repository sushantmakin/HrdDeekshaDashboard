using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HitaRasDharaDeekshaMissCallDashboard.Models
{
    public class SmsLogViewModel
    {

        public List<SmsLogData> ContentItems { get; set; }
    }

    public class SmsLogData
    {

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [Key]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; }

        [Required]
        [Display(Name = "Timestamp")]
        public DateTime Timestamp { get; set; }

        [Required]
        [Display(Name = "SMS sent status")]
        public Boolean SmsSentStatus { get; set; }
    }
}