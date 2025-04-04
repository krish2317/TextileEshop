using Microsoft.EntityFrameworkCore;

namespace TextileEshop.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }  // Cascade Delete allowed

        public int ProductId { get; set; }
        public virtual Product Product { get; set; } // Restrict Delete to prevent conflicts

        public int Quantity { get; set; }

        [Precision(18, 2)]  // Fix decimal precision warning
        public decimal Price { get; set; }
    }
}
