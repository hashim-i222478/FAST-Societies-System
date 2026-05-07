# FAST Societies Management System 🚀

A premium, high-performance desktop application built for automating and centralizing the management of student societies at FAST-NU. Designed with a modern **"Midnight Editorial"** aesthetic, this system transitions campus activities from fragmented manual processes to a streamlined digital ecosystem.

## ✨ Key Features

### 🏛️ Administrative Oversight
- **Society Lifecycle Management**: Create, approve, suspend, or delete societies with a dynamic, context-aware interface.
- **University-Wide Analytics**: Generate and export (CSV) detailed reports on user distribution, society performance, and event engagement.
- **System Activity Logs**: Real-time audit trails tracking logins, registrations, and administrative actions for full accountability.
- **User Control**: Centralized dashboard to manage Students, Society Heads, and Admins including account status management.
- **Event Approvals**: Dedicated queue for reviewing and approving society-hosted events.

### 👑 Society Head Portal
- **Society Dashboard**: Overview of memberships, pending requests, and upcoming logistics.
- **Event Lifecycle**: Complete control to create, update, and cancel events for the society.
- **Task Management**: Assign and track tasks for society members to ensure smooth event execution.
- **Performance Reporting**: Generate society-specific CSV reports for member attendance and event statistics.
- **Membership Processing**: Approve or reject new student applications with a single click.

### 🎓 Student Experience
- **Browse & Join**: Discover active societies, view their profiles, and apply for membership.
- **Event Discovery**: Stay updated with campus-wide activities and register for events instantly.
- **Digital Tickets**: View and manage tickets for registered events.
- **Personal Portfolio**: Track active memberships and assigned tasks from societies.

## 🎨 Design Philosophy: "Midnight Editorial"
The system features a bespoke UI design system characterized by:
- **High-Contrast Dark Mode**: Tailored for reduced eye strain and a professional look.
- **Glassmorphism & Micro-animations**: Subtle visual effects for a premium, state-of-the-art feel.
- **Responsive Layouts**: Programmatically generated UI ensuring consistency across various screen resolutions.
- **Dynamic Interaction**: Context-sensitive controls that guide users through the correct workflows.

## 🛠️ Technology Stack
- **Language**: C# (.NET Core / .NET 10)
- **Framework**: Windows Forms (Bespoke Programmatic UI)
- **Database**: Microsoft SQL Server (LocalDB / SQLEXPRESS)
- **Architecture**: N-Tier Architecture (DAL, BLL, UI layers) for maximum scalability and clean separation of concerns.

## 🚀 Getting Started

### Prerequisites
- .NET 10.0 SDK or later
- Microsoft SQL Server 2019+ or LocalDB
- `Microsoft.Data.SqlClient` library

### Installation
1. **Clone the Repository**:
   ```bash
   git clone https://github.com/hashim-i222478/FAST-Societies-System.git
   ```

2. **Database Setup**:
   - Execute the `database-schema-clean.sql` script located in the root directory on your SQL Server instance.
   - The script creates the `FASTSocietiesSystemDB` and all necessary tables/views.

3. **Configuration**:
   - Verify the connection string in `FASTSocietiesSystem/DAL/DatabaseConnection.cs` matches your local server instance name.

4. **Build & Run**:
   ```bash
   cd FASTSocietiesSystem
   dotnet build
   dotnet run
   ```

### Default Credentials
- **Admin Access**: `admin@fast.com` / `Admin123`
- **Other Users**: Register a new account via the **Sign Up** link on the login screen.

## 📝 Project Structure
- `Models/`: Core domain entities and data structures.
- `DAL/`: Data Access Layer implementing the Repository pattern.
- `BLL/`: Business Logic Layer containing system services and validation.
- `UI/Forms/`: Custom-built programmatic Windows Forms.
- `UI/Helpers/`: Theme Manager and UI utility controls.

---
*Developed for SMM Project - Semester 8*
