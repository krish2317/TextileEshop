namespace TextileEshop.Models
{
    public class AdminLog
    {
        public int Id { get; set; }
        public string AdminId { get; set; }
        public ApplicationUser Admin { get; set; }
        public string Action { get; set; }
        public DateTime ActionDate { get; set; }
    }

}
