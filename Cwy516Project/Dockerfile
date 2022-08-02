#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM 192.168.98.249/test/aspnet3.1:v1 AS base
WORKDIR /app
EXPOSE 80

FROM 192.168.98.249/test/sdk3.1:v1 AS build
WORKDIR /src
COPY ["Cwy516Project/Cwy516Project.csproj", "Cwy516Project/"]
COPY ["Infrastructure.Dapper/Infrastructure.Dapper.csproj", "Infrastructure.Dapper/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Infrastructure.Polly/Infrastructure.Polly.csproj", "Infrastructure.Polly/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infrastructure.Jaeger/Infrastructure.Jaeger.csproj", "Infrastructure.Jaeger/"]
COPY ["Infrastructure.IdentityServer/Infrastructure.IdentityServer.csproj", "Infrastructure.IdentityServer/"]
COPY ["Infrastructure.EntityFrameworkCore/Infrastructure.EntityFrameworkCore.csproj", "Infrastructure.EntityFrameworkCore/"]
COPY ["Infrastructure.MyOrm/Infrastructure.MyOrm.csproj", "Infrastructure.MyOrm/"]
COPY ["Infrastructure.Ocelot/Infrastructure.Ocelot.csproj", "Infrastructure.Ocelot/"]
COPY ["Infrastructure.MongoDB/Infrastructure.MongoDB.csproj", "Infrastructure.MongoDB/"]
COPY ["Infrastructure.Consul/Infrastructure.Consul.csproj", "Infrastructure.Consul/"]
RUN dotnet restore "Cwy516Project/Cwy516Project.csproj"
COPY . .
WORKDIR "/src/Cwy516Project"
RUN dotnet build "Cwy516Project.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cwy516Project.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cwy516Project.dll"]