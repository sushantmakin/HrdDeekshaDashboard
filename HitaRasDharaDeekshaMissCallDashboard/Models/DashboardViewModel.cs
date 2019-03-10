using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HitaRasDharaDeekshaMissCallDashboard.Models
{
    public class DashboardViewModel
    {
        public List<HomeViewModel> ContentItems { get; set; }
    }

    //public class DashboardData
    //{

    //    [Required]
    //    [Display(Name = "Name")]
    //    public string Name { get; set; }

    //    [Required]
    //    [Display(Name = "Phone Number")]
    //    public string Phone { get; set; }

    //    [Required]
    //    [Display(Name = "Status")]
    //    public string Status { get; set; }
    //}
}