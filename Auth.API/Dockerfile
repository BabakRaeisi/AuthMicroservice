FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["Auth.API/Auth.API.csproj", "Auth.API/"]
COPY ["Auth.Core/Auth.Core.csproj", "Auth.Core/"]
COPY ["Auth.Infrastructure/Auth.Infrastructure.csproj", "Auth.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "Auth.API/Auth.API.csproj"

# Copy source code
COPY . .
WORKDIR "/src/Auth.API"

# Build
RUN dotnet build "Auth.API.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "Auth.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 5001
ENV ASPNETCORE_URLS=http://+:5001
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "Auth.API.dll"]