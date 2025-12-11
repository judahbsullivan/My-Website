# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["api/api.csproj", "api/"]
RUN dotnet restore "api/api.csproj"

# Copy everything else and build
COPY api/ ./api/
RUN dotnet build "api/api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "api/api.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Copy published app
COPY --from=publish /app/publish .

# Expose port (Fly.io will set PORT env var, default to 8080)
EXPOSE 8080

# Set environment
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the app
ENTRYPOINT ["dotnet", "api.dll"]