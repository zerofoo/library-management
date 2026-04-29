using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models
{
    public class BorrowingConfiguration
    {
        [Key]
        public int ConfigId { get; set; }

        [Required]
        public int LibraryId { get; set; }

        [Required]
        [Range(1, 365, ErrorMessage = "Loan duration must be between 1 and 365 days")]
        [Display(Name = "Loan Duration (Days)")]
        public int LoanDurationDays { get; set; } = 14;

        [Required]
        [Range(0, 10, ErrorMessage = "Max renewals must be between 0 and 10")]
        [Display(Name = "Maximum Renewals")]
        public int MaxRenewals { get; set; } = 2;

        [Required]
        [Range(1, 50, ErrorMessage = "Max borrowable items must be between 1 and 50")]
        [Display(Name = "Maximum Borrowable Items")]
        public int MaxBorrowableItems { get; set; } = 5;

        [Required]
        [Range(0, 100, ErrorMessage = "Penalty must be between 0 and 100")]
        [Display(Name = "Overdue Penalty Per Day ($)")]
        [DataType(DataType.Currency)]
        public decimal OverduePenaltyPerDay { get; set; } = 0.50m;

        // Navigation property
        [ForeignKey("LibraryId")]
        public virtual LibraryProfile? LibraryProfile { get; set; }
    }
}
