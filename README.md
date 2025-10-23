# Simple HR Management System

This project is a simple HR Management System web application built with ASP.NET Core 8 and PostgreSQL. It fulfills the requirements of an assessment task, demonstrating CRUD operations with filtering, reporting, and business logic implementation using modern web development patterns.

---

## ✨ Key Features

* **Multi-Entity Management:** Full CRUD operations for Companies, Designations, Departments, Shifts, and Employees.
* **Company Filtering:** A global company selector using browser cookies filters data across all relevant sections.
* **AJAX Interface:** Create, Update, and Delete operations are handled via AJAX using Bootstrap modals for a smooth user experience without full page reloads.
* **Data Validation:** Both client-side (jQuery Unobtrusive) and server-side (Data Annotations) validation are implemented.
* **Business Logic:**
    * Automatic salary component calculation based on Gross pay and company percentages.
    * Automatic "Late" status calculation for attendance based on shift timings.
* **Bulk Operations:** Bulk attendance entry for a selected date.
* **Database Procedures:** PostgreSQL functions (`sp_summarize_attendance`, `sp_calculate_salary`) handle data aggregation and calculation.
* **Reporting:** A dedicated reports page with filters (Department, Date Range, Month/Year, Payment Status) displays various summaries (Employee List, Attendance, Salary) and allows marking salaries as paid.

---

## 🛠️ Technology Stack

* **Backend:** ASP.NET Core 8 MVC
* **Database:** PostgreSQL
* **ORM:** Entity Framework Core 8 (Code-First with Migrations)
* **Frontend:** HTML, CSS, Bootstrap 5, JavaScript, jQuery
* **Architecture:** Layered Architecture, Repository Pattern, Unit of Work Pattern, ViewModels

---

## 🚀 Setup Instructions

1.  **Prerequisites:**
    * .NET 8 SDK installed.
    * PostgreSQL server installed and running.
    * A Git client (like the one integrated into Visual Studio).

2.  **Clone the Repository:**
    ```bash
    git clone [https://github.com/YourUsername/YourRepoName.git](https://github.com/YourUsername/YourRepoName.git)
    cd YourRepoName
    ```

3.  **Database Setup:**
    * Create a new PostgreSQL database (e.g., `simple_hr_db`).
    * Open the `appsettings.json` file in the project root.
    * Update the `DefaultConnection` string with your PostgreSQL server details (host, database name, user ID, and password).
        ```json
        "ConnectionStrings": {
          "DefaultConnection": "Server=localhost;Port=5432;Database=simple_hr_db;User Id=postgres;Password=YourPassword"
        }
        ```
    * Open the **Package Manager Console** in Visual Studio (`Tools` -> `NuGet Package Manager` -> `Package Manager Console`).
    * Run the command `Update-Database` to apply all migrations and create the tables.
    * **(Optional)** Manually run the SQL scripts in pgAdmin to create the stored procedures (`sp_summarize_attendance` and `sp_calculate_salary`). *Note: These can also be created via migrations if added.*

4.  **Build the Project:**
    * Open the solution (`.sln` file) in Visual Studio.
    * Build the solution (Ctrl+Shift+B or `Build` -> `Build Solution`).

---

## ▶️ Running the Application

1.  Ensure your PostgreSQL server is running.
2.  In Visual Studio, press **F5** or click the green "Start Debugging" button ▶️.
3.  The application will build, start a local web server, and open in your default browser.
4.  **First Use:** Navigate to the "Companies" page and add at least one company with salary percentages. Then, use the dropdown in the navigation bar to select that company. You can then proceed to add Designations, Departments, Shifts, Employees, and Attendance records.
