using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models
{
    public class BorrowingTransaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        [Display(Name = "Borrow Date")]
        [DataType(DataType.DateTime)]
        public DateTime BorrowDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Due Date")]
        [DataType(DataType.DateTime)]
        public DateTime DueDate { get; set; }

        [Display(Name = "Return Date")]
        [DataType(DataType.DateTime)]
        public DateTime? ReturnDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Borrowed"; // Borrowed, Reserved, Returned, Overdue

        [Display(Name = "Renewal Count")]
        public int RenewalCount { get; set; } = 0;

        [Display(Name = "Fine Amount")]
        [DataType(DataType.Currency)]
        public decimal FineAmount { get; set; } = 0;

        [Display(Name = "Fine Paid")]
        public bool FinePaid { get; set; } = false;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [ForeignKey("BookId")]
        public virtual Book? Book { get; set; }

        [Display(Name = "Is Overdue")]
        public bool IsOverdue => Status != "Returned" && DateTime.Now > DueDate;

        [Display(Name = "Days Overdue")]
        public int DaysOverdue => IsOverdue ? (DateTime.Now - DueDate).Days : 0;
    }
}
