# FAST Societies Management System 🚀

A premium, high-performance desktop application built for automating and centralizing the management of student societies at FAST-NU. Designed with a modern **"Midnight Editorial"** aesthetic, this system transitions campus activities from fragmented manual processes to a streamlined digital ecosystem.

## ✨ Key Features

### 🏛️ Administrative Oversight
- **Full Society Lifecycle**: Create, approve, suspend, or delete societies with a single click.
- **User Management**: Unified dashboard to manage Students, Society Heads, and Admins.
- **Audit Trails**: Real-time **System Logs** to track all critical administrative actions for accountability.
- **Global Control**: Override capabilities for events and membership approvals.

### 👑 Society Head Portal
- **Event Lifecycle**: Complete control to **Create, Update, and Cancel** society events.
- **Member Management**: Manage internal member lists, including removing inactive members.
- **Membership Processing**: Dedicated queue to review and approve/reject new student applications.
- **Dashboard**: Real-time stats on society growth and upcoming logistics.

### 🎓 Student Experience
- **Browse & Join**: Discover active societies and apply for memberships seamlessly.
- **Event Registration**: Stay updated with upcoming campus events and register instantly.
- **Personal Dashboard**: Track active memberships and registered event schedules.

## 🎨 Design Philosophy: "Midnight Editorial"
The system features a bespoke UI design system characterized by:
- **High-Contrast Dark Mode**: Tailored for reduced eye strain and a professional look.
- **Glassmorphism & Gradients**: Subtle visual effects for a premium, state-of-the-art feel.
- **Responsive Layouts**: Programmatically generated UI (no WinForms Designer) ensuring consistency across resolutions.

## 🛠️ Technology Stack
- **Language**: C# (.NET Core)
- **Framework**: Windows Forms (Programmatic UI)
- **Database**: Microsoft SQL Server (LocalDB / SQLEXPRESS)
- **Architecture**: N-Tier Architecture (DAL, BLL, UI layers) for maximum scalability.

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- SQL Server Express (Local instance)

### Installation
1. **Database Setup**:
   - Run the provided `database-schema-clean.sql` script on your SQL Server instance (`.\SQLEXPRESS`).
   - The system automatically targets `FASTSocietiesSystemDB`.

2. **Configuration**:
   - Update the connection string in `DAL/DatabaseConnection.cs` if your SQL instance name differs.

3. **Run the App**:
   ```bash
   cd FASTSocietiesSystem
   dotnet run
   ```

### Default Credentials
- **Admin**: `admin@fast.com` / `Admin123`
- **Student/Head**: Register via the app's sign-up screen.

## 📝 Project Structure
- `Models/`: Domain entities (User, Society, Event, Membership).
- `DAL/`: Data Access Layer handling SQL operations.
- `BLL/`: Business Logic Layer enforcing system rules.
- `UI/`: Programmatic WinForms components and custom theme helpers.

---
*Developed for SMM Project - Semester 8*
