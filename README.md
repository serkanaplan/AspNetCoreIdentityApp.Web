# ASP.NET Core Identity

![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET%20Core-MVC-blue.svg)
![EF Core](https://img.shields.io/badge/EF%20Core-8.0.0-green.svg)
![Identity](https://img.shields.io/badge/Identity-orange.svg)

## Overview
This project is focused on demonstrating the functionality of the ASP.NET Core Identity library. It includes a membership system where users can register, log in, and manage their accounts. The main goal of this project is to provide a practical example of how to implement and use ASP.NET Core Identity.

## Features of ASP.NET Core Identity
- **User Registration**: Allows users to create an account with email and password.
- **User Login**: Provides secure login functionality for registered users.
- **User Roles**: Enables management of user roles and permissions.
- **Password Recovery**: Facilitates password reset via email.
- **Account Management**: Users can update their profile information.
- **Two-Factor Authentication**: Adds an extra layer of security.
- **External Login Providers**: Supports authentication via external providers like Google and Facebook.

## Technologies Used
- **ASP.NET Core MVC**: Framework for building the web application.
- **ASP.NET Core Identity**: Library for handling user authentication and authorization.
- **Entity Framework Core**: ORM for database access and manipulation.
- **SQL Server**: Database used for storing user information and identity data.

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Installation
1. Clone the repository:
    ```sh
    git clone https://github.com/yourusername/identity-membership-system.git
    ```
2. Navigate to the project directory:
    ```sh
    cd identity-membership-system
    ```
3. Restore the dependencies:
    ```sh
    dotnet restore
    ```
4. Update the database:
    ```sh
    dotnet ef database update
    ```
5. Run the application:
    ```sh
    dotnet run
    ```

## Usage
- Open your browser and navigate to `https://localhost:5001`
- Register a new user account
- Log in with the registered account
- Explore the features of the membership system


## Contact
- **Email**: serkanaplan@gmail.com
- **GitHub**: [serkanaplan](https://github.com/serkanaplan)

---

Thank you for checking out my project!

