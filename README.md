# Kiwi_review
A movie review api enable user can review each movie in different topics and highlight the review with customized setting.

User can create many movies, each movie contains some topics, user can write review under each topic, highlight the review that user want to concentrate on.

## Dependency
Using .Net6.0, Entityframework6.0(Bad compatibility with arrch64 architecture, unable to use ef to migrate database).
Install all dependencies with NuGet.

## RUN

`dotnet build` to build the project first

`dotnet run` to simply run the project

A visual api interface in url /swagger/index.html

## Databse

MySQL 8.0.26

### Note

Using `dotnet ef` to generate data model is not stable in Arm64 Architecture, .Net5 is better recommded to use in this project.
