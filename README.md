# Multi-Service Appointment Manager

A comprehensive web application for managing appointments across multiple services and providers.  
Built using ASP.NET Core MVC and Entity Framework Core with a SQLite database, this project aims to streamline appointment booking, management, and analysis for service-based businesses.

---

## Table of Contents

- [Features](#features)  
- [Technologies Used](#technologies-used)  
- [Getting Started](#getting-started)  
- [Project Structure](#project-structure)  
- [Database Schema](#database-schema)  
- [Dashboard Overview](#dashboard-overview)  
- [Contact](#contact)  

---

## Features

### Appointment Management
- Create, edit, delete, and view appointments with date, time, assigned provider, and selected service.  
- Date and provider filters on the appointments page to easily locate appointments by day or service provider.  
- Validation to prevent incorrect date/time entries.

### Provider and Service Management
- CRUD operations for providers and services.  
- Foreign key constraints ensure referential integrity (cannot delete providers with active appointments).  
- Charges associated with services to calculate revenue.

### Dashboard with Visual Insights
- **Daily Appointments Count:** Total appointments scheduled for today.  
- **Daily and Weekly Revenue:** Total charges from appointments aggregated by day and week.  
- **Top Providers This Week:** Visualized with a pie chart displaying the providers with the highest booking counts.  
- **Top Services This Week:** Displayed as a pie chart highlighting the most booked services.  
- **Upcoming Appointments:** List of the next 5 upcoming appointments to prepare your schedule.

### Screenshots
- Dashboard:  ![Dashboard](Screenshots/1.%20Dashboard.jpeg)
- Providers Main: ![Providers Main](Screenshots/2.%20Providers%20Main.jpeg)
- Add New Provider: ![Add New Provider](Screenshots/3.%20Providers%20-%20Add%20New%20Provider.jpeg)
- Edit Providers: ![Edit Provider](Screenshots/4.%20Provider%20-%20Edit.jpeg)
- Provider Detail: ![Provider Detail](5.%20Providers%20-%20Details.jpeg)
- Delete Providers: ![Delete Providers](Screenshots/6.%20Providers%20-%20Delete.jpeg)
- Service Main: ![Service Main](Screenshots/7.%20Services%20-%20Main.jpeg)
- Add New Service: ![Add New Service](Screenshots/8.%20Services%20-%20Add%20New%20Service.jpeg)
- Edit Service: ![Edit Service](Screenshots/9.%20Services%20-%20Edit.jpeg)
- Delete Service: ![Delete Service](Screenshots/10.%20Services%20-%20Delete.jpeg)
- Appointment Main Page: ![Appointment Main](Screenshots/11.%20Appointments%20-%20Main.jpeg)
- Create New Appointment: ![Create New Appointment](Screenshots/12.%20Appointments%20-%20Create%20Appointment.jpeg)
- Edit Appointment: ![Edit Appointment](Screenshots/13.%20Appointments%20-%20Edit.jpeg)
- Appointment Detail: ![Appointment Detail](Screenshots/14.%20Appointments%20-%20Details.jpeg)
- Delete Appointment: ![Delete Appointment](Screenshots/15.%20Appointments%20-%20Delete.jpeg)
- Date Filter: ![Date Filter](Screenshots/16.%20Appointments%20-%20Date%20filter.jpeg)
- Provider Filter: ![Provider Filter](Screenshots/17.%20Appointments%20-%20Provider%20filter.jpg)

### UI and UX
- Responsive layout using Bootstrap 5.  
- Modern and clean UI with intuitive filtering and navigation.  
- Fixed navigation bar for easy access to pages.  
- Custom styling to highlight important data and improve readability.

---

## Technologies Used

- **ASP.NET Core MVC** — Web application framework for building dynamic websites  
- **Entity Framework Core** — ORM for database access using C# classes and LINQ  
- **SQLite** — Lightweight, file-based relational database for development and deployment  
- **Bootstrap 5** — Responsive CSS framework for layout and design  
- **Chart.js** — JavaScript library for rendering interactive charts and graphs  
- **Razor Views** — Server-side rendering of UI components with C# integration  

---

## Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) or higher  
- IDE such as Visual Studio 2022, Visual Studio Code with C# extensions  
- Git (for cloning the repository)  

### Installation and Setup

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/asma-attique/MultiServiceAppointmentManager.git
   ```
2. **Navigate into the Project Folder:**

   ```bash
    cd MultiServiceAppointmentManager
    ```

3. **Restore NuGet Packages:**

    ```bash
     dotnet restore
    ```
     
4. **Apply Database Migrations:**

    ```bash
    dotnet ef database update
    ```
    This creates the SQLite database file and schema.

5. **Run the Application:**

    ```bash
    dotnet run
    ```
    or 
  
    ```bash
    dotnet watch run
    ```
    
6. **Open in Browser:**

    Navigate to https://localhost:5001 (or the URL shown in your console) to view the app.
   

---
## Project Structure
```pgsql
/Controllers
    AppointmentsController.cs
    ProvidersController.cs
    ServicesController.cs
    HomeController.cs
/Data
    AppDbContext.cs
    Migrations/
/Models
    Appointment.cs
    Provider.cs
    Service.cs
/Views
    /Appointments
        Index.cshtml
        Create.cshtml
        Edit.cshtml
        Delete.cshtml
    /Providers
    /Services
    /Home
        Index.cshtml (Dashboard)
/wwwroot
    /css
    /js
```
---
## Database Schema

- **Appointment: Stores appointment date, time, related provider, service, and customer name.

- **Provider: Service providers with details such as name and contact.

- **Service: Services offered with name and associated charges.

### Relationships:

- **Appointment references one Provider and one Service (foreign keys).

- **Providers and Services can have multiple appointments.
---
## Dashboard Overview

- **The home page dashboard provides at-a-glance insights:

- **Revenue Today & This Week: Shows earnings calculated from booked services.

- **Top Providers & Services: Pie charts visualize the most active providers and popular services over the last week.

- **Upcoming Appointments: A concise table listing the next 5 scheduled appointments to prepare for the day.

- **Charts are implemented using Chart.js, with colors chosen for clarity and visual appeal.

---
## Contact
- **Asma Attique
- **Software Engineer | Computer Engineer
- **Email: asma.attique.20gmail.com
- **LinkedIn: http://linkedin.com/in/asmaattique20
---

Thank you for exploring the Multi-Service Appointment Manager project! Contributions and suggestions are welcome.
