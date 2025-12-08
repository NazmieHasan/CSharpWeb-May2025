# CSharpWeb-May2025
 ASP.NET Core Web App (Model-View-Controller)
 
Welcome to **HotelApp**, a web application designed to demonstrate core web development skills and technologies through a room booking and management system.

---

## 🛠️ Technologies Used

### Backend
- **ASP.NET Core**: Robust and scalable web framework.
- **Entity Framework 8**: Efficient database interactions with LINQ and more.
- **Newtonsoft.Json**: Advanced JSON serialization and deserialization.

### Testing (TODO)

### Frontend
- **Bootstrap 5**: Responsive and modern UI components.

### Testing
- **NUnit **: Comprehensive testing framework.
- **Moq **: Mocking framework for unit testing.
- **MockQueryable **: Simplifying LINQ mocking for Entity Framework queries.

---

## 🚀 How to Run the Project

1. Clone the repository:
   ```bash
   git clone https://github.com/NazmieHasan/CSharpWeb-May2025.git
   ```
   
2. Connection DB: HotelApp.Web appsetings.json, HotelApp.WebApi appsetings.json


3. Create secrets.json with users


4. Navigate to the project directory:
   ```bash
   cd HotelWeb
   ```

5. Restore dependencies and apply migrations
   ```bash
   dotnet restore
   dotnet ef database update
   ```

6. Run the application:
   ```bash
   dotnet run
   ```
   
7. Configure Startup Projects: Multiple Startup Projects HotelApp.Web, HotelApp.WebApi


8. Open your browser and navigate to:
   ```
   http://localhost:{localPort}
   ```

---

## 📜 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---
