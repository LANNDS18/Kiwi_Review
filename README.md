# Kiwi_review
A movie review api enable user can review each movie in different topics and highlight the review with customized setting.

User can create many movies, each movie contains some topics, user can write review under each topic, highlight the review that user want to concentrate on.

## Dependency
.Net6.0.1 SDK, Entityframework6.0 (Bad compatibility with arm64 architecture if using arm64 .Net6 SDK)
Install all dependencies with NuGet.

## RUN

`dotnet build` to build the project first

`dotnet run` to simply run the project

A visual api interface in https://localhost:****/swagger/index.html

## Databse

MySQL 8.0.26

Create First migration
`dotnet ef migrations add InitialCreate`

Update databse
`dotnet ef database update`

Add Changes
`dotnet ef migrations add < >`

### Note

Using `dotnet ef` to generate data model is not stable in arm64 Architecture, .Net5 is better recommded to use in this project.
