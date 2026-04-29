using LibraryManagementSystem.Attributes;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Helpers;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    public class BorrowingController : Controller
    {
        private readonly LibraryDbContext _context;

        public BorrowingController(LibraryDbContext context)
        {
            _context = context;
        }

        [AuthorizeRole("Member")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BorrowBook(int bookId)
        {
            var userId = HttpContext.Session.GetUserId();
            var config = await _context.BorrowingConfigurations.FirstOrDefaultAsync();

            if (config == null)
            {
                TempData["ErrorMessage"] = "Borrowing configuration not set up. Please contact the librarian.";
                return RedirectToAction("Browse", "Member");
            }

            var currentBorrowCount = await _context.BorrowingTransactions
                .CountAsync(t => t.UserId == userId && (t.Status == "Borrowed" || t.Status == "Reserved" || t.Status == "Overdue"));

            if (currentBorrowCount >= config.MaxBorrowableItems)
            {
                TempData["ErrorMessage"] = $"You have reached the maximum borrowing limit of {config.MaxBorrowableItems} items.";
                return RedirectToAction("BookDetails", "Member", new { id = bookId });
            }

            var hasUnpaidFines = await _context.BorrowingTransactions
                .AnyAsync(t => t.UserId == userId && t.FineAmount > 0 && !t.FinePaid);

            if (hasUnpaidFines)
            {
                TempData["ErrorMessage"] = "You have unpaid fines. Please clear them before borrowing more books.";
                return RedirectToAction("Dashboard", "Member");
            }

            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                return NotFound();
            }

            var existingBorrow = await _context.BorrowingTransactions
                .AnyAsync(t => t.UserId == userId && t.BookId == bookId && t.Status != "Returned");

            if (existingBorrow)
            {
                TempData["ErrorMessage"] = "You have already borrowed or reserved this book.";
                return RedirectToAction("BookDetails", "Member", new { id = bookId });
            }

            var transaction = new BorrowingTransaction
            {
                UserId = userId.Value,
                BookId = bookId,
                BorrowDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(config.LoanDurationDays),
                Status = book.AvailableCopies > 0 ? "Borrowed" : "Reserved"
            };

            if (book.AvailableCopies > 0)
            {
                book.AvailableCopies--;
            }

            _context.BorrowingTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = transaction.Status == "Borrowed" 
                ? $"Book borrowed successfully! Due date: {transaction.DueDate:MMMM dd, yyyy}" 
                : "Book reserved successfully! We'll notify you when it becomes available.";

            return RedirectToAction("Dashboard", "Member");
        }

        [AuthorizeRole("Member")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RenewBook(int transactionId)
        {
            var userId = HttpContext.Session.GetUserId();
            var transaction = await _context.BorrowingTransactions
                .Include(t => t.Book)
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId && t.UserId == userId);

            if (transaction == null)
            {
                return NotFound();
            }

            var config = await _context.BorrowingConfigurations.FirstOrDefaultAsync();
            if (config == null)
            {
                TempData["ErrorMessage"] = "Borrowing configuration not found.";
                return RedirectToAction("Dashboard", "Member");
            }

            if (transaction.Status == "Overdue" || transaction.IsOverdue)
            {
                TempData["ErrorMessage"] = "Cannot renew overdue books. Please return the book and pay any fines.";
                return RedirectToAction("Dashboard", "Member");
            }

            if (transaction.RenewalCount >= config.MaxRenewals)
            {
                TempData["ErrorMessage"] = $"You have reached the maximum renewal limit of {config.MaxRenewals} times.";
                return RedirectToAction("Dashboard", "Member");
            }

            transaction.RenewalCount++;
            transaction.DueDate = transaction.DueDate.AddDays(config.LoanDurationDays);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Book renewed successfully! New due date: {transaction.DueDate:MMMM dd, yyyy}";
            return RedirectToAction("Dashboard", "Member");
        }

        [AuthorizeRole("Librarian")]
        public async Task<IActionResult> ManageTransactions(string? status)
        {
            var transactionsQuery = _context.BorrowingTransactions
                .Include(t => t.User)
                .Include(t => t.Book)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                transactionsQuery = transactionsQuery.Where(t => t.Status == status);
                ViewBag.Status = status;
            }

            var transactions = await transactionsQuery
                .OrderByDescending(t => t.BorrowDate)
                .ToListAsync();

            var config = await _context.BorrowingConfigurations.FirstOrDefaultAsync();
            foreach (var transaction in transactions.Where(t => t.Status == "Borrowed" && t.IsOverdue))
            {
                transaction.Status = "Overdue";
                if (config != null)
                {
                    transaction.FineAmount = transaction.DaysOverdue * config.OverduePenaltyPerDay;
                }
            }

            await _context.SaveChangesAsync();

            return View(transactions);
        }

        [AuthorizeRole("Librarian")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsReturned(int transactionId)
        {
            var transaction = await _context.BorrowingTransactions
                .Include(t => t.Book)
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId);

            if (transaction == null)
            {
                return NotFound();
            }

            transaction.Status = "Returned";
            transaction.ReturnDate = DateTime.Now;

            if (transaction.Book != null)
            {
                transaction.Book.AvailableCopies++;
            }

            var config = await _context.BorrowingConfigurations.FirstOrDefaultAsync();
            if (transaction.ReturnDate > transaction.DueDate && config != null)
            {
                var daysLate = (transaction.ReturnDate.Value - transaction.DueDate).Days;
                transaction.FineAmount = daysLate * config.OverduePenaltyPerDay;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = transaction.FineAmount > 0 
                ? $"Book returned. Fine amount: ${transaction.FineAmount:F2}" 
                : "Book returned successfully!";

            return RedirectToAction("ManageTransactions");
        }

        [AuthorizeRole("Librarian")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkFinePaid(int transactionId)
        {
            var transaction = await _context.BorrowingTransactions.FindAsync(transactionId);
            if (transaction == null)
            {
                return NotFound();
            }

            transaction.FinePaid = true;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Fine marked as paid.";
            return RedirectToAction("ManageTransactions");
        }
    }
}
