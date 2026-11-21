MotoLedger/
|-- Program.cs
|-- appsettings.json
|
|-- Data/
| |-- ApplicationDbContext.cs
|
|-- Models/
| |-- Entities/
| | |-- ApplicationUser.cs
| | |-- Vehicle.cs
| | |-- VehicleExpense.cs
| | |-- SalesTransaction.cs
| |
| |-- ViewModels/
| |-- DashboardVM.cs
| |-- VehicleFormVM.cs
| |-- ExpenseFormVM.cs
| |-- SaleCheckoutVM.cs
|
|-- Repositories/
| |-- Interfaces/
| | |-- IVehicleRepository.cs
| | |-- IExpenseRepository.cs
| | |-- ISalesRepository.cs
| |
| |-- Implementations/
| |-- VehicleRepository.cs
| |-- ExpenseRepository.cs
| |-- SalesRepository.cs
|
|-- Services/
| |-- EmailSender.cs
|
|-- Controllers/
| |-- HomeController.cs
| |-- VehicleController.cs
| |-- ExpenseController.cs
| |-- SalesController.cs
|
|-- Views/
| |-- Home/
| | |-- Index.cshtml
| |
| |-- Vehicle/
| | |-- Index.cshtml
| | |-- Create.cshtml
| | |-- Details.cshtml
| |
| |-- Expense/
| | |-- Create.cshtml
| | |-- History.cshtml
| |
| |-- Sales/
| | |-- Checkout.cshtml
| | |-- Ledger.cshtml
| | |-- Invoice.cshtml
| |
| |-- Shared/
| |-- \_Layout.cshtml
| |-- \_ValidationScriptsPartial.cshtml
|
|-- wwwroot/
|-- css/
| |-- site.css
|
|-- js/
|-- site.js
