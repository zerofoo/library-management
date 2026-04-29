namespace LibraryManagementSystem.Models.ViewModels
{
    public class ReportViewModel
    {
        public List<MonthlyBorrowingData> BorrowingTrends { get; set; } = new List<MonthlyBorrowingData>();
        public List<BorrowingTransaction> OverdueBooks { get; set; } = new List<BorrowingTransaction>();
        public List<ActiveMemberData> MostActiveMembers { get; set; } = new List<ActiveMemberData>();
        public List<PopularBookData> MostPopularBooks { get; set; } = new List<PopularBookData>();
        public int TotalBooks { get; set; }
        public int TotalMembers { get; set; }
        public int ActiveBorrows { get; set; }
        public decimal TotalFinesCollected { get; set; }
    }

    public class MonthlyBorrowingData
    {
        public string Month { get; set; } = string.Empty;
        public int BorrowCount { get; set; }
    }

    public class ActiveMemberData
    {
        public string MemberName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int BorrowCount { get; set; }
    }

    public class PopularBookData
    {
        public string BookTitle { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int BorrowCount { get; set; }
        public double AverageRating { get; set; }
    }
}
