using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HitaRasDharaDeekshaMissCallDashboard.Models
{
    public class CMSViewModel
    {
        [Key]
        [Required]
        [Display(Name = "Key")]
        public string Key { get; set; }

        [Required]
        [Display(Name = "Value")]
        public string Value { get; set; }
    }
}