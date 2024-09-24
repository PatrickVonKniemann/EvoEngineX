# Use the official .NET image for the runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj and restore dependencies separately
COPY ["InfrastructureTestCode.csproj", "./"]
RUN dotnet restore "InfrastructureTestCode.csproj"

# Copy the rest of the source code
COPY . .

WORKDIR "/src"
RUN dotnet build "InfrastructureTestCode.csproj" -c Release -o /app/build
RUN dotnet publish "InfrastructureTestCode.csproj" -c Release -o /app/publish

# Build the runtime container
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "InfrastructureTestCode.dll"]
