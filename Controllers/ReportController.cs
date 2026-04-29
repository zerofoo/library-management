using LibraryManagementSystem.Attributes;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [AuthorizeRole("Librarian")]
    public class ReportController : Controller
    {
        private readonly LibraryDbContext _context;

        public ReportController(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new ReportViewModel
            {
                TotalBooks = await _context.Books.CountAsync(),
                TotalMembers = await _context.Users.CountAsync(u => u.Role == "Member"),
                ActiveBorrows = await _context.BorrowingTransactions.CountAsync(t => t.Status == "Borrowed" || t.Status == "Overdue"),
                TotalFinesCollected = await _context.BorrowingTransactions.Where(t => t.FinePaid).SumAsync(t => t.FineAmount)
            };

            var sixMonthsAgo = DateTime.Now.AddMonths(-6);
            viewModel.BorrowingTrends = await _context.BorrowingTransactions
                .Where(t => t.BorrowDate >= sixMonthsAgo)
                .GroupBy(t => new { t.BorrowDate.Year, t.BorrowDate.Month })
                .Select(g => new MonthlyBorrowingData
                {
                    Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                    BorrowCount = g.Count()
                })
                .OrderBy(m => m.Month)
                .ToListAsync();

            viewModel.OverdueBooks = await _context.BorrowingTransactions
                .Include(t => t.User)
                .Include(t => t.Book)
                .Where(t => t.Status == "Overdue" || (t.Status == "Borrowed" && t.DueDate < DateTime.Now))
                .OrderBy(t => t.DueDate)
                .ToListAsync();

            viewModel.MostActiveMembers = await _context.BorrowingTransactions
                .Include(t => t.User)
                .GroupBy(t => new { t.UserId, t.User!.FirstName, t.User.LastName, t.User.Email })
                .Select(g => new ActiveMemberData
                {
                    MemberName = $"{g.Key.FirstName} {g.Key.LastName}",
                    Email = g.Key.Email,
                    BorrowCount = g.Count()
                })
                .OrderByDescending(m => m.BorrowCount)
                .Take(10)
                .ToListAsync();

            viewModel.MostPopularBooks = await _context.BorrowingTransactions
                .Include(t => t.Book)
                    .ThenInclude(b => b!.BookRatings)
                .GroupBy(t => new { t.BookId, t.Book!.Title, t.Book.Author })
                .Select(g => new PopularBookData
                {
                    BookTitle = g.Key.Title,
                    Author = g.Key.Author,
                    BorrowCount = g.Count(),
                    AverageRating = g.First().Book!.BookRatings.Any() ? g.First().Book.BookRatings.Average(r => r.Rating) : 0
                })
                .OrderByDescending(b => b.BorrowCount)
                .Take(10)
                .ToListAsync();

            return View(viewModel);
        }
    }
}
