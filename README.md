# Real-Time Chat Application

A Windows Forms application for real-time chat built with .NET 8, SQL Server, and MQTT.

## Features

- **Multi-User Support**: Admin and regular user roles
- **Real-Time Messaging**: Instant message delivery using MQTT protocol
- **Secure Communication**: Message encryption for privacy
- **Chat Rooms**: Create and manage group conversations
- **Direct Messaging**: Private conversations between users
- **User Management**: Admin dashboard for user administration
- **Modern UI**: Attractive and intuitive interface

## Technology Stack

- **.NET 8**: Latest framework for Windows Forms applications
- **SQL Server**: Robust database for storing users, messages, and chat rooms
- **Entity Framework Core**: ORM for database operations
- **MQTT Protocol**: For real-time message exchange
- **BCrypt**: For secure password hashing

## Prerequisites

Before running the application, ensure you have:

- Visual Studio 2022 (or later)
- .NET 8 SDK
- SQL Server (Local or Express)
- MQTT Broker (e.g., Mosquitto) running on port 1883

## Setup Instructions

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/ChatApp.git
cd ChatApp
```

### 2. Configure the Database

1. Open the `App.config` file and update the connection string:

```xml
<connectionStrings>
    <add name="ChatAppDb" connectionString="Data Source=YOUR_SERVER;Initial Catalog=ChatAppDb;Integrated Security=True;TrustServerCertificate=True" providerName="Microsoft.Data.SqlClient" />
</connectionStrings>
```

2. Create the database using Entity Framework migrations:

```bash
# Open Package Manager Console in Visual Studio
Add-Migration InitialCreate
Update-Database
```

### 3. MQTT Broker Setup

1. Install and run an MQTT broker (e.g., Mosquitto)
2. Ensure it's running on localhost:1883 (or update the connection settings in `MqttService.cs`)

### 4. Build and Run

1. Build the solution in Visual Studio
2. Run the application
3. Log in using the default admin credentials:
   - Username: admin
   - Password: admin123

## Project Structure

- **Models/**: Database entity models
  - `User.cs`: User model with role information
  - `Message.cs`: Message model with encryption
  - `ChatRoom.cs`: Chat room and membership models
  - `ChatAppDbContext.cs`: Entity Framework context

- **Services/**: Application services
  - `EncryptionService.cs`: Message encryption and decryption
  - `MqttService.cs`: Real-time messaging service

- **Forms/**: Windows Forms
  - `LoginForm.cs`: User authentication
  - `RegisterForm.cs`: User registration
  - `ChatForm.cs`: Main chat interface
  - `AdminDashboardForm.cs`: Admin control panel
  - `CreateChatRoomForm.cs`: Create new chat rooms
  - `ManageChatRoomForm.cs`: Chat room management
  - `EditUserForm.cs`: User profile editing

## Security Features

- **Password Hashing**: BCrypt for secure password storage
- **Message Encryption**: All messages are encrypted in the database
- **Role-Based Access Control**: Admin and user role separation

## Usage Tips

- **Creating a Chat Room**: Click "Create Room" button in the chat interface
- **Direct Messaging**: Select a user from the users list to start a chat
- **User Management**: Admins can access the dashboard for user management
- **Room Management**: Admins can manage chat rooms and memberships

## Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature-name`
3. Commit your changes: `git commit -m 'Add feature'`
4. Push to the branch: `git push origin feature-name`
5. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- MQTT.NET library for MQTT communication
- BCrypt.NET for password hashing
- Entity Framework Core team
- Microsoft .NET team
