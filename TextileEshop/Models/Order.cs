namespace TextileEshop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public ApplicationUser Buyer { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } // Pending, Shipped, Delivered, Cancelled
        public ICollection<OrderItem> OrderItems { get; set; }
        public Payment Payment { get; set; }
    }

}
