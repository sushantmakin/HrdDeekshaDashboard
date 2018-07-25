using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HitaRasDharaDeekshaMissCallDashboard.Models
{
    public class UpdateStatusViewModel
    {
        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Phone Number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "You must provide a phone number")]
        [MaxLength(10)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Not a valid phone number")]
        public string Phone { get; set; }
        [Required]
        [Display(Name = "Deeksha Status")]
        public Status DeekshaStatus { get; set; }
    }

    public enum Status
    {
        [Description("Pending")]
        Pending,

        [Display(Name = "Bhagwat Rahesya for 1 year")]
        BhagwatRahesya,

        [Display(Name = "Deeksha on 15th September 2018")]
        Deeksha15Sep,

        [Display(Name = "Deeksha in March 2019 (Holi)")]
        DeekshaMarch2019,

        [Display(Name = "Deeksha on 16th September 2018")]
        Deeksha16Sep
    }
}