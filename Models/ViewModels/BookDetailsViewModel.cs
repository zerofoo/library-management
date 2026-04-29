namespace LibraryManagementSystem.Models.ViewModels
{
    public class BookDetailsViewModel
    {
        public Book Book { get; set; } = new Book();
        public double AverageRating { get; set; }
        public int TotalRatings { get; set; }
        public List<BookRating> RecentReviews { get; set; } = new List<BookRating>();
        public bool UserHasBorrowed { get; set; }
        public bool UserCanBorrow { get; set; }
        public int UserCurrentBorrowCount { get; set; }
    }
}
