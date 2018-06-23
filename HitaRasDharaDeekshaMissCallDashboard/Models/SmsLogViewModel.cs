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
        [Display(Name = "Id")]
        [Key]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int Status { get; set; }

        [Required]
        [Display(Name = "Timestamp")]
        public DateTime Timestamp { get; set; }

        [Required]
        [Display(Name = "SMS sent status")]
        public Boolean SmsSentStatus { get; set; }

    }
}