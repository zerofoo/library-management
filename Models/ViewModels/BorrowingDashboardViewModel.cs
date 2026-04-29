namespace LibraryManagementSystem.Models.ViewModels
{
    public class BorrowingDashboardViewModel
    {
        public List<BorrowingTransaction> ActiveBorrows { get; set; } = new List<BorrowingTransaction>();
        public List<BorrowingTransaction> OverdueBorrows { get; set; } = new List<BorrowingTransaction>();
        public List<BorrowingTransaction> ReservedBooks { get; set; } = new List<BorrowingTransaction>();
        public decimal TotalFines { get; set; }
        public int BooksOnLoan { get; set; }
        public int MaxBorrowableItems { get; set; }
        public bool CanBorrowMore { get; set; }
    }
}
