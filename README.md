# CookingAssistantBackend
Backend project for cooking assistant website made in ASP.NET Core Web Api. 

University assignment for programming course.


## Tech details:
- ASP.NET Core Web Api project
- Entity Framework Core for communicating with SQL Server Database.
- User authentication based on JWT tokens and hashed passwords.
- AutoMapper for creating Data Transfer Objects
- Configuration secrets supplied from Azure App Configuration

## Features:
- Browsing through user-added recipes catalogue (searching by name or tags)
- Each recipe is divided to steps for easy step by step cooking instructions
- Users can create their accounts and add new recipes
- Users can comment steps and suggest changes
- Comments can be liked by users, most liked comments will be shown when doing that step
- Premade recipe categories used for displaying appropriate icon
