using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMemberEmails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 10);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 1,
                columns: new[] { "Author", "AvailableCopies", "CoverImagePath", "Genre", "ISBN", "PublicationYear", "Summary", "Title" },
                values: new object[] { "Kristin Hannah", 4, "/images/book-covers/the-women.png", "Historical Fiction", "978-1250178633", 2024, "A powerful story of friendship and bravery that changed history. Women who served in the Vietnam War faced challenges that tested their courage and changed lives forever.", "The Women" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 2,
                columns: new[] { "Author", "AvailableCopies", "CoverImagePath", "Genre", "ISBN", "PublicationYear", "Summary", "Title", "TotalCopies" },
                values: new object[] { "Andrew Heywood", 3, "/images/book-covers/political-ideologies.png", "Political Science", "978-1137606013", 2021, "The most popular and comprehensive introduction to political ideologies, this best-selling textbook analyzes the major political ideologies of our time.", "Political Ideologies: An Introduction", 4 });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 3,
                columns: new[] { "Author", "AvailableCopies", "CoverImagePath", "Genre", "ISBN", "PublicationYear", "Summary", "Title", "TotalCopies" },
                values: new object[] { "Lucy Score", 5, "/images/book-covers/things-we-never-got-over.png", "Romance", "978-1728282145", 2022, "He's absolutely not falling for the good girl. A grumpy small-town romance from the Sunday Times and New York Times bestselling author.", "Things We Never Got Over", 6 });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 4,
                columns: new[] { "Author", "AvailableCopies", "CoverImagePath", "Genre", "ISBN", "PublicationYear", "Summary", "Title" },
                values: new object[] { "Hector Garcia and Francesc Miralles", 4, "/images/book-covers/ikigai.png", "Self-Help", "978-0143130727", 2017, "An international bestseller revealing the Japanese secret to finding purpose, meaning, and joy in life. Discover your reason for being.", "Ikigai: The Japanese Secret to a Long and Happy Life" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 5,
                columns: new[] { "Author", "AvailableCopies", "CoverImagePath", "Genre", "ISBN", "PublicationYear", "Summary", "Title", "TotalCopies" },
                values: new object[] { "Dr Carol S. Dweck", 3, "/images/book-covers/mindset.png", "Psychology", "978-1780332000", 2017, "World-renowned Stanford psychologist Carol Dweck shows how success can be influenced by how we think about our talents and abilities. The growth mindset creates motivation and productivity.", "Mindset: Changing the way you think to fulfil your potential", 4 });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 6,
                columns: new[] { "Author", "AvailableCopies", "CoverImagePath", "Genre", "ISBN", "PublicationYear", "Summary", "Title", "TotalCopies" },
                values: new object[] { "James Clear", 5, "/images/book-covers/atomic-habits.png", "Self-Help", "978-0735211292", 2018, "Tiny changes, remarkable results. The phenomenal international bestseller with over 25 million copies sold. Transform your life with tiny changes in behavior, starting today.", "Atomic Habits: An Easy & Proven Way to Build Good Habits & Break Bad Ones", 6 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$LkfJTaJC.5Rc/f71MkE4nO7JNnjC8nj1LlnHasftQ4CHVhl53agVy");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "Email", "Password" },
                values: new object[] { "bibi.dip@email.com", "$2a$11$sfjSG/nNtx.InDy.gx63beBJiU.eR8t4ib00Y5SyQ1Jr9LnseYLKi" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "Email", "Password" },
                values: new object[] { "san.vam@email.com", "$2a$11$89rqjRPvYUfM0RLymF.WbOZnE8PO1f8vvv4y.baaXH..Kj4QRyoZK" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 1,
                columns: new[] { "Author", "AvailableCopies", "CoverImagePath", "Genre", "ISBN", "PublicationYear", "Summary", "Title" },
                values: new object[] { "Robert C. Martin", 3, null, "Programming", "978-0132350884", 2008, "A comprehensive guide to writing clean, maintainable code with practical examples and best practices.", "Clean Code: A Handbook of Agile Software Craftsmanship" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 2,
                columns: new[] { "Author", "AvailableCopies", "CoverImagePath", "Genre", "ISBN", "PublicationYear", "Summary", "Title", "TotalCopies" },
                values: new object[] { "Thomas H. Cormen", 2, null, "Computer Science", "978-0262033848", 2009, "Comprehensive text on algorithms covering a broad range of topics in depth.", "Introduction to Algorithms", 3 });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 3,
                columns: new[] { "Author", "AvailableCopies", "CoverImagePath", "Genre", "ISBN", "PublicationYear", "Summary", "Title", "TotalCopies" },
                values: new object[] { "Erich Gamma", 4, null, "Software Engineering", "978-0201633610", 1994, "Classic book on software design patterns for object-oriented programming.", "Design Patterns: Elements of Reusable Object-Oriented Software", 4 });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 4,
                columns: new[] { "Author", "AvailableCopies", "CoverImagePath", "Genre", "ISBN", "PublicationYear", "Summary", "Title" },
                values: new object[] { "David Thomas", 5, null, "Programming", "978-0135957059", 2019, "Your journey to mastery in software development with practical advice and best practices.", "The Pragmatic Programmer" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 5,
                columns: new[] { "Author", "AvailableCopies", "CoverImagePath", "Genre", "ISBN", "PublicationYear", "Summary", "Title", "TotalCopies" },
                values: new object[] { "Steve McConnell", 2, null, "Programming", "978-0735619678", 2004, "A practical guide to software construction covering design, coding, debugging, and testing.", "Code Complete: A Practical Handbook of Software Construction", 3 });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 6,
                columns: new[] { "Author", "AvailableCopies", "CoverImagePath", "Genre", "ISBN", "PublicationYear", "Summary", "Title", "TotalCopies" },
                values: new object[] { "Eric Freeman", 3, null, "Software Engineering", "978-0596007126", 2004, "A brain-friendly guide to design patterns with easy-to-understand explanations.", "Head First Design Patterns", 3 });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "Author", "AvailableCopies", "CoverImagePath", "DateAdded", "Genre", "ISBN", "PublicationYear", "Summary", "Title", "TotalCopies" },
                values: new object[,]
                {
                    { 7, "Martin Fowler", 4, null, new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Programming", "978-0134757599", 2018, "Essential guide to improving code structure and design without changing its behavior.", "Refactoring: Improving the Design of Existing Code", 4 },
                    { 8, "Robert C. Martin", 3, null, new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Professional Development", "978-0137081073", 2011, "Practical advice for becoming a true professional software developer.", "The Clean Coder: A Code of Conduct for Professional Programmers", 3 },
                    { 9, "Kyle Simpson", 5, null, new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Web Development", "978-1449335588", 2014, "Deep dive into JavaScript's scope and closures mechanisms.", "You Don't Know JS: Scope & Closures", 5 },
                    { 10, "Marijn Haverbeke", 4, null, new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Web Development", "978-1593279509", 2018, "A modern introduction to programming with JavaScript.", "Eloquent JavaScript", 4 }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$kd.lRYWl3l0aFlSnrc1TpOh1RssVVoirOtqVG2XGT5aHT27T.WBRC");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "Email", "Password" },
                values: new object[] { "john.doe@email.com", "$2a$11$2JS5LDKFgcX2jjRsVrJhuekxnhp.qIF6eNxEt5V.PYpwANK7XinGG" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "Email", "Password" },
                values: new object[] { "jane.smith@email.com", "$2a$11$/EI9VrTGKQ1zpiwX26fCCuDJb3djSy.zhBeA/AgXsrssY3xbTc0Se" });
        }
    }
}
