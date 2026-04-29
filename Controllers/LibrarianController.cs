using LibraryManagementSystem.Attributes;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Helpers;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [AuthorizeRole("Librarian")]
    public class LibrarianController : Controller
    {
        private readonly LibraryDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public LibrarianController(LibraryDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Dashboard()
        {
            var stats = new
            {
                TotalBooks = await _context.Books.CountAsync(),
                TotalMembers = await _context.Users.CountAsync(u => u.Role == "Member"),
                ActiveBorrows = await _context.BorrowingTransactions.CountAsync(t => t.Status == "Borrowed"),
                OverdueBooks = await _context.BorrowingTransactions.CountAsync(t => t.Status == "Overdue" || (t.Status == "Borrowed" && t.DueDate < DateTime.Now))
            };

            ViewBag.Stats = stats;
            return View();
        }

        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.Session.GetUserId();
            var profile = await _context.LibraryProfiles
                .Include(p => p.Configuration)
                .FirstOrDefaultAsync(p => p.CreatedByUserId == userId);

            return View(profile);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProfile()
        {
            var userId = HttpContext.Session.GetUserId();
            var existingProfile = await _context.LibraryProfiles
                .FirstOrDefaultAsync(p => p.CreatedByUserId == userId);

            if (existingProfile != null)
            {
                TempData["ErrorMessage"] = "You already have a library profile.";
                return RedirectToAction("Profile");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProfile(LibraryProfile model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = HttpContext.Session.GetUserId();
            model.CreatedByUserId = userId.Value;
            model.CreatedDate = DateTime.Now;

            _context.LibraryProfiles.Add(model);
            await _context.SaveChangesAsync();

            var config = new BorrowingConfiguration
            {
                LibraryId = model.LibraryId,
                LoanDurationDays = 14,
                MaxRenewals = 2,
                MaxBorrowableItems = 5,
                OverduePenaltyPerDay = 0.50m
            };

            _context.BorrowingConfigurations.Add(config);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Library profile created successfully!";
            return RedirectToAction("Profile");
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile(int id)
        {
            var profile = await _context.LibraryProfiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetUserId();
            if (profile.CreatedByUserId != userId)
            {
                return Forbid();
            }

            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(LibraryProfile model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = HttpContext.Session.GetUserId();
            if (model.CreatedByUserId != userId)
            {
                return Forbid();
            }

            _context.LibraryProfiles.Update(model);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Library profile updated successfully!";
            return RedirectToAction("Profile");
        }

        [HttpGet]
        public async Task<IActionResult> EditConfiguration(int id)
        {
            var config = await _context.BorrowingConfigurations
                .Include(c => c.LibraryProfile)
                .FirstOrDefaultAsync(c => c.LibraryId == id);

            if (config == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetUserId();
            if (config.LibraryProfile?.CreatedByUserId != userId)
            {
                return Forbid();
            }

            return View(config);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfiguration(BorrowingConfiguration model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _context.BorrowingConfigurations.Update(model);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Borrowing configuration updated successfully!";
            return RedirectToAction("Profile");
        }
    }
}
