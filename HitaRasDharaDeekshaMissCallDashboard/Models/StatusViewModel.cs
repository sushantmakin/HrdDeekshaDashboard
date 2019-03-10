using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HitaRasDharaDeekshaMissCallDashboard.Models
{
    public class StatusViewModel
    {
        [Key]
        [Required]
        [Display(Name = "StatusId")]
        public int StatusId { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; }

        [Required]
        [Display(Name = "Visibility")]
        public bool Visible { get; set; }

        [Required]
        [Display(Name = "SMSMessage")]
        public string SmsMessage { get; set; }

    }
}