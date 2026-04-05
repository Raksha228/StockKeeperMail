<div align="center">

# StockKeeperMail

Desktop inventory, warehouse, and order management system built with WPF, Entity Framework Core, and SQL Server.

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-10-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![WPF](https://img.shields.io/badge/WPF-5C2D91?style=for-the-badge&logo=windows&logoColor=white)
![XAML](https://img.shields.io/badge/XAML-0C54C2?style=for-the-badge&logo=xaml&logoColor=white)
![EF Core](https://img.shields.io/badge/Entity_Framework_Core-6DB33F?style=for-the-badge&logo=.net&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Material Design](https://img.shields.io/badge/Material_Design_in_XAML-757575?style=for-the-badge&logo=materialdesign&logoColor=white)
![MVVM](https://img.shields.io/badge/MVVM-CommunityToolkit-0A84FF?style=for-the-badge)

[Repository](https://github.com/Raksha228/StockKeeperMail) · [Issues](https://github.com/Raksha228/StockKeeperMail/issues)

</div>

---

## Table of Contents

- [About the Project](#about-the-project)
- [Core Functionality](#core-functionality)
- [Technology Stack](#technology-stack)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Database Setup](#database-setup)
- [First Login and Initial Data](#first-login-and-initial-data)
- [Planned Improvements](#planned-improvements)

## About the Project

**StockKeeperMail** is a desktop system for managing warehouse operations, products, orders, customers, staff, and internal communication in one application.

The project is focused on day-to-day inventory workflows: keeping product records up to date, tracking stock by warehouse location, creating and processing orders, monitoring delivery status, registering defective goods, and keeping an internal audit trail of important actions.

Unlike a minimal CRUD demo, this application combines operational screens, warehouse movement logic, dashboard analytics, role-based access control, internal staff messaging, and invoice printing in a single WPF solution.

## Core Functionality

### 1. Authentication and access control
- Staff login through the desktop client
- Role-based permissions for separate modules
- Restriction of view, create, edit, and delete actions by role
- Separation of business areas such as orders, products, customers, storage, logs, roles, suppliers, and staff

### 2. Dashboard and operational analytics
- Monthly revenue overview
- Monthly order count
- Total products in stock
- Delivery status counters for:
  - Processing
  - Shipped
  - In Transit
  - Delivered
- Sales chart for the current year

### 3. Product and catalog management
- Product creation and editing
- SKU, unit, price, availability, quantity, supplier, and category management
- Category management
- Supplier management
- Product-to-location assignment

### 4. Warehouse and stock operations
- Warehouse location management
- Detailed stock view by location
- Moving products between locations
- Taking products from a location
- Disposing products from stock
- Declaring defective items
- Updating product quantities during warehouse operations

### 5. Order management
- Order creation and editing
- Customer binding for each order
- Order detail management
- Automatic order total calculation
- Delivery status tracking
- Invoice view and invoice printing

### 6. Customer and staff management
- Customer records
- Staff records
- Role assignment for staff members
- Administrative control over business users inside the system

### 7. Internal communication and traceability
- Built-in internal message module between staff members
- Inbox and sent views
- Read/unread message state
- System log records for important actions

## Technology Stack

- **Language:** C#
- **Framework:** .NET 10 Windows
- **UI:** WPF + XAML
- **Architecture pattern:** MVVM
- **MVVM toolkit:** CommunityToolkit.Mvvm
- **UI styling:** MaterialDesignThemes + MaterialDesignColors
- **Charts:** LiveCharts
- **ORM:** Entity Framework Core
- **Database:** Microsoft SQL Server
- **Configuration:** Microsoft.Extensions.Configuration + JSON configuration file

## Architecture

The solution is split into two main projects:

- **StockKeeperMail.Desktop** — WPF desktop client with views, view models, controls, services, navigation, and user workflows
- **StockKeeperMail.Database** — Entity Framework Core context, entities, migrations, and database configuration

This separation keeps the presentation layer independent from the persistence layer and makes it easier to maintain the codebase.

## Project Structure

```text
StockKeeperMail/
├── StockKeeperMail.Desktop/
│   ├── Controls/
│   ├── DAL/
│   ├── Services/
│   ├── Stores/
│   ├── Utilities/
│   ├── ViewModels/
│   ├── Views/
│   └── ResourceDictionaries/
├── StockKeeperMail.Database/
│   ├── Data/
│   ├── Migrations/
│   ├── Models/
│   ├── Services/
│   └── dbconfig.json
└── StockKeeperMail.slnx
```

## Getting Started

### Prerequisites

Before running the project, make sure you have:

- Visual Studio 2022 with desktop development tools
- .NET 10 SDK
- Microsoft SQL Server Express, Developer, or LocalDB
- SQL Server Management Studio, optional but useful for inspection and manual setup

### Clone the repository

```bash
git clone https://github.com/Raksha228/StockKeeperMail.git
cd StockKeeperMail
```

### Open the solution

Open the solution file in Visual Studio and restore NuGet packages.

## Database Setup

1. Open `StockKeeperMail.Database/dbconfig.json`
2. Set your SQL Server connection string
3. In Visual Studio, open **Package Manager Console**
4. Select **StockKeeperMail.Database** as the default project
5. Apply migrations:

```powershell
Update-Database
```

If you change the data model later, create a new migration first:

```powershell
Add-Migration MigrationName
Update-Database
```

### Example connection string

```json
{
  "ConnectionStrings": {
    "DB": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=VLGMedBD;Integrated Security=True;Encrypt=False;"
  }
}
```

## First Login and Initial Data

Important note: the application does **not** currently ship with guaranteed seed data for roles and staff accounts.

That means after creating the database you should prepare at least:

- one **Role** record with the permissions you need
- one **Staff** record linked to that role
- valid `StaffUsername` and `StaffPassword` values for login

Without initial staff data, the login screen will open correctly, but there will be no account to authenticate with.

## Planned Improvements

- stronger bootstrap flow for first launch and admin account creation
- password hashing instead of plain-text password storage
- automatic seed data for initial roles and staff
- export and reporting improvements
- screenshots and interface preview in the README
- deployment instructions for release builds

---

If this project is useful, consider starring the repository and opening an issue for suggestions or improvements.
