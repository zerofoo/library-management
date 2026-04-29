using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        [Display(Name = "Book Title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author is required")]
        [StringLength(100)]
        [Display(Name = "Author Name")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Genre is required")]
        [StringLength(50)]
        public string Genre { get; set; } = string.Empty;

        [RegularExpression(@"^\d{3}-\d{10}$|^\d{13}$", ErrorMessage = "Invalid ISBN format (e.g., 978-1234567890 or 9781234567890)")]
        [StringLength(17)]
        public string? ISBN { get; set; }

        [StringLength(2000)]
        [Display(Name = "Book Summary")]
        [DataType(DataType.MultilineText)]
        public string? Summary { get; set; }

        [Display(Name = "Publication Year")]
        [Range(1000, 2100, ErrorMessage = "Publication year must be between 1000 and 2100")]
        public int? PublicationYear { get; set; }

        [StringLength(500)]
        [Display(Name = "Cover Image")]
        public string? CoverImagePath { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Available copies must be non-negative")]
        [Display(Name = "Available Copies")]
        public int AvailableCopies { get; set; } = 0;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Total copies must be at least 1")]
        [Display(Name = "Total Copies")]
        public int TotalCopies { get; set; } = 1;

        [Display(Name = "Date Added")]
        [DataType(DataType.DateTime)]
        public DateTime DateAdded { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<BorrowingTransaction> BorrowingTransactions { get; set; } = new List<BorrowingTransaction>();
        public virtual ICollection<BookRating> BookRatings { get; set; } = new List<BookRating>();

        [Display(Name = "Average Rating")]
        public double AverageRating => BookRatings.Any() ? BookRatings.Average(r => r.Rating) : 0;

        [Display(Name = "Is Available")]
        public bool IsAvailable => AvailableCopies > 0;
    }
}
