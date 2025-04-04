using System.ComponentModel.DataAnnotations;

namespace TextileEshop.ViewModels
{
    public class ProfileViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        public string Role { get; set; } // Read-only role
    }
}
