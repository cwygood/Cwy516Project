#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM 192.168.152.201/my/aspnet:v3.1 AS base
WORKDIR /app
EXPOSE 80

COPY . .
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "Cwy516Project.dll"]