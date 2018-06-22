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
        Pending,
        Path1,
        Path2,
        Granted
    }
}