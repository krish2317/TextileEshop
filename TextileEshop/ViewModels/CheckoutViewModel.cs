using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TextileEshop.Models
{
    public class CheckoutViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal TotalAmount { get; set; }

        [Required]
        public string PaymentMethod { get; set; } // "COD" or "Card"

        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }
    }
}
