Project Name: MotoLedger (Dealership Inventory & Real-Time Profit Tracking System)
Architecture: ASP.NET Core MVC (.NET 8)
Database: Microsoft SQL Server via Entity Framework Core
Authentication: ASP.NET Core Identity (Cookie-based, Multi-Role RBAC)
Frontend: HTML5, CSS3, Bootstrap 5 (Razor Views .cshtml)

1. Project Overview
   MotoLedger is a web-based automotive lot management platform built to solve a specific business problem: tracking post-purchase reconditioning expenses (mechanical repairs, detailing, taxes) against a vehicle's base purchase cost to accurately calculate the net profit margin when the car is sold.

2. Key Academic & Technical Constraints (Must Follow)
   Design Pattern: Strictly follow the MVC (Model-View-Controller) architecture using the Repository Pattern for data access. Keep database logic out of the controllers.

Authentication & Security: Use ASP.NET Core Identity. Implement Role-Based Access Control (RBAC) using roles (Admin, Sales, Workshop).

Performance Optimization: Utilize ASP.NET Core IMemoryCache on the Dashboard to cache aggregate KPIs (Total Cars, Total Profit) and prevent constant database polling.

Data Integrity: Use explicit SQL relationships (One-to-Many, One-to-One) via Entity Framework Core.

3. Core Features & Business Logic Flow
   Dashboard Module: Displays cached KPIs (Total Active Cars, Total Investment, Net Profit) and recent system activity.

Vehicle Inventory Module: CRUD operations for cars. A car starts with a BasePurchasePrice. Status can be Available, Sold, or Archived.

Expense Tracking Module: Users can add multiple VehicleExpense items (e.g., "$200 for Paint") to a specific Vehicle.

Math: Grand Total Cost = BasePurchasePrice + Sum(All Vehicle Expenses).

Sales Execution Module: When a car is sold, it generates a SalesTransaction.

Math: Net Profit = SalePrice - Grand Total Cost.

Locks the vehicle status to Sold so no further expenses can be added.

4. Database Schema (Entities)
   Located in Models/Entities/:

ApplicationUser (Inherits IdentityUser): Stores user accounts and role assignments.

Vehicle:

Properties: Id, Make, Model, RegistrationNumber, BasePurchasePrice, PurchaseDate, Status (Available/Sold).

Relationships: Has Many VehicleExpenses, Has One SalesTransaction.

VehicleExpense:

Properties: Id, VehicleId (FK), ExpenseType, VendorName, Cost, ExpenseDate, Description.

SalesTransaction:

Properties: Id, VehicleId (FK), SalePrice, SaleDate, CustomerName, CustomerPhone, PaymentMethod, CalculatedProfit.

5. ViewModels (Data Transfer to Views)
   Located in Models/ViewModels/:

DashboardVM: Int TotalAvailableCars, Decimal TotalActiveInvestment, Decimal TotalNetProfit.

VehicleFormVM: Data annotation-validated fields for creating/editing a car.

ExpenseFormVM: Validated fields for adding a repair cost to a specific car.

SaleCheckoutVM: Validated fields capturing the sale price and customer details.

6. Repository Layer (Data Access)
   Located in Repositories/Interfaces/ and Repositories/Implementations/:

IVehicleRepository / VehicleRepository: Handles adding, editing, soft-deleting, and listing active vehicles.

IExpenseRepository / ExpenseRepository: Handles logging expenses and dynamically calculating the GrandTotalCost for a vehicle.

ISalesRepository / SalesRepository: Handles executing the sale, locking the car, and calculating the final NetProfit.

7. Controllers & Views Structure
   HomeController: Fetches DashboardVM (using IMemoryCache) and returns Views/Home/Index.cshtml.

VehicleController: Uses IVehicleRepository to return Inventory Grids (Index.cshtml) and forms (Create.cshtml, Details.cshtml).

ExpenseController: Uses IExpenseRepository to log costs (Create.cshtml) and view a car's expense ledger (History.cshtml).

SalesController: Uses ISalesRepository to finalize sales (Checkout.cshtml), view completed sales (Ledger.cshtml), and print receipts (Invoice.cshtml).

Instructions for the AI Assistant:
When I ask you to generate a file, class, controller, or view for this project, you must ensure it perfectly aligns with this architecture. Do not use generic naming conventions; use the exact folder paths, entity names, and view models outlined in this document. Always use Bootstrap 5 for .cshtml views and ensure form tags use ASP.NET Core Tag Helpers (asp-for, asp-controller, asp-action).
