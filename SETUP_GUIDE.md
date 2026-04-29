# Library Management System - Quick Setup Guide

## Prerequisites Checklist
- [ ] .NET 8.0 SDK installed
- [ ] SQL Server Express (LocalDB) installed
- [ ] Visual Studio 2022 or VS Code installed

## Quick Start (5 Steps)

### Step 1: Open the Project
Open `LibraryManagementSystem.csproj` in Visual Studio, or open the folder in VS Code.

### Step 2: Restore Packages
In Visual Studio, packages will restore automatically. In VS Code:
```bash
dotnet restore
```

### Step 3: Create the Database
**Visual Studio - Package Manager Console:**
```powershell
Add-Migration InitialCreate
Update-Database
```

**VS Code - Terminal:**
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Step 4: Run the Application
**Visual Studio:** Press F5

**VS Code:**
```bash
dotnet run
```

### Step 5: Login and Test
Open browser to `https://localhost:5001`

**Librarian Login:**
- Email: `librarian@library.com`
- Password: `Librarian123`

**Member Login:**
- Email: `bibi.dip@email.com` or `san.vam@email.com`
- Password: `Member123`

## Common Issues & Solutions

### Issue: "Unable to connect to database"
**Solution:** Ensure SQL Server Express LocalDB is installed:
```bash
sqllocaldb info
```
If not listed, install SQL Server Express from Microsoft.

### Issue: "Migration already exists"
**Solution:** Delete the `Migrations` folder and re-run the migration commands.

### Issue: "Port already in use"
**Solution:** Change port in `Properties/launchSettings.json`:
```json
"applicationUrl": "https://localhost:5002;http://localhost:5001"
```

### Issue: "File upload not working"
**Solution:** Ensure `wwwroot/images/book-covers/` directory exists with write permissions.

## Testing Checklist

### As Librarian:
- [ ] Login with librarian account
- [ ] View dashboard statistics
- [ ] Create library profile
- [ ] Configure borrowing settings
- [ ] Add a new book (with cover image)
- [ ] Edit an existing book
- [ ] View all borrowing transactions
- [ ] Mark a book as returned
- [ ] View reports and analytics

### As Member:
- [ ] Register a new member account
- [ ] Login with member account
- [ ] Browse available books
- [ ] View book details
- [ ] Borrow a book
- [ ] View "My Borrowing" dashboard
- [ ] Renew a borrowed book
- [ ] View borrowing history
- [ ] Rate and review a book (after librarian marks it as returned)

### Edge Cases:
- [ ] Try borrowing when at maximum limit (should show error)
- [ ] Try renewing a book multiple times (should stop at limit)
- [ ] Return a book late (should calculate fine)
- [ ] Try borrowing with unpaid fines (should be blocked)

## Features Overview

### Database Schema
- **6 Entity Models**: User, Book, LibraryProfile, BorrowingConfiguration, BorrowingTransaction, BookRating
- **Relationships**: Fully configured with Fluent API
- **Seed Data**: Pre-populated with sample data

### Authentication & Authorization
- BCrypt password hashing
- Session-based authentication
- Role-based authorization (Librarian/Member)

### Business Logic
- Automatic overdue detection
- Fine calculation based on days overdue
- Borrowing limit enforcement
- Renewal restrictions
- Reservation system for unavailable books

### UI/UX
- Responsive design (mobile-friendly)
- Professional custom CSS
- Role-based navigation
- Form validation (client and server-side)
- Status badges and alerts

## Project Structure
```
LibraryManagementSystem/
├── Controllers/          (7 controllers)
├── Models/              (6 entities + 5 view models)
├── Views/               (24 views)
├── Data/                (DbContext with seed data)
├── Helpers/             (Session management)
├── Attributes/          (Authorization)
├── wwwroot/
│   ├── css/             (Custom styles)
│   └── images/          (Book covers upload folder)
├── Migrations/          (EF Core migrations)
├── Properties/          (Launch settings)
└── Configuration files  (appsettings.json, etc.)
```

## Support
For detailed information, see `README.md`

**Version:** 1.0.0  
**Last Updated:** April 2026
