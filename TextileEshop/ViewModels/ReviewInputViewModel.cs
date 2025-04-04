using System.ComponentModel.DataAnnotations;

namespace TextileEshop.ViewModels
{
    public class ReviewInputViewModel
    {
        [Required]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Comment is required.")]
        [StringLength(500, ErrorMessage = "Comment is too long.")]
        public string Comment { get; set; }
    }
}
