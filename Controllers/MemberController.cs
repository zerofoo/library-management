using LibraryManagementSystem.Attributes;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Helpers;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [AuthorizeRole("Member")]
    public class MemberController : Controller
    {
        private readonly LibraryDbContext _context;

        public MemberController(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userId = HttpContext.Session.GetUserId();
            var config = await _context.BorrowingConfigurations.FirstOrDefaultAsync();

            var activeBorrows = await _context.BorrowingTransactions
                .Include(t => t.Book)
                .Where(t => t.UserId == userId && (t.Status == "Borrowed" || t.Status == "Reserved"))
                .ToListAsync();

            foreach (var borrow in activeBorrows.Where(b => b.Status == "Borrowed" && b.IsOverdue))
            {
                borrow.Status = "Overdue";
                if (config != null)
                {
                    borrow.FineAmount = borrow.DaysOverdue * config.OverduePenaltyPerDay;
                }
            }

            await _context.SaveChangesAsync();

            var viewModel = new BorrowingDashboardViewModel
            {
                ActiveBorrows = activeBorrows.Where(t => t.Status == "Borrowed").ToList(),
                OverdueBorrows = activeBorrows.Where(t => t.Status == "Overdue").ToList(),
                ReservedBooks = activeBorrows.Where(t => t.Status == "Reserved").ToList(),
                TotalFines = activeBorrows.Where(t => !t.FinePaid).Sum(t => t.FineAmount),
                BooksOnLoan = activeBorrows.Count(t => t.Status == "Borrowed" || t.Status == "Overdue"),
                MaxBorrowableItems = config?.MaxBorrowableItems ?? 5,
                CanBorrowMore = activeBorrows.Count < (config?.MaxBorrowableItems ?? 5)
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Browse(string? search, string? genre)
        {
            var booksQuery = _context.Books
                .Include(b => b.BookRatings)
                .Where(b => b.AvailableCopies > 0)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                booksQuery = booksQuery.Where(b =>
                    b.Title.Contains(search) ||
                    b.Author.Contains(search) ||
                    b.Genre.Contains(search));
                ViewBag.Search = search;
            }

            if (!string.IsNullOrEmpty(genre))
            {
                booksQuery = booksQuery.Where(b => b.Genre == genre);
                ViewBag.Genre = genre;
            }

            var books = await booksQuery.OrderByDescending(b => b.DateAdded).ToListAsync();
            var genres = await _context.Books.Select(b => b.Genre).Distinct().ToListAsync();
            ViewBag.Genres = genres;

            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> BookDetails(int id)
        {
            var userId = HttpContext.Session.GetUserId();
            var book = await _context.Books
                .Include(b => b.BookRatings)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            var userHasBorrowed = await _context.BorrowingTransactions
                .AnyAsync(t => t.UserId == userId && t.BookId == id && t.Status != "Returned");

            var config = await _context.BorrowingConfigurations.FirstOrDefaultAsync();
            var currentBorrowCount = await _context.BorrowingTransactions
                .CountAsync(t => t.UserId == userId && (t.Status == "Borrowed" || t.Status == "Reserved" || t.Status == "Overdue"));

            var hasUnpaidFines = await _context.BorrowingTransactions
                .AnyAsync(t => t.UserId == userId && t.FineAmount > 0 && !t.FinePaid);

            var viewModel = new BookDetailsViewModel
            {
                Book = book,
                AverageRating = book.BookRatings.Any() ? book.BookRatings.Average(r => r.Rating) : 0,
                TotalRatings = book.BookRatings.Count,
                RecentReviews = book.BookRatings.OrderByDescending(r => r.DateSubmitted).Take(5).ToList(),
                UserHasBorrowed = userHasBorrowed,
                UserCanBorrow = !userHasBorrowed && 
                               currentBorrowCount < (config?.MaxBorrowableItems ?? 5) && 
                               !hasUnpaidFines &&
                               book.AvailableCopies > 0,
                UserCurrentBorrowCount = currentBorrowCount
            };

            return View(viewModel);
        }

        public async Task<IActionResult> History()
        {
            var userId = HttpContext.Session.GetUserId();
            var history = await _context.BorrowingTransactions
                .Include(t => t.Book)
                .Where(t => t.UserId == userId && t.Status == "Returned")
                .OrderByDescending(t => t.ReturnDate)
                .ToListAsync();

            return View(history);
        }

        [HttpGet]
        public async Task<IActionResult> RateBook(int transactionId)
        {
            var userId = HttpContext.Session.GetUserId();
            var transaction = await _context.BorrowingTransactions
                .Include(t => t.Book)
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId && t.UserId == userId);

            if (transaction == null || transaction.Status != "Returned")
            {
                TempData["ErrorMessage"] = "You can only rate books you have returned.";
                return RedirectToAction("History");
            }

            var existingRating = await _context.BookRatings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.BookId == transaction.BookId);

            if (existingRating != null)
            {
                return View(existingRating);
            }

            var newRating = new BookRating
            {
                BookId = transaction.BookId,
                UserId = userId.Value
            };

            ViewBag.BookTitle = transaction.Book?.Title;
            return View(newRating);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RateBook(BookRating model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = HttpContext.Session.GetUserId();
            var existingRating = await _context.BookRatings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.BookId == model.BookId);

            if (existingRating != null)
            {
                existingRating.Rating = model.Rating;
                existingRating.ReviewText = model.ReviewText;
                existingRating.DateSubmitted = DateTime.Now;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Your rating has been updated!";
            }
            else
            {
                model.UserId = userId.Value;
                model.DateSubmitted = DateTime.Now;
                _context.BookRatings.Add(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thank you for your rating!";
            }

            return RedirectToAction("History");
        }
    }
}
