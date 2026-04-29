using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<LibraryProfile> LibraryProfiles { get; set; }
        public DbSet<BorrowingConfiguration> BorrowingConfigurations { get; set; }
        public DbSet<BorrowingTransaction> BorrowingTransactions { get; set; }
        public DbSet<BookRating> BookRatings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User → BorrowingTransaction relationship
            modelBuilder.Entity<BorrowingTransaction>()
                .HasOne(bt => bt.User)
                .WithMany(u => u.BorrowingTransactions)
                .HasForeignKey(bt => bt.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Book → BorrowingTransaction relationship
            modelBuilder.Entity<BorrowingTransaction>()
                .HasOne(bt => bt.Book)
                .WithMany(b => b.BorrowingTransactions)
                .HasForeignKey(bt => bt.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure User → BookRating relationship
            modelBuilder.Entity<BookRating>()
                .HasOne(br => br.User)
                .WithMany(u => u.BookRatings)
                .HasForeignKey(br => br.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Book → BookRating relationship
            modelBuilder.Entity<BookRating>()
                .HasOne(br => br.Book)
                .WithMany(b => b.BookRatings)
                .HasForeignKey(br => br.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure LibraryProfile ↔ BorrowingConfiguration (One-to-One)
            modelBuilder.Entity<LibraryProfile>()
                .HasOne(lp => lp.Configuration)
                .WithOne(bc => bc.LibraryProfile)
                .HasForeignKey<BorrowingConfiguration>(bc => bc.LibraryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure User → LibraryProfile relationship
            modelBuilder.Entity<LibraryProfile>()
                .HasOne(lp => lp.CreatedByUser)
                .WithOne(u => u.LibraryProfile)
                .HasForeignKey<LibraryProfile>(lp => lp.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2024, 1, 1);
            
            // Seed Librarian User (UserId = 1)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Email = "librarian@library.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Librarian123"),
                    Role = "Librarian",
                    FirstName = "Admin",
                    LastName = "Librarian",
                    PhoneNumber = "555-0100",
                    RegistrationDate = seedDate,
                    IsActive = true
                }
            );

            // Seed Member Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 2,
                    Email = "john.doe@email.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Member123"),
                    Role = "Member",
                    FirstName = "John",
                    LastName = "Doe",
                    PhoneNumber = "555-0101",
                    RegistrationDate = seedDate,
                    IsActive = true
                },
                new User
                {
                    UserId = 3,
                    Email = "jane.smith@email.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Member123"),
                    Role = "Member",
                    FirstName = "Jane",
                    LastName = "Smith",
                    PhoneNumber = "555-0102",
                    RegistrationDate = seedDate,
                    IsActive = true
                }
            );

            // Seed Library Profile
            modelBuilder.Entity<LibraryProfile>().HasData(
                new LibraryProfile
                {
                    LibraryId = 1,
                    LibraryName = "University Central Library",
                    Location = "123 Campus Drive, Education City",
                    OperatingHours = "Mon-Fri: 8:00 AM - 8:00 PM, Sat-Sun: 10:00 AM - 6:00 PM",
                    ContactEmail = "info@universitylibrary.edu",
                    ContactPhone = "555-0200",
                    CreatedByUserId = 1,
                    CreatedDate = seedDate
                }
            );

            // Seed Borrowing Configuration
            modelBuilder.Entity<BorrowingConfiguration>().HasData(
                new BorrowingConfiguration
                {
                    ConfigId = 1,
                    LibraryId = 1,
                    LoanDurationDays = 14,
                    MaxRenewals = 2,
                    MaxBorrowableItems = 5,
                    OverduePenaltyPerDay = 0.50m
                }
            );

            // Seed Books
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    BookId = 1,
                    Title = "Clean Code: A Handbook of Agile Software Craftsmanship",
                    Author = "Robert C. Martin",
                    Genre = "Programming",
                    ISBN = "978-0132350884",
                    Summary = "A comprehensive guide to writing clean, maintainable code with practical examples and best practices.",
                    PublicationYear = 2008,
                    AvailableCopies = 3,
                    TotalCopies = 5,
                    DateAdded = seedDate.AddMonths(-6)
                },
                new Book
                {
                    BookId = 2,
                    Title = "Introduction to Algorithms",
                    Author = "Thomas H. Cormen",
                    Genre = "Computer Science",
                    ISBN = "978-0262033848",
                    Summary = "Comprehensive text on algorithms covering a broad range of topics in depth.",
                    PublicationYear = 2009,
                    AvailableCopies = 2,
                    TotalCopies = 3,
                    DateAdded = seedDate.AddMonths(-5)
                },
                new Book
                {
                    BookId = 3,
                    Title = "Design Patterns: Elements of Reusable Object-Oriented Software",
                    Author = "Erich Gamma",
                    Genre = "Software Engineering",
                    ISBN = "978-0201633610",
                    Summary = "Classic book on software design patterns for object-oriented programming.",
                    PublicationYear = 1994,
                    AvailableCopies = 4,
                    TotalCopies = 4,
                    DateAdded = seedDate.AddMonths(-4)
                },
                new Book
                {
                    BookId = 4,
                    Title = "The Pragmatic Programmer",
                    Author = "David Thomas",
                    Genre = "Programming",
                    ISBN = "978-0135957059",
                    Summary = "Your journey to mastery in software development with practical advice and best practices.",
                    PublicationYear = 2019,
                    AvailableCopies = 5,
                    TotalCopies = 5,
                    DateAdded = seedDate.AddMonths(-3)
                },
                new Book
                {
                    BookId = 5,
                    Title = "Code Complete: A Practical Handbook of Software Construction",
                    Author = "Steve McConnell",
                    Genre = "Programming",
                    ISBN = "978-0735619678",
                    Summary = "A practical guide to software construction covering design, coding, debugging, and testing.",
                    PublicationYear = 2004,
                    AvailableCopies = 2,
                    TotalCopies = 3,
                    DateAdded = seedDate.AddMonths(-2)
                },
                new Book
                {
                    BookId = 6,
                    Title = "Head First Design Patterns",
                    Author = "Eric Freeman",
                    Genre = "Software Engineering",
                    ISBN = "978-0596007126",
                    Summary = "A brain-friendly guide to design patterns with easy-to-understand explanations.",
                    PublicationYear = 2004,
                    AvailableCopies = 3,
                    TotalCopies = 3,
                    DateAdded = seedDate.AddMonths(-1)
                },
                new Book
                {
                    BookId = 7,
                    Title = "Refactoring: Improving the Design of Existing Code",
                    Author = "Martin Fowler",
                    Genre = "Programming",
                    ISBN = "978-0134757599",
                    Summary = "Essential guide to improving code structure and design without changing its behavior.",
                    PublicationYear = 2018,
                    AvailableCopies = 4,
                    TotalCopies = 4,
                    DateAdded = seedDate.AddDays(-20)
                },
                new Book
                {
                    BookId = 8,
                    Title = "The Clean Coder: A Code of Conduct for Professional Programmers",
                    Author = "Robert C. Martin",
                    Genre = "Professional Development",
                    ISBN = "978-0137081073",
                    Summary = "Practical advice for becoming a true professional software developer.",
                    PublicationYear = 2011,
                    AvailableCopies = 3,
                    TotalCopies = 3,
                    DateAdded = seedDate.AddDays(-15)
                },
                new Book
                {
                    BookId = 9,
                    Title = "You Don't Know JS: Scope & Closures",
                    Author = "Kyle Simpson",
                    Genre = "Web Development",
                    ISBN = "978-1449335588",
                    Summary = "Deep dive into JavaScript's scope and closures mechanisms.",
                    PublicationYear = 2014,
                    AvailableCopies = 5,
                    TotalCopies = 5,
                    DateAdded = seedDate.AddDays(-10)
                },
                new Book
                {
                    BookId = 10,
                    Title = "Eloquent JavaScript",
                    Author = "Marijn Haverbeke",
                    Genre = "Web Development",
                    ISBN = "978-1593279509",
                    Summary = "A modern introduction to programming with JavaScript.",
                    PublicationYear = 2018,
                    AvailableCopies = 4,
                    TotalCopies = 4,
                    DateAdded = seedDate.AddDays(-5)
                }
            );
        }
    }
}
