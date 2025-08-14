# Use the official .NET 8 runtime image as the base image
FROM mcr.microsoft.com/dotnet/runtime:8.0-alpine AS base
WORKDIR /app
EXPOSE 5000

# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["DumpertMCP/DumpertMCP.csproj", "DumpertMCP/"]
RUN dotnet restore "DumpertMCP/DumpertMCP.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR "/src/DumpertMCP"

# Build the application
RUN dotnet build "DumpertMCP.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "DumpertMCP.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the entry point
ENTRYPOINT ["dotnet", "DumpertMCP.dll"]

