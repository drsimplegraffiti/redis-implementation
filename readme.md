#### Run redis
- docker run --name my-redis -p 6379:6379 -d redis


##### Add postgres package to dotnet
- dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL 
- dotnet add package Microsoft.EntityFrameworkCore
- dotnet add package Microsoft.EntityFrameworkCore.Design
- dotnet add package Microsoft.EntityFrameworkCore.Tools

##### install redis package
- dotnet add package StackExchange.Redis
- dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis


##### Run migrations
- dotnet ef migrations add InitialCreate
- dotnet ef database update