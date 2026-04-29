using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models
{
    public class BookRating
    {
        [Key]
        public int RatingId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        [Display(Name = "Rating (1-5)")]
        public int Rating { get; set; }

        [StringLength(1000)]
        [Display(Name = "Review")]
        [DataType(DataType.MultilineText)]
        public string? ReviewText { get; set; }

        [Display(Name = "Date Submitted")]
        [DataType(DataType.DateTime)]
        public DateTime DateSubmitted { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [ForeignKey("BookId")]
        public virtual Book? Book { get; set; }
    }
}
