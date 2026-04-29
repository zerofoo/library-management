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
                    Title = "The Women",
                    Author = "Kristin Hannah",
                    Genre = "Historical Fiction",
                    ISBN = "978-1250178633",
                    Summary = "A powerful story of friendship and bravery that changed history. Women who served in the Vietnam War faced challenges that tested their courage and changed lives forever.",
                    PublicationYear = 2024,
                    CoverImagePath = "/images/book-covers/the-women.png",
                    AvailableCopies = 4,
                    TotalCopies = 5,
                    DateAdded = seedDate.AddMonths(-6)
                },
                new Book
                {
                    BookId = 2,
                    Title = "Political Ideologies: An Introduction",
                    Author = "Andrew Heywood",
                    Genre = "Political Science",
                    ISBN = "978-1137606013",
                    Summary = "The most popular and comprehensive introduction to political ideologies, this best-selling textbook analyzes the major political ideologies of our time.",
                    PublicationYear = 2021,
                    CoverImagePath = "/images/book-covers/political-ideologies.png",
                    AvailableCopies = 3,
                    TotalCopies = 4,
                    DateAdded = seedDate.AddMonths(-5)
                },
                new Book
                {
                    BookId = 3,
                    Title = "Things We Never Got Over",
                    Author = "Lucy Score",
                    Genre = "Romance",
                    ISBN = "978-1728282145",
                    Summary = "He's absolutely not falling for the good girl. A grumpy small-town romance from the Sunday Times and New York Times bestselling author.",
                    PublicationYear = 2022,
                    CoverImagePath = "/images/book-covers/things-we-never-got-over.png",
                    AvailableCopies = 5,
                    TotalCopies = 6,
                    DateAdded = seedDate.AddMonths(-4)
                },
                new Book
                {
                    BookId = 4,
                    Title = "Ikigai: The Japanese Secret to a Long and Happy Life",
                    Author = "Hector Garcia and Francesc Miralles",
                    Genre = "Self-Help",
                    ISBN = "978-0143130727",
                    Summary = "An international bestseller revealing the Japanese secret to finding purpose, meaning, and joy in life. Discover your reason for being.",
                    PublicationYear = 2017,
                    CoverImagePath = "/images/book-covers/ikigai.png",
                    AvailableCopies = 4,
                    TotalCopies = 5,
                    DateAdded = seedDate.AddMonths(-3)
                },
                new Book
                {
                    BookId = 5,
                    Title = "Mindset: Changing the way you think to fulfil your potential",
                    Author = "Dr Carol S. Dweck",
                    Genre = "Psychology",
                    ISBN = "978-1780332000",
                    Summary = "World-renowned Stanford psychologist Carol Dweck shows how success can be influenced by how we think about our talents and abilities. The growth mindset creates motivation and productivity.",
                    PublicationYear = 2017,
                    CoverImagePath = "/images/book-covers/mindset.png",
                    AvailableCopies = 3,
                    TotalCopies = 4,
                    DateAdded = seedDate.AddMonths(-2)
                },
                new Book
                {
                    BookId = 6,
                    Title = "Atomic Habits: An Easy & Proven Way to Build Good Habits & Break Bad Ones",
                    Author = "James Clear",
                    Genre = "Self-Help",
                    ISBN = "978-0735211292",
                    Summary = "Tiny changes, remarkable results. The phenomenal international bestseller with over 25 million copies sold. Transform your life with tiny changes in behavior, starting today.",
                    PublicationYear = 2018,
                    CoverImagePath = "/images/book-covers/atomic-habits.png",
                    AvailableCopies = 5,
                    TotalCopies = 6,
                    DateAdded = seedDate.AddMonths(-1)
                }
            );
        }
    }
}
