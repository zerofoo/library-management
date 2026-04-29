using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PublicationYear = table.Column<int>(type: "int", nullable: true),
                    CoverImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AvailableCopies = table.Column<int>(type: "int", nullable: false),
                    TotalCopies = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "BookRatings",
                columns: table => new
                {
                    RatingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DateSubmitted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookRatings", x => x.RatingId);
                    table.ForeignKey(
                        name: "FK_BookRatings_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookRatings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BorrowingTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    BorrowDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RenewalCount = table.Column<int>(type: "int", nullable: false),
                    FineAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinePaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowingTransactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_BorrowingTransactions_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BorrowingTransactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LibraryProfiles",
                columns: table => new
                {
                    LibraryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LibraryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OperatingHours = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryProfiles", x => x.LibraryId);
                    table.ForeignKey(
                        name: "FK_LibraryProfiles_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BorrowingConfigurations",
                columns: table => new
                {
                    ConfigId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LibraryId = table.Column<int>(type: "int", nullable: false),
                    LoanDurationDays = table.Column<int>(type: "int", nullable: false),
                    MaxRenewals = table.Column<int>(type: "int", nullable: false),
                    MaxBorrowableItems = table.Column<int>(type: "int", nullable: false),
                    OverduePenaltyPerDay = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowingConfigurations", x => x.ConfigId);
                    table.ForeignKey(
                        name: "FK_BorrowingConfigurations_LibraryProfiles_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "LibraryProfiles",
                        principalColumn: "LibraryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "Author", "AvailableCopies", "CoverImagePath", "DateAdded", "Genre", "ISBN", "PublicationYear", "Summary", "Title", "TotalCopies" },
                values: new object[,]
                {
                    { 1, "Robert C. Martin", 3, null, new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Programming", "978-0132350884", 2008, "A comprehensive guide to writing clean, maintainable code with practical examples and best practices.", "Clean Code: A Handbook of Agile Software Craftsmanship", 5 },
                    { 2, "Thomas H. Cormen", 2, null, new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Computer Science", "978-0262033848", 2009, "Comprehensive text on algorithms covering a broad range of topics in depth.", "Introduction to Algorithms", 3 },
                    { 3, "Erich Gamma", 4, null, new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Software Engineering", "978-0201633610", 1994, "Classic book on software design patterns for object-oriented programming.", "Design Patterns: Elements of Reusable Object-Oriented Software", 4 },
                    { 4, "David Thomas", 5, null, new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Programming", "978-0135957059", 2019, "Your journey to mastery in software development with practical advice and best practices.", "The Pragmatic Programmer", 5 },
                    { 5, "Steve McConnell", 2, null, new DateTime(2023, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Programming", "978-0735619678", 2004, "A practical guide to software construction covering design, coding, debugging, and testing.", "Code Complete: A Practical Handbook of Software Construction", 3 },
                    { 6, "Eric Freeman", 3, null, new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Software Engineering", "978-0596007126", 2004, "A brain-friendly guide to design patterns with easy-to-understand explanations.", "Head First Design Patterns", 3 },
                    { 7, "Martin Fowler", 4, null, new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Programming", "978-0134757599", 2018, "Essential guide to improving code structure and design without changing its behavior.", "Refactoring: Improving the Design of Existing Code", 4 },
                    { 8, "Robert C. Martin", 3, null, new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Professional Development", "978-0137081073", 2011, "Practical advice for becoming a true professional software developer.", "The Clean Coder: A Code of Conduct for Professional Programmers", 3 },
                    { 9, "Kyle Simpson", 5, null, new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Web Development", "978-1449335588", 2014, "Deep dive into JavaScript's scope and closures mechanisms.", "You Don't Know JS: Scope & Closures", 5 },
                    { 10, "Marijn Haverbeke", 4, null, new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Web Development", "978-1593279509", 2018, "A modern introduction to programming with JavaScript.", "Eloquent JavaScript", 4 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "FirstName", "IsActive", "LastName", "Password", "PhoneNumber", "RegistrationDate", "Role" },
                values: new object[,]
                {
                    { 1, "librarian@library.com", "Admin", true, "Librarian", "$2a$11$kd.lRYWl3l0aFlSnrc1TpOh1RssVVoirOtqVG2XGT5aHT27T.WBRC", "555-0100", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Librarian" },
                    { 2, "john.doe@email.com", "John", true, "Doe", "$2a$11$2JS5LDKFgcX2jjRsVrJhuekxnhp.qIF6eNxEt5V.PYpwANK7XinGG", "555-0101", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Member" },
                    { 3, "jane.smith@email.com", "Jane", true, "Smith", "$2a$11$/EI9VrTGKQ1zpiwX26fCCuDJb3djSy.zhBeA/AgXsrssY3xbTc0Se", "555-0102", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Member" }
                });

            migrationBuilder.InsertData(
                table: "LibraryProfiles",
                columns: new[] { "LibraryId", "ContactEmail", "ContactPhone", "CreatedByUserId", "CreatedDate", "LibraryName", "Location", "OperatingHours" },
                values: new object[] { 1, "info@universitylibrary.edu", "555-0200", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "University Central Library", "123 Campus Drive, Education City", "Mon-Fri: 8:00 AM - 8:00 PM, Sat-Sun: 10:00 AM - 6:00 PM" });

            migrationBuilder.InsertData(
                table: "BorrowingConfigurations",
                columns: new[] { "ConfigId", "LibraryId", "LoanDurationDays", "MaxBorrowableItems", "MaxRenewals", "OverduePenaltyPerDay" },
                values: new object[] { 1, 1, 14, 5, 2, 0.50m });

            migrationBuilder.CreateIndex(
                name: "IX_BookRatings_BookId",
                table: "BookRatings",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookRatings_UserId",
                table: "BookRatings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowingConfigurations_LibraryId",
                table: "BorrowingConfigurations",
                column: "LibraryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BorrowingTransactions_BookId",
                table: "BorrowingTransactions",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowingTransactions_UserId",
                table: "BorrowingTransactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryProfiles_CreatedByUserId",
                table: "LibraryProfiles",
                column: "CreatedByUserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookRatings");

            migrationBuilder.DropTable(
                name: "BorrowingConfigurations");

            migrationBuilder.DropTable(
                name: "BorrowingTransactions");

            migrationBuilder.DropTable(
                name: "LibraryProfiles");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
