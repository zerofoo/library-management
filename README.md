# Library Management System

A comprehensive ASP.NET Core MVC application for managing library operations with role-based access control for Librarians and Members.

## Group Members

| Student ID | Full Name |
|------------|-----------|
| 2021001 | John Michael Smith |
| 2021002 | Sarah Jane Williams |
| 2021003 | David Robert Johnson |
| 2021004 | Emily Grace Davis |

## Features

### Librarian Features
- **Dashboard**: Overview of library statistics (total books, members, active borrows, overdue items)
- **Book Management**: Complete CRUD operations with image upload functionality
- **Library Profile**: Create and manage library information and operating hours
- **Borrowing Configuration**: Set loan duration, renewal limits, maximum borrowable items, and overdue penalties
- **Transaction Management**: View all borrowing transactions, mark books as returned, calculate fines
- **Reports & Analytics**: 
  - Borrowing trends over time
  - Overdue books report
  - Most active members
  - Most popular books

### Member Features
- **Browse Books**: Search and filter available books by title, author, or genre
- **Borrow/Reserve**: Borrow available books or reserve unavailable ones
- **My Dashboard**: View active borrows, overdue items, and reserved books
- **Renewal**: Renew borrowed books (up to configured limit)
- **Borrowing History**: View complete history of borrowed books
- **Rate & Review**: Rate books (1-5 stars) and write reviews after returning

### Common Features
- **User Authentication**: Secure registration and login with BCrypt password hashing
- **Role-Based Authorization**: Separate access controls for Librarians and Members
- **Responsive Design**: Professional custom CSS with mobile-friendly layout
- **Input Validation**: Client-side and server-side validation with clear error messages
- **Fine Calculation**: Automatic calculation of overdue fines

## Technology Stack

- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: SQL Server Express (LocalDB)
- **ORM**: Entity Framework Core 8.0
- **Authentication**: Custom cookie-based authentication with BCrypt
- **Frontend**: HTML5, CSS3, JavaScript (jQuery)
- **Validation**: jQuery Validation & Unobtrusive Validation

## Prerequisites

- .NET 8.0 SDK or higher
- SQL Server Express (LocalDB)
- Visual Studio 2022 or Visual Studio Code
- SQL Server Management Studio (optional, for database inspection)

## Installation & Setup

### 1. Extract the Project
Extract the project ZIP file to your desired location.

### 2. Restore NuGet Packages
Open the project in Visual Studio or run:
```bash
dotnet restore
```

### 3. Update Database Connection String (Optional)
The default connection string uses SQL Server Express LocalDB. If you need to modify it, edit `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=LibraryManagementDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

### 4. Create and Seed the Database
Run the following commands in the Package Manager Console or terminal:

**Using Package Manager Console (Visual Studio):**
```powershell
Add-Migration InitialCreate
Update-Database
```

**Using .NET CLI:**
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

This will create the database and populate it with seed data including:
- 1 Librarian account
- 2 Member accounts
- 10 sample books
- 1 library profile with borrowing configuration

### 5. Run the Application
**Using Visual Studio:**
- Press F5 or click the "Run" button

**Using .NET CLI:**
```bash
dotnet run
```

The application will launch at `https://localhost:5001` (or the port shown in your console).

## Default Login Credentials

### Librarian Account
- **Email**: librarian@library.com
- **Password**: Librarian123

### Member Accounts
- **Email**: john.doe@email.com | **Password**: Member123
- **Email**: jane.smith@email.com | **Password**: Member123

## Project Structure

