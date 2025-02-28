# Vehicle Rental System (Backoffice & API)

A comprehensive vehicle rental management system built with **WPF** for the backoffice and **ASP.NET Core Web API** for the backend. The system is designed to streamline vehicle rental operations, including HR management, customer handling, and rental processing. The database is powered by **MSSQL**, ensuring robust data management and scalability.

![Backoffice Dashboard Preview] <placeholder-dashboard-preview.gif>

-------

## Features

### Backoffice (WPF)
- **Vehicle Management**  
  Add, update, and remove vehicles with detailed specifications (e.g., type, model, price).
- **Customer Management**  
  Manage customer profiles, including personal.
- **Rental Processing**  
  Handle reservation requests, reservations, payments and post rental reports.
- **Interactive Map**
  

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

