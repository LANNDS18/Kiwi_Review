# Kiwi_review
A movie review api enable user can review each movie in different topics and highlight the review with customized setting.

User can import multiple movies, each movie contains some topics, user can write review under each topic, highlight the review that user want to concentrate on.

## Dependency

.NET SDK 6.0.101 

Microsoft.AspNetCore.Authentication.JwtBearer 6.0.1

Microsoft.EntityFrameworkCore 6.0.1

Microsoft.IdentityModel.Tokens6.15.1

MySql.Data 8.0.28

MySql.EntityFrameworkCore 6.0.0

Newtonsoft.Json 13.0.1

Swashbuckle.AspNetCore 6.2.3

System.IdentityModel.Tokens.Jwt 6.15.1

Download .NET SDK in https://dotnet.microsoft.com/en-us/
Install all dependencies with NuGet.

## RUN

Build in project file

`dotnet build` 

Simply run the project

`dotnet run`

A visual api interface in https://localhost:****/swagger/index.html

## Database

MySQL 8.0.26

Create First migration

`dotnet ef migrations add InitialCreate`

Update databse

`dotnet ef database update`

Add Changes

`dotnet ef migrations add <NAME>`

### Note

Using `dotnet ef` to generate data model is not stable in arm64 Architecture, .Net5 is better recommded to use in this project.

## Deployment

`dotnet publish -h|--help`

Publish .dll to deploy in different platform.