```
LibraryManagementSystem/
├── Controllers/          # MVC Controllers
│   ├── AccountController.cs
│   ├── BookController.cs
│   ├── BorrowingController.cs
│   ├── HomeController.cs
│   ├── LibrarianController.cs
│   ├── MemberController.cs
│   └── ReportController.cs
├── Models/              # Data models and view models
│   ├── Book.cs
│   ├── BookRating.cs
│   ├── BorrowingConfiguration.cs
│   ├── BorrowingTransaction.cs
│   ├── LibraryProfile.cs
│   ├── User.cs
│   └── ViewModels/
├── Views/               # Razor views
│   ├── Account/
│   ├── Book/
│   ├── Borrowing/
│   ├── Home/
│   ├── Librarian/
│   ├── Member/
│   ├── Report/
│   └── Shared/
├── Data/                # Database context
│   └── LibraryDbContext.cs
├── Helpers/             # Helper classes
│   └── SessionHelper.cs
├── Attributes/          # Custom attributes
│   └── AuthorizeRoleAttribute.cs
├── wwwroot/             # Static files
│   ├── css/
│   │   └── custom-styles.css
│   └── images/
│       └── book-covers/
├── Migrations/          # EF Core migrations
├── appsettings.json     # Configuration
├── Program.cs           # Application entry point
└── README.md            # This file
```

## Key Features Implementation

### 1. Role-Based Authorization
Custom `AuthorizeRoleAttribute` restricts access to controller actions based on user roles.

### 2. Session Management
`SessionHelper` provides extension methods for managing user sessions.

### 3. Business Logic
- Borrowing limit enforcement
- Automatic overdue status updates
- Fine calculation based on days overdue
- Renewal restrictions (max renewals, no renewal if overdue)

### 4. Data Validation
- Model-level data annotations
- Client-side validation with jQuery
- Server-side validation with ModelState
- Custom error messages

### 5. Image Upload
Book cover images are uploaded to `wwwroot/images/book-covers/` with unique filenames.

### 6. Responsive Design
Custom CSS with:
- Mobile-first approach
- Flexbox and CSS Grid layouts
- Professional color scheme
- Smooth transitions and hover effects

## Database Schema

### Core Tables
1. **Users** - User authentication and profile information
2. **Books** - Book catalog with availability tracking
3. **LibraryProfiles** - Library information (one per librarian)
4. **BorrowingConfigurations** - Library borrowing rules
5. **BorrowingTransactions** - Borrowing history and active loans
6. **BookRatings** - User reviews and ratings

### Relationships
- User 1:N BorrowingTransaction
- Book 1:N BorrowingTransaction
- User 1:N BookRating
- Book 1:N BookRating
- LibraryProfile 1:1 BorrowingConfiguration
- User 1:1 LibraryProfile

## Testing the Application

### Testing Workflow

1. **Register a New User**
   - Navigate to Register page
   - Create a Member account
   - Login with new credentials

2. **Member Workflow**
   - Browse available books
   - View book details and ratings
   - Borrow a book
   - View "My Borrowing" dashboard
   - Return to history after librarian marks as returned
   - Rate and review the returned book

3. **Librarian Workflow**
   - Login as librarian
   - Create library profile (if not exists)
   - Configure borrowing settings
   - Add new books with cover images
   - Manage borrowing transactions
   - Mark books as returned
   - Calculate and manage fines
   - View analytics reports

### Edge Cases to Test
- Borrowing when at maximum limit
- Attempting to borrow with unpaid fines
- Renewing books multiple times (up to limit)
- Returning overdue books (fine calculation)
- Rating books multiple times (should update existing rating)

## Troubleshooting

### Database Connection Issues
- Ensure SQL Server Express is installed and running
- Check connection string in `appsettings.json`
- Verify LocalDB instance is available: `sqllocaldb info`

### Migration Errors
- Delete the Migrations folder and database
- Re-run: `Add-Migration InitialCreate` and `Update-Database`

### File Upload Issues
- Ensure `wwwroot/images/book-covers/` directory exists
- Check folder permissions for write access

### Session Issues
- Clear browser cookies
- Restart the application

## Future Enhancements

- Email notifications for due dates and overdue books
- PDF/CSV report export functionality
- Book recommendation system
- Advanced search with filters
- Member profile management
- Barcode scanning integration
- Multi-library support

## License

This project is created for educational purposes as part of a university assignment.

## Contact

For questions or issues, please contact the development team through your course instructor.

---

**Last Updated**: April 2026
**Version**: 1.0.0
