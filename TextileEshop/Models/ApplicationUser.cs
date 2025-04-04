using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TextileEshop.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(250)]
        public string? Address { get; set; }

        // User Role (Admin, Seller, Buyer)
        [Required]
        [StringLength(20)]
        public string Role { get; set; }

        [Required]
        [StringLength(50)]
        public override string UserName { get; set; }

        [Required]
        [EmailAddress]
        public override string Email { get; set; }

    }
}
