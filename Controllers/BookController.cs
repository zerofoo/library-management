using LibraryManagementSystem.Attributes;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Helpers;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [AuthorizeRole("Librarian")]
    public class BookController : Controller
    {
        private readonly LibraryDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public BookController(LibraryDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index(string? search, string? genre)
        {
            var booksQuery = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                booksQuery = booksQuery.Where(b => 
                    b.Title.Contains(search) || 
                    b.Author.Contains(search) ||
                    b.ISBN!.Contains(search));
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book model, IFormFile? coverImage)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (coverImage != null && coverImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "book-covers");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}_{coverImage.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await coverImage.CopyToAsync(fileStream);
                }

                model.CoverImagePath = $"/images/book-covers/{uniqueFileName}";
            }

            model.DateAdded = DateTime.Now;
            _context.Books.Add(model);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Book added successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Book model, IFormFile? coverImage)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var book = await _context.Books.FindAsync(model.BookId);
            if (book == null)
            {
                return NotFound();
            }

            book.Title = model.Title;
            book.Author = model.Author;
            book.Genre = model.Genre;
            book.ISBN = model.ISBN;
            book.Summary = model.Summary;
            book.PublicationYear = model.PublicationYear;
            book.AvailableCopies = model.AvailableCopies;
            book.TotalCopies = model.TotalCopies;

            if (coverImage != null && coverImage.Length > 0)
            {
                if (!string.IsNullOrEmpty(book.CoverImagePath))
                {
                    var oldImagePath = Path.Combine(_environment.WebRootPath, book.CoverImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "book-covers");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}_{coverImage.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await coverImage.CopyToAsync(fileStream);
                }

                book.CoverImagePath = $"/images/book-covers/{uniqueFileName}";
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Book updated successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.Books
                .Include(b => b.BookRatings)
                    .ThenInclude(r => r.User)
                .Include(b => b.BorrowingTransactions)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            var viewModel = new BookDetailsViewModel
            {
                Book = book,
                AverageRating = book.BookRatings.Any() ? book.BookRatings.Average(r => r.Rating) : 0,
                TotalRatings = book.BookRatings.Count,
                RecentReviews = book.BookRatings.OrderByDescending(r => r.DateSubmitted).Take(5).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books
                .Include(b => b.BorrowingTransactions)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            var activeBorrows = book.BorrowingTransactions.Any(t => t.Status == "Borrowed" || t.Status == "Reserved");
            if (activeBorrows)
            {
                TempData["ErrorMessage"] = "Cannot delete book with active borrowing transactions.";
                return RedirectToAction("Index");
            }

            if (!string.IsNullOrEmpty(book.CoverImagePath))
            {
                var imagePath = Path.Combine(_environment.WebRootPath, book.CoverImagePath.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Book deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
