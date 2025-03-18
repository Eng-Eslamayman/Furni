# Furnihuture - eCommerce Project

## Overview
Furnihuture is an eCommerce platform built using **ASP.NET MVC** and **jQuery**, following an **n-tier architecture** and employing best practices such as the **repository pattern with unit of work**. The project integrates **Serilog** for error handling and **Hangfire** for managing background jobs.

This project provides both **user and admin dashboards**, leveraging **DataTables** for efficient data management, supporting both **server-side and client-side** operations.

## 🚀 Features

### 🛒 eCommerce Functionality
- **Product Catalog**: Browse products with detailed descriptions, images, and pricing.
- **Shopping Cart**: Add, remove, and update items in the cart.
- **Order Management**: Process orders, generate invoices, and send notifications.
- **Customer Experience**: User-friendly interface with search and filter capabilities.

### 🔧 Admin Dashboard
- **Total Sales**: Monitor overall sales performance.
- **Recent Orders**: Track customer orders in real-time.
- **Product Ratings**: View top-rated and low-rated products.
- **Stock Levels**: Manage inventory with live stock updates.
- **User Management**: Control roles, permissions, and user authentication.

### ⚙️ Technologies Used
- **ASP.NET MVC** for the web application framework.
- **jQuery & DataTables** for interactive UI and data management.
- **Entity Framework (EF)** with **Repository Pattern & Unit of Work**.
- **SQL Server** for database management.
- **Serilog** for advanced logging and error tracking.
- **Hangfire** for scheduling background jobs like order processing and email notifications.

## 📂 Project Structure
The project follows a modular **n-tier architecture**, separating concerns into different layers:

1. **Presentation Layer**: ASP.NET MVC views and controllers.
2. **Business Logic Layer**: Service classes implementing core business functionality.
3. **Data Access Layer**: Repository pattern for database interactions.
4. **Infrastructure Layer**: Logging, background jobs, and external services.

---

## 🎯 Development Roadmap

### ✅ Completed Parts:
- **[Part 1: Database Design & Dashboard Overview]**  
  Designed a **robust database structure** focusing on relationships and data integrity.  
  Built an **admin dashboard** displaying key business metrics.

- **[Part 2: Admin Area and Product Management](https://lnkd.in/dejUgUN4)** ✅  
  Implemented **product management**, including adding, editing, and deleting products.  
  Developed **categories management** and optimized the admin experience.

- **[Part 3: Customer Experience and Shopping Cart](https://lnkd.in/dRm2nynP)** ✅  
  Enhanced **customer journey**, from browsing products to **adding them to the cart**.  
  Implemented a **secure checkout process** with order history tracking.

- **[Part 4: Identity Management and Two-Factor Authentication](https://lnkd.in/dxfbz3vh)** ✅  
  Integrated **user authentication**, including registration, login, and **role-based access control (RBAC)**.  
  Added **two-factor authentication (2FA)** for enhanced security.

- **[Part 5: Logging and Error Handling](https://lnkd.in/dY_SRNq)** ✅  
  Configured **Serilog** for structured logging and real-time error tracking.  
  Implemented a **global exception handler** to improve system reliability.

---

## 🛠️ Installation & Setup

1. **Clone the repository**:
   ```bash
   git clone https://github.com/yourusername/furnihuture.git
   cd furnihuture
