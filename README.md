# Vehicle Rental System (Backoffice & API)

A comprehensive vehicle rental management system built with **WPF** for the backoffice and **ASP.NET Core Web API** for the backend. The system is designed to streamline vehicle rental operations, including HR management, customer handling, and rental processing. The database is powered by **MSSQL**, ensuring robust data management and scalability.

![Backoffice Dashboard Preview] <placeholder-dashboard-preview.gif>

-------

## Features

### Backoffice (WPF)
- **Vehicle Management**  
  Add, update, and remove vehicles with detailed specifications (e.g., type, model, price).
- **Customer Management**  
  Manage customer profiles, including personal details.
- **HR Management**  
  Handle employees, roles, and permissions for backoffice users.
- **Rental Processing**  
  Handle reservation requests, reservations, payments and post rental reports.
- **File Management**  
  Store and manage documents such as invoices, customer licenses.
- **Automatic Invoice Creation**  
  Generate invoices automatically upon rental finalization, with customizable templates.
- **Interactive Map**  
  Visualize rental places locations and vehicles on an interactive map.
  

  ### Core API (ASP.NET Core Web API)
- **RESTful Endpoints**  
  Standardized endpoints for vehicles, customers, rentals, reports and more.
- **JWT Authentication**  
  Secure API access with JSON Web Tokens (JWT) and role-based authorization.
- **Database Integration**  
  Seamless integration with **MSSQL** for efficient data storage and retrieval.
- **Validation & Error Handling**  
  Robust request validation and meaningful error responses.

## Technologies

**Backoffice**
- WPF (Windows Presentation Foundation)
- .NET Framework
- MVVM (Model-View-ViewModel) Architecture

**API**
- ASP.NET Core Web API
- JWT Authentication
- Entity Framework Core (with MSSQL)
- Swagger/OpenAPI

**Database**
- MSSQL (Microsoft SQL Server)

## Installation

### Prerequisites
- .NET 8 SDK
- Visual Studio 2022 (or later)
- MSSQL Server (prefferably local instance)

### Setup Instructions

1. **Clone the Repository**
   
   ```bash
   git clone https://github.com/xxnls/vehicle-rental-system.git
   cd vehicle-rental-system
   ```
2. **Change database settings (optional)**
   
   Update the connection string in `appsettings.json` (API) with your MSSQL credentials.
3. **Run migrations**
   
   Use `Update-Database` in Package Manager Console to create and update database and feed it with data from seeders.
4. **Build and start the project**

   Build and start API and then Backoffice. Login window should open. Use admin credentials `admin:123` to login into the app.

### Example Usage

Adding a New Vehicle
![Vehicle Creation Process] <gif-add-vehicle.gif>
Path: Vehicles > Add Vehicle > Fill details (model, type, price) > Save

Processing a Rental
![Rental Processing] <gif-process-rental.gif>
Path: Rentals > Select Customer > Assign Vehicle > Confirm Rental

Generating an Invoice
![Invoice Generation] <gif-invoice-generation.gif>
Path: Rentals > Select Rental > Generate Invoice > Download PDF

Interactive Map
![Interactive Map] <gif-interactive-map.gif>
Path: Map > View Rental Locations > Filter by Vehicle Availability

### API Documentation

Interactive API documentation is available via Swagger UI when running the API locally:
https://localhost:7230/swagger
![Swagger Preview] <placeholder-swagger.png>

### Website Frontend

The consumer-facing website is available in a separate repository:
[Vehicle Rental System Website](https://github.com/xxnls/vrswebsite)

