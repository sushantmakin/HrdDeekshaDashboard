using System.ComponentModel.DataAnnotations;
using System.Web.ModelBinding;

namespace HitaRasDharaDeekshaMissCallDashboard.Models
{
    public class HomeViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Phone Number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "You must provide a phone number")]
        [MaxLength(10)]
        [Key]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Not a valid phone number")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Deeksha Status")]
        public int DeekshaStatus { get; set; }
    }
}