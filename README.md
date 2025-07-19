# Classroom Role Roulette

## Overview
This console application was developed for the "Fundamentos de Programación" course at Instituto Tecnológico De Las Americas (ITLA). The application is a role assignment tool that randomly distributes different classroom roles among students using a roulette-style selection system.

## Features

### Student Management
- View a list of all registered students
- Add new students to the system
- Edit student information (name, assigned roles)
- Remove students from the system

### Role Management
- View a list of all available roles
- Add new roles to the system
- Edit existing roles
- Remove roles from the system

### Role Roulette
- Select specific roles to be assigned in the current spin
- Randomly assign selected roles to students
- Visual roulette animation during the selection process
- Option to re-spin the roulette with the same role selection
- Prevent assigning the same role to a student who already has it
- Prevent selecting the same student twice in a single roulette spin

### History Tracking
- Automatically save role assignment history with timestamps
- View historical assignment records
- Records are organized by time (e.g., "Assignment from 2 hours ago")

## Technical Details

### Libraries Used
- Spectre.Console: For rich console UI elements (tables, selection menus, animations)
- Newtonsoft.Json: For JSON serialization/deserialization of data
- Humanizer: For user-friendly time representations

### Data Persistence
- Students data is stored in `estudiantes.txt`
- Roles data is stored in `roles.txt`
- Assignment history is stored in `registros/registro_YYYY-MM-DD_HH-MM-SS.txt` files

### Project Structure
- **Program.cs**: Main application logic and UI navigation
- **Student.cs**: Student class with methods for student management
- **Roulette.cs**: Roulette class with methods for role assignment and animation
- **helpers.cs**: Helper functions for arrays and console operations

## Getting Started

### Prerequisites
- .NET Core 6.0 or higher
- Required NuGet packages:
  - Spectre.Console
  - Humanizer
  - Newtonsoft.Json

### Installation
1. Clone this repository
2. Ensure all required NuGet packages are installed using `dotnet restore`
3. Run the application using `dotnet run`

### Usage
The application features an interactive menu system:
1. **Ruleta (Roulette)**: Spin the roulette to assign roles
2. **Estudiantes (Students)**: Manage the list of students
3. **Roles**: Manage the available roles
4. **Ver registros (View Records)**: Access the history of role assignments
5. **Salir (Exit)**: Exit the application

## Educational Purpose
This project demonstrates fundamental programming concepts including:
- Object-oriented programming
- File I/O operations
- User interface design
- Random number generation
- Array manipulation
- Error handling
- Data persistence


## Author
> Jorge Raynieri Sanchez Pichardo 2025 - 1023

## Acknowledgments
- Instituto Tecnológico De Las Americas (ITLA)
- Professor of "Fundamentos de Programación"
